#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class AkGameObjListenerList : AkAudioListener.BaseListenerList
{
	[SerializeField]
	private List<AkAudioListener> initialListenerList = new List<AkAudioListener>();

	public void Initialize()
	{
		int Count = initialListenerList.Count;
		for (int ii = 0; ii < Count; ++ii)
		{
			if (Add(initialListenerList[ii]))
			{
				changed = true;
			}
		}
	}

	public void Refresh(GameObject gameObject)
	{
		if (changed && gameObject != null)
		{
			changed = false;

			var Ids = GetListenerIds();
			if (Ids != null && Ids.Length > 0)
				AkSoundEngine.SetListeners(gameObject, Ids, (uint)Ids.Length);
			else
				AkSoundEngine.ResetListenersToDefault(gameObject);
		}
	}

#if UNITY_EDITOR
	public void AddToInitialListenerList(AkAudioListener listener)
	{
		if (!initialListenerList.Contains(listener))
			initialListenerList.Add(listener);
	}

	public void RemoveFromInitialListenerList(AkAudioListener listener)
	{
		if (initialListenerList.Contains(listener))
			initialListenerList.Remove(listener);
	}
#endif

}

#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.