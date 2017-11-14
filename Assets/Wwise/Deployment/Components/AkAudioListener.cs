#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections.Generic;


[AddComponentMenu("Wwise/AkAudioListener")]
[RequireComponent(typeof(AkGameObj))]
[DisallowMultipleComponent]
///@brief Add this script on the game object that represent a listener.  This is normally added to the Camera object or the Player object, but can be added to any game object when implementing 3D busses.  \c isDefaultListener determines whether the game object will be considered a default listener - a listener that automatically listens to all game objects that do not have listeners attached to their AkGameObjListenerList's.
/// \sa
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__listeners.html" target="_blank">Integrating Listeners</a> (Note: This is described in the Wwise SDK documentation.)
public class AkAudioListener : MonoBehaviour
{
	public bool isDefaultListener = true;
	private ulong akGameObjectID = AkSoundEngine.AK_INVALID_GAME_OBJECT;

	private void Awake()
	{
		akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		SetIsDefaultListener_NoCheck(isDefaultListener);
	}

	private void SetIsDefaultListener_NoCheck(bool isDefault)
	{
		if (isDefault)
			DefaultListeners.Add(this);
		else
			DefaultListeners.Remove(this);
	}

	public void SetIsDefaultListener(bool isDefault)
	{
		if (isDefaultListener != isDefault)
		{
			isDefaultListener = isDefault;
			SetIsDefaultListener_NoCheck(isDefault);
		}
	}

	private void OnEnable()
	{
		SetIsDefaultListener_NoCheck(isDefaultListener);
	}

	private void OnDisable()
	{
		SetIsDefaultListener_NoCheck(false);
	}

	private void OnDestroy()
	{
		SetIsDefaultListener(false);
		akGameObjectID = AkSoundEngine.AK_INVALID_GAME_OBJECT;
	}

	public ulong GetAkGameObjectID()
	{
		return akGameObjectID;
	}

	public class BaseListenerList
	{
		// @todo: Use HashSet<ulong> and CopyTo() with a private ulong[]
		private List<ulong> listenerIdList = new List<ulong>();

		protected bool changed = false;

		/// <summary>
		/// Uniquely adds listeners to the list
		/// </summary>
		/// <param name="listener"></param>
		/// <returns></returns>
		public bool Add(AkAudioListener listener)
		{
			if (listener == null)
				return false;

			var gameObjectId = listener.GetAkGameObjectID();
			if (listenerIdList.Contains(gameObjectId))
				return false;

			listenerIdList.Add(gameObjectId);
			changed = true;
			return true;
		}

		/// <summary>
		/// Removes listeners from the list
		/// </summary>
		/// <param name="listener"></param>
		/// <returns></returns>
		public bool Remove(AkAudioListener listener)
		{
			if (listener == null)
				return false;

			var gameObjectId = listener.GetAkGameObjectID();
			if (!listenerIdList.Contains(gameObjectId))
				return false;

			listenerIdList.Remove(gameObjectId);
			changed = true;
			return true;
		}

		public ulong[] GetListenerIds()
		{
			return listenerIdList.ToArray();
		}
	}

	public class DefaultListenerList : BaseListenerList
	{
		internal void Refresh()
		{
			if (changed)
			{
				changed = false;
				var Ids = GetListenerIds();
				AkSoundEngine.SetDefaultListeners(Ids, (uint)Ids.Length);
			}
		}
	}

	public static DefaultListenerList DefaultListeners { get { return defaultListeners; } }
	private static DefaultListenerList defaultListeners = new DefaultListenerList();

	#region WwiseMigration

#pragma warning disable 0414 // private field assigned but not used.

	[SerializeField]
	// Wwise v2016.2 and below supported up to 8 listeners[0-7].
	public int listenerId = 0;

#pragma warning restore 0414 // private field assigned but not used.

	public void Migrate14()
	{
		bool wasDefaultListener = (listenerId == 0);
		Debug.Log("WwiseUnity: AkAudioListener.Migrate14 for " + gameObject.name);
		isDefaultListener = wasDefaultListener;
	}

	#endregion
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.