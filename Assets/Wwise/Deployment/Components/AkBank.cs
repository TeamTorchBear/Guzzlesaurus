#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System;

[AddComponentMenu("Wwise/AkBank")]
/// @brief Loads and unloads a SoundBank at a specified moment. Vorbis sounds can be decompressed at a specified moment using the decode compressed data option. In that case, the SoundBank will be prepared.
public class AkBank : AkUnityEventHandler 
{
#if UNITY_EDITOR
	public byte[] valueGuid = new byte[16];
#endif
	
	/// Name of the SoundBank, as specified in the Wwise project.
    public string bankName = "";
	/// Check this to load the SoundBank in the background. Be careful, if Events are triggered and the SoundBank hasn't finished loading, you'll have "Event not found" errors.
	public bool loadAsynchronous = false;
	
	/// Decode this SoundBank upon load
	public bool decodeBank = false;
	
	/// Save the decoded SoundBank to disk for faster loads in the future
	public bool saveDecodedBank = false;

	/// Reserved.
	public List<int> unloadTriggerList = new List<int>() {AkUnityEventHandler.DESTROY_TRIGGER_ID };

	protected override void Awake()
	{
		base.Awake ();

		RegisterTriggers (unloadTriggerList, UnloadBank);
		
		//Call the UnloadBank function if registered to the Awake Trigger
        if ((unloadTriggerList.Contains(AWAKE_TRIGGER_ID)))
		{
			UnloadBank(null);
		}
	}

	protected override void Start()
	{
		base.Start ();

		//Call the UnloadBank function if registered to the Start Trigger
		if ((unloadTriggerList.Contains(START_TRIGGER_ID)))
		{
			UnloadBank(null);
		}
	}

	/// Loads the SoundBank
	public override void HandleEvent(GameObject in_gameObject)
	{
		if (!loadAsynchronous)
			AkBankManager.LoadBank(bankName, decodeBank, saveDecodedBank);
		else
			AkBankManager.LoadBankAsync(bankName);
    }

	/// Unloads a SoundBank
	public void UnloadBank(GameObject in_gameObject)
	{
		AkBankManager.UnloadBank(bankName);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy ();

		UnregisterTriggers (unloadTriggerList, UnloadBank);

		if((unloadTriggerList.Contains(DESTROY_TRIGGER_ID)))
		{
			UnloadBank(null);
		}
	}
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.