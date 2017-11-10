#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.

using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	///@brief This type represents the base for all Wwise Types that require a GUID.
	public class BaseType
	{
#if UNITY_EDITOR
		public byte[] valueGuid = new byte[16];
#endif

		public int ID = 0;

		protected uint GetID() { return (uint)ID; }

		protected virtual bool IsValid()
		{
			return ID != 0;
		}

		public bool Validate()
		{
			if (IsValid())
				return true;

			Debug.LogWarning("Wwise ID has not been resolved. Consider picking a new " + GetType().Name + ".");
			return false;
		}

		protected void Verify(AKRESULT result)
		{
#if UNITY_EDITOR
			if (result != AKRESULT.AK_Success)
				Debug.LogWarning("Unsuccessful call made on " + GetType().Name + ".");
#endif
		}
	}

	[Serializable]
	///@brief This type represents the base for all Wwise Types that also require a group GUID, such as State and Switch.
	public class BaseGroupType : BaseType
	{
#if UNITY_EDITOR
		public byte[] groupGuid = new byte[16];
#endif

		public int groupID = 0;

		protected uint GetGroupID() { return (uint)groupID; }

		protected override bool IsValid() { return base.IsValid() && groupID != 0; }
	}

	[Serializable]
	///@brief This type represents the values of the flags used when posting an Event with a callback.
	public class CallbackFlags
	{
		public uint value = 0;
	}

	[Serializable]
	///@brief This type can be used to post events to the sound engine.
	public class Event : BaseType
	{
		private void VerifyPlayingID(uint playingId)
		{
#if UNITY_EDITOR
			if (playingId == AkSoundEngine.AK_INVALID_PLAYING_ID)
				Debug.LogError("Could not post event ID \"" + GetID() + "\". Did you make sure to load the appropriate SoundBank?");
#endif
		}

		/// <summary>
		/// Posts this event on a gameobject.
		/// </summary>
		/// <param name="gameObject">The GameObject</param>
		/// <returns>Returns the playing ID.</returns>
		public uint Post(GameObject gameObject)
		{
			if (!IsValid())
				return AkSoundEngine.AK_INVALID_PLAYING_ID;

			uint playingId = AkSoundEngine.PostEvent(GetID(), gameObject);
			VerifyPlayingID(playingId);
			return playingId;
		}

		/// <summary>
		/// Posts this event on a gameobject.
		/// </summary>
		/// <param name="gameObject">The GameObject</param>
		/// <param name="flags"></param>
		/// <param name="callback"></param>
		/// <param name="cookie">Optional cookie received by the callback</param>
		/// <returns>Returns the playing ID.</returns>
		public uint Post(GameObject gameObject, CallbackFlags flags, AkCallbackManager.EventCallback callback, object cookie = null)
		{
			if (!IsValid())
				return AkSoundEngine.AK_INVALID_PLAYING_ID;

			uint playingId = AkSoundEngine.PostEvent(GetID(), gameObject, flags.value, callback, cookie);
			VerifyPlayingID(playingId);
			return playingId;
		}

		/// <summary>
		/// Posts this event on a gameobject.
		/// </summary>
		/// <param name="gameObject">The GameObject</param>
		/// <param name="flags"></param>
		/// <param name="callback"></param>
		/// <param name="cookie">Optional cookie received by the callback</param>
		/// <returns>Returns the playing ID.</returns>
		public uint Post(GameObject gameObject, uint flags, AkCallbackManager.EventCallback callback, object cookie = null)
		{
			if (!IsValid())
				return AkSoundEngine.AK_INVALID_PLAYING_ID;

			uint playingId = AkSoundEngine.PostEvent(GetID(), gameObject, flags, callback, cookie);
			VerifyPlayingID(playingId);
			return playingId;
		}

		public void Stop(GameObject gameObject, int transitionDuration = 0, AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
		{
			ExecuteAction(gameObject, AkActionOnEventType.AkActionOnEventType_Stop, transitionDuration, curveInterpolation);
		}

		/// <summary>
		/// Executes various actions on this event associated with a gameobject.
		/// </summary>
		/// <param name="gameObject">The GameObject</param>
		/// <param name="actionOnEventType"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="curveInterpolation"></param>
		public void ExecuteAction(GameObject gameObject, AkActionOnEventType actionOnEventType, int transitionDuration, AkCurveInterpolation curveInterpolation)
		{
			if (IsValid())
			{
				AKRESULT result = AkSoundEngine.ExecuteActionOnEvent(GetID(), actionOnEventType, gameObject, transitionDuration, curveInterpolation);
				Verify(result);
			}
		}
	}

	[Serializable]
	///@brief This type can be used to set game parameter values to the sound engine.
	public class RTPC : BaseType
	{
		public void SetValue(GameObject gameObject, float value)
		{
			if (IsValid())
			{
				AKRESULT result = AkSoundEngine.SetRTPCValue(GetID(), value, gameObject);
				Verify(result);
			}
		}

		public void SetGlobalValue(float value)
		{
			if (IsValid())
			{
				AKRESULT result = AkSoundEngine.SetRTPCValue(GetID(), value);
				Verify(result);
			}
		}
	}

	[Serializable]
	///@brief This type can be used to post triggers to the sound engine.
	public class Trigger : BaseType
	{
		public void Post(GameObject gameObject)
		{
			if (IsValid())
			{
				AKRESULT result = AkSoundEngine.PostTrigger(GetID(), gameObject);
				Verify(result);
			}
		}
	}

	[Serializable]
	///@brief This type can be used to set Wwise states.
	public class State : BaseGroupType
	{
		public void SetValue()
		{
			if (IsValid())
			{
				AKRESULT result = AkSoundEngine.SetState(GetGroupID(), GetID());
				Verify(result);
			}
		}
	}

	[Serializable]
	///@brief This type can be used to set switch values on gameobjects.
	public class Switch : BaseGroupType
	{
		public void SetValue(GameObject gameObject)
		{
			if (IsValid())
			{
				AKRESULT result = AkSoundEngine.SetSwitch(GetGroupID(), GetID(), gameObject);
				Verify(result);
			}
		}
	}

	[Serializable]
	///@brief This type represents an auxiliary send in the master-mixer hierarchy.
	public class AuxBus : BaseType
	{
	}

	[Serializable]
	///@brief This type can be used to load/unload soundbanks.
	public class Bank : BaseType
	{
		public string name;

		public void Load(bool decodeBank = false, bool saveDecodedBank = false)
		{
			if (IsValid())
				AkBankManager.LoadBank(name, decodeBank, saveDecodedBank);
		}

		public void LoadAsync(AkCallbackManager.BankCallback callback = null)
		{
			if (IsValid())
				AkBankManager.LoadBankAsync(name, callback);
		}

		public void Unload()
		{
			if (IsValid())
				AkBankManager.UnloadBank(name);
		}

		protected override bool IsValid() { return name.Length != 0 || base.IsValid(); }
	}
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.