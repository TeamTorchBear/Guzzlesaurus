#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections.Generic;


/// <summary>
/// Event callback information.
/// Event callback functions can receive this structure as a parameter
/// </summary>
public class AkEventCallbackMsg
{
	public AkCallbackType	type;	///AkSoundEngine.PostEvent callback flags. See the AkCallbackType enumeration for a list of all callbacks
	public GameObject		sender;	///GameObject from whom the callback function was called  
	public AkCallbackInfo info;   ///For more information about the event callback, see the classes derived from AkCallbackInfo.
}

[AddComponentMenu("Wwise/AkEvent")]
[RequireComponent(typeof(AkGameObj))]
/// @brief Helper class that knows a Wwise Event and when to trigger it in Unity.
/// \sa
/// - \ref unity_use_AkEvent_AkAmbient
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__events.html" target="_blank">Integration Details - Events</a> (Note: This is described in the Wwise SDK documentation.)
public class AkEvent : AkUnityEventHandler 
{
#if UNITY_EDITOR
	public byte[] valueGuid = new byte[16];
#endif

	/// ID of the Event as found in the WwiseID.cs file
    public int eventID = 0;
	/// Game object onto which the Event will be posted.  By default, when empty, it is posted on the same object on which the component was added.
	public GameObject soundEmitterObject = null;
	/// Enables additional options to reuse existing events.  Use it to transform a Play event into a Stop event without having to define one in the Wwise Project.
    public bool enableActionOnEvent = false;
	/// Replacement action.  See AK::SoundEngine::ExecuteEventOnAction()
    public AkActionOnEventType actionOnEventType = AkActionOnEventType.AkActionOnEventType_Stop;
	/// Fade curve to use with the new Action.  See AK::SoundEngine::ExecuteEventOnAction() 
    public AkCurveInterpolation curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear;
	/// Duration of the fade.  See AK::SoundEngine::ExecuteEventOnAction() 
    public float transitionDuration = 0.0f;
	//
	public AkEventCallbackData m_callbackData = null;

	private void Callback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
	{
		for(int i = 0; i < m_callbackData.callbackFunc.Count; i++)
		{
			if(((int)in_type & m_callbackData.callbackFlags[i]) != 0 && m_callbackData.callbackGameObj[i] != null)
			{
				AkEventCallbackMsg callbackInfo = new AkEventCallbackMsg();
				callbackInfo.type	= in_type;
				callbackInfo.sender	= gameObject;
				callbackInfo.info	= in_info;

				m_callbackData.callbackGameObj[i].SendMessage(m_callbackData.callbackFunc[i], callbackInfo);
			}
		}
	}
    public uint playingId = AkSoundEngine.AK_INVALID_PLAYING_ID;
    public override void HandleEvent(GameObject in_gameObject)
	{
		GameObject gameObj = (useOtherObject && in_gameObject != null) ? in_gameObject : gameObject;

		soundEmitterObject = gameObj;

        if (enableActionOnEvent)
        {
            AkSoundEngine.ExecuteActionOnEvent((uint)eventID, actionOnEventType, gameObj, (int)transitionDuration * 1000, curveInterpolation);
			return;
        }
        else if (m_callbackData != null)
            playingId = AkSoundEngine.PostEvent((uint)eventID, gameObj, (uint)m_callbackData.uFlags, Callback, null, 0, null, AkSoundEngine.AK_INVALID_PLAYING_ID);
        else
            playingId = AkSoundEngine.PostEvent((uint)eventID, gameObj);

        if (playingId == AkSoundEngine.AK_INVALID_PLAYING_ID)
        {
            Debug.LogError("Could not post event ID \"" + eventID + "\". Did you make sure to load the appropriate SoundBank?");
        }
    }

    public void Stop(int _transitionDuration, AkCurveInterpolation _curveInterpolation = AkCurveInterpolation.AkCurveInterpolation_Linear)
    {
		AkSoundEngine.ExecuteActionOnEvent((uint)eventID, AkActionOnEventType.AkActionOnEventType_Stop, soundEmitterObject, _transitionDuration, _curveInterpolation);
    }
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.