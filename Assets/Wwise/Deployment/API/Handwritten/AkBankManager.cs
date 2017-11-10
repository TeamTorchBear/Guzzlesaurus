#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System;

/// @brief Maintains the list of loaded SoundBanks loaded. This is currently used only with AkAmbient objects.
public static class AkBankManager
{
	class BankHandle
	{
		protected int m_RefCount = 0;
		protected uint m_BankID;
		public string bankName;

		public BankHandle(string name)
		{
			bankName = name;
		}

		public int RefCount
		{
			get
			{
				return m_RefCount;
			}
		}

		/// Loads a bank. This version blocks until the bank is loaded. See AK::SoundEngine::LoadBank for more information.
		public virtual AKRESULT DoLoadBank()
		{
			return AkSoundEngine.LoadBank(bankName, AkSoundEngine.AK_DEFAULT_POOL_ID, out m_BankID);
		}

		public void LoadBank()
		{
			if (m_RefCount == 0)
			{
				// There might be a case where we were asked to unload the SoundBank, but then asked immediately after to load that bank.
				// If that happens, there will be a short amount of time where the ref count will be 0, but the bank will still be in memory.
				// In that case, we do not want to unload the bank, so we have to remove it from the list of pending bank unloads.
				if (BanksToUnload.Contains(this))
					BanksToUnload.Remove(this);
				else
				{
					AKRESULT res = DoLoadBank();
					LogLoadResult(res);
				}
			}

			IncRef();
		}

		/// Unloads a bank.
		public virtual void UnloadBank()
		{
			AkSoundEngine.UnloadBank(m_BankID, IntPtr.Zero, null, null);
		}

		public void IncRef()
		{
			m_RefCount++;
		}

		public void DecRef()
		{
			m_RefCount--;
			if (m_RefCount == 0)
				BanksToUnload.Add(this);
		}

		protected void LogLoadResult(AKRESULT result)
		{
			if (result != AKRESULT.AK_Success)
				Debug.LogWarning("WwiseUnity: Bank " + bankName + " failed to load (" + result.ToString() + ")");
		}
	}

	class AsyncBankHandle : BankHandle
	{
		AkCallbackManager.BankCallback bankCallback = null;

		public AsyncBankHandle(string name, AkCallbackManager.BankCallback callback) : base(name)
		{
			bankCallback = callback;
		}

		static void GlobalBankCallback(uint in_bankID, IntPtr in_pInMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie)
		{
			m_Mutex.WaitOne();
			var handle = (AsyncBankHandle)in_Cookie;
			var callback = handle.bankCallback;
			if (in_eLoadResult != AKRESULT.AK_Success)
			{
				handle.LogLoadResult(in_eLoadResult);
				m_BankHandles.Remove(handle.bankName);
			}
			m_Mutex.ReleaseMutex();

			if (callback != null)
				callback(in_bankID, in_pInMemoryBankPtr, in_eLoadResult, in_memPoolId, null);
		}

		/// Loads a bank.  This version returns right away and loads in background. See AK::SoundEngine::LoadBank for more information
		public override AKRESULT DoLoadBank()
		{
			return AkSoundEngine.LoadBank(bankName, GlobalBankCallback, this, AkSoundEngine.AK_DEFAULT_POOL_ID, out m_BankID);
		}
	}

	class DecodableBankHandle : BankHandle
	{
		bool decodeBank = true;
		bool saveDecodedBank;
		string decodedBankPath;

		public DecodableBankHandle(string name, bool save) : base(name)
		{
			saveDecodedBank = save;

			var bankFileName = bankName + ".bnk";

			// test language-specific decoded file path
			var language = AkInitializer.GetCurrentLanguage();
			decodedBankPath = System.IO.Path.Combine(AkInitializer.GetDecodedBankFullPath(), language);
			var decodedBankFilePath = System.IO.Path.Combine(decodedBankPath, bankFileName);

			bool decodedFileExists = System.IO.File.Exists(decodedBankFilePath);
			if (!decodedFileExists)
			{
				// test non-language-specific decoded file path
				decodedBankPath = AkInitializer.GetDecodedBankFullPath();
				decodedBankFilePath = System.IO.Path.Combine(decodedBankPath, bankFileName);
				decodedFileExists = System.IO.File.Exists(decodedBankFilePath);
			}

			if (decodedFileExists)
			{
				try
				{
					var decodedFileTime = System.IO.File.GetLastWriteTime(decodedBankFilePath);
					var defaultBankPath = AkBasePathGetter.GetSoundbankBasePath();
					var encodedBankFilePath = System.IO.Path.Combine(defaultBankPath, bankFileName);
					var encodedFileTime = System.IO.File.GetLastWriteTime(encodedBankFilePath);

					decodeBank = (decodedFileTime <= encodedFileTime);
				}
				catch
				{
					// Assume the decoded bank exists, but is not accessible. Re-decode it anyway, so we do nothing.
				}
			}
		}

		/// Loads a bank. This version blocks until the bank is loaded. See AK::SoundEngine::LoadBank for more information.
		public override AKRESULT DoLoadBank()
		{
			if (decodeBank)
				return AkSoundEngine.LoadAndDecodeBank(bankName, saveDecodedBank, out m_BankID);

			AKRESULT res = AKRESULT.AK_Success;

			if (!string.IsNullOrEmpty(decodedBankPath))
				res = AkSoundEngine.SetBasePath(decodedBankPath);

			if (res == AKRESULT.AK_Success)
			{
				res = AkSoundEngine.LoadBank(bankName, AkSoundEngine.AK_DEFAULT_POOL_ID, out m_BankID);

				if (!string.IsNullOrEmpty(decodedBankPath))
					AkSoundEngine.SetBasePath(AkBasePathGetter.GetSoundbankBasePath());
			}

			return res;
		}

		/// Unloads a bank.
		public override void UnloadBank()
		{
			if (decodeBank && !saveDecodedBank)
				AkSoundEngine.PrepareBank(PreparationType.Preparation_Unload, m_BankID);
			else
				base.UnloadBank();
		}
	}

	static Dictionary<string, BankHandle> m_BankHandles = new Dictionary<string, BankHandle>();
	static List<BankHandle> BanksToUnload = new List<BankHandle>();

	static public void DoUnloadBanks()
	{
		int count = BanksToUnload.Count;
		for (int i = 0; i < count; ++i)
			BanksToUnload[i].UnloadBank();

		BanksToUnload.Clear();
	}

	static public void Reset()
	{
		m_BankHandles.Clear();
		BanksToUnload.Clear();
	}

	/// Loads a SoundBank. This version blocks until the bank is loaded. See AK::SoundEngine::LoadBank for more information.
	public static void LoadBank(string name, bool decodeBank, bool saveDecodedBank)
	{
#if UNITY_SWITCH
		// No bank decoding on Nintendo switch
		decodeBank = false;
#endif

		m_Mutex.WaitOne();
		BankHandle handle = null;
		if (!m_BankHandles.TryGetValue(name, out handle))
		{
			handle = decodeBank ? new DecodableBankHandle(name, saveDecodedBank) : new BankHandle(name);
			m_BankHandles.Add(name, handle);
			m_Mutex.ReleaseMutex();
			handle.LoadBank();
		}
		else
		{
			// Bank already loaded, increment its ref count.
			handle.IncRef();
			m_Mutex.ReleaseMutex();
		}
	}

	/// Loads a SoundBank. This version returns right away and loads in background. See AK::SoundEngine::LoadBank for more information.
	public static void LoadBankAsync(string name, AkCallbackManager.BankCallback callback = null)
	{
		m_Mutex.WaitOne();
		BankHandle handle = null;
		if (!m_BankHandles.TryGetValue(name, out handle))
		{
			AsyncBankHandle asyncHandle = new AsyncBankHandle(name, callback);
			m_BankHandles.Add(name, asyncHandle);
			m_Mutex.ReleaseMutex();
			asyncHandle.LoadBank();
		}
		else
		{
			// Bank already loaded, increment its ref count.
			handle.IncRef();
			m_Mutex.ReleaseMutex();
		}
	}

	/// Unloads a SoundBank. See AK::SoundEngine::UnloadBank for more information.
	public static void UnloadBank(string name)
	{
		m_Mutex.WaitOne();
		BankHandle handle = null;
		if (m_BankHandles.TryGetValue(name, out handle))
		{
			handle.DecRef();
			if (handle.RefCount == 0)
				m_BankHandles.Remove(name);
		}
		m_Mutex.ReleaseMutex();
	}

	static System.Threading.Mutex m_Mutex = new System.Threading.Mutex();
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.