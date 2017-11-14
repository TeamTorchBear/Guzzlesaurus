#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2012 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

#pragma warning disable 0219, 0414

[AddComponentMenu("Wwise/AkInitializer")]
/// This script deals with initialization, and frame updates of the Wwise audio engine.  
/// It is marked as \c DontDestroyOnLoad so it stays active for the life of the game, 
/// not only one scene. You can, and probably should, modify this script to change the 
/// initialization parameters for the sound engine. A few are already exposed in the property inspector.
/// It must be present on one Game Object at the beginning of the game to initialize the audio properly.
/// It must be executed BEFORE any other MonoBehaviors that use AkSoundEngine.
/// \sa
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=workingwithsdks__initialization.html" target="_blank">Initialize the Different Modules of the Sound Engine</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=namespace_a_k_1_1_sound_engine_a27257629833b9481dcfdf5e793d9d037.html#a27257629833b9481dcfdf5e793d9d037" target="_blank">AK::SoundEngine::Init()</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=namespace_a_k_1_1_sound_engine_a9176602bbe972da4acc1f8ebdb37f2bf.html#a9176602bbe972da4acc1f8ebdb37f2bf" target="_blank">AK::SoundEngine::Term()</a> (Note: This is described in the Wwise SDK documentation.)
/// - AkCallbackManager
[RequireComponent(typeof(AkTerminator))]
public class AkInitializer : MonoBehaviour
{
	public readonly static string c_DefaultBasePath = Path.Combine("Audio", "GeneratedSoundBanks");
	///Path for the soundbanks. This must contain one sub folder per platform, with the same as in the Wwise project.
	public string basePath = c_DefaultBasePath;

	public const string c_Language = "English(US)";
	/// Language sub-folder. 
	public string language = c_Language;

	public const int c_DefaultPoolSize = 4096;
	///Default Pool size.  This contains the meta data for your audio project.  Default size is 4 MB, but you should adjust for your needs.
	public int defaultPoolSize = c_DefaultPoolSize;

	public const int c_LowerPoolSize = 2048;
	///Lower Pool size.  This contains the audio processing buffers and DSP data.  Default size is 2 MB, but you should adjust for your needs.
	public int lowerPoolSize = c_LowerPoolSize;

	public const int c_StreamingPoolSize = 1024;
	///Streaming Pool size.  This contains the streaming buffers.  Default size is 1 MB, but you should adjust for your needs.
	public int streamingPoolSize = c_StreamingPoolSize;

	public const int c_PreparePoolSize = 0;
	///Prepare Pool size.  This contains the banks loaded using PrepareBank (Banks decoded on load use this).  Default size is 0 MB, but you should adjust for your needs.
	public int preparePoolSize = c_PreparePoolSize;

	public const float c_MemoryCutoffThreshold = 0.95f;
	///This setting will trigger the killing of sounds when the memory is reaching 95% of capacity.  Lowest priority sounds are killed.
	public float memoryCutoffThreshold = c_MemoryCutoffThreshold;

	public const int c_MonitorPoolSize = 128;
	///Monitor Pool size.  Size of the monitoring pool, in bytes. This parameter is not used in Release build.
	public int monitorPoolSize = c_MonitorPoolSize;

	public const int c_MonitorQueuePoolSize = 64;
	///Monitor Queue Pool size.  Size of the monitoring queue pool, in bytes. This parameter is not used in Release build.
	public int monitorQueuePoolSize = c_MonitorQueuePoolSize;

	public const int c_CallbackManagerBufferSize = 4;
	///CallbackManager buffer size.  The size of the buffer used per-frame to transfer callback data. Default size is 4 KB, but you should increase this, if required.
	public int callbackManagerBufferSize = c_CallbackManagerBufferSize;

	public const bool c_EngineLogging = true;
	///Enable Wwise engine logging. Option to turn on/off the logging of the Wwise engine.
	public bool engineLogging = c_EngineLogging;

	public static string GetBasePath()
	{
#if UNITY_EDITOR
		return WwiseSettings.LoadSettings().SoundbankPath;
#else
        return ms_Instance.basePath;
#endif
	}

	public static string GetDecodedBankFolder()
	{
		return "DecodedBanks";
	}

	public static string GetDecodedBankFullPath()
	{
#if (UNITY_ANDROID || UNITY_IOS || UNITY_SWITCH) && !UNITY_EDITOR
		// This is for platforms that only have a specific file location for persistent data.
		return Path.Combine(Application.persistentDataPath, GetDecodedBankFolder());
#else
		return Path.Combine(AkBasePathGetter.GetPlatformBasePath(), GetDecodedBankFolder());
#endif
	}

	public static string GetCurrentLanguage()
	{
		return ms_Instance.language;
	}

	void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (ms_Instance != null)
		{
			//Don't init twice
			//Check if there are 2 objects with this script.  If yes, remove this component.
			if (ms_Instance != this)
				UnityEngine.Object.DestroyImmediate(this.gameObject);
			return;
		}

		Debug.Log("WwiseUnity: Initialize sound engine ...");

		//Use default properties for most SoundEngine subsystem.  
		//The game programmer should modify these when needed.  See the Wwise SDK documentation for the initialization.
		//These settings may very well change for each target platform.
		AkMemSettings memSettings = new AkMemSettings();
		memSettings.uMaxNumPools = 20;

		AkDeviceSettings deviceSettings = new AkDeviceSettings();
		AkSoundEngine.GetDefaultDeviceSettings(deviceSettings);

		AkStreamMgrSettings streamingSettings = new AkStreamMgrSettings();
		streamingSettings.uMemorySize = (uint)streamingPoolSize * 1024;

		AkInitSettings initSettings = new AkInitSettings();
		AkSoundEngine.GetDefaultInitSettings(initSettings);
		initSettings.uDefaultPoolSize = (uint)defaultPoolSize * 1024;
		initSettings.uMonitorPoolSize = (uint)monitorPoolSize * 1024;
		initSettings.uMonitorQueuePoolSize = (uint)monitorQueuePoolSize * 1024;
#if (!UNITY_ANDROID && !UNITY_WSA) || UNITY_EDITOR // Exclude WSA. It only needs the name of the DLL, and no path.
		initSettings.szPluginDLLPath = Path.Combine(Application.dataPath, "Plugins" + Path.DirectorySeparatorChar);
#endif

		AkPlatformInitSettings platformSettings = new AkPlatformInitSettings();
		AkSoundEngine.GetDefaultPlatformInitSettings(platformSettings);
		platformSettings.uLEngineDefaultPoolSize = (uint)lowerPoolSize * 1024;
		platformSettings.fLEngineDefaultPoolRatioThreshold = memoryCutoffThreshold;

		AkMusicSettings musicSettings = new AkMusicSettings();
		AkSoundEngine.GetDefaultMusicSettings(musicSettings);

#if UNITY_EDITOR
		AkSoundEngine.SetGameName(Application.productName + " (Editor)");
#else
		AkSoundEngine.SetGameName(Application.productName);
#endif

		AKRESULT result = AkSoundEngine.Init(memSettings, streamingSettings, deviceSettings, initSettings, platformSettings, musicSettings, (uint)preparePoolSize * 1024);
		if (result != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed to initialize the sound engine. Abort.");
			return; //AkSoundEngine.Init should have logged more details.
		}

		ms_Instance = this;

		string basePathToSet = AkBasePathGetter.GetSoundbankBasePath();
		if (string.IsNullOrEmpty(basePathToSet))
		{
			return;
		}

		result = AkSoundEngine.SetBasePath(basePathToSet);
		if (result != AKRESULT.AK_Success)
		{
			return;
		}

#if !UNITY_SWITCH
		// Calling Application.persistentDataPath crashes Switch
		string decodedBankFullPath = GetDecodedBankFullPath();
		// AkSoundEngine.SetDecodedBankPath creates the folders for writing to (if they don't exist)
		AkSoundEngine.SetDecodedBankPath(decodedBankFullPath);
#endif

		AkSoundEngine.SetCurrentLanguage(language);

#if !UNITY_SWITCH
		// Calling Application.persistentDataPath crashes Switch
		// AkSoundEngine.AddBasePath is currently only implemented for iOS and Android; No-op for all other platforms.
		AkSoundEngine.AddBasePath(Application.persistentDataPath + Path.DirectorySeparatorChar);
		// Adding decoded bank path last to ensure that it is the first one used when writing decoded banks.
		AkSoundEngine.AddBasePath(decodedBankFullPath);
#endif

		result = AkCallbackManager.Init(callbackManagerBufferSize * 1024);
		if (result != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed to initialize Callback Manager. Terminate sound engine.");
			AkSoundEngine.Term();
			ms_Instance = null;
			return;
		}

		AkBankManager.Reset();

		Debug.Log("WwiseUnity: Sound engine initialized.");

		//The sound engine should not be destroyed once it is initialized.
		DontDestroyOnLoad(this);

#if UNITY_EDITOR
		//Redirect Wwise error messages into Unity console.
		AkCallbackManager.SetMonitoringCallback(ErrorLevel.ErrorLevel_All, CopyMonitoringInConsole);
#endif

		//Load the init bank right away.  Errors will be logged automatically.
		uint BankID;
		result = AkSoundEngine.LoadBank("Init.bnk", AkSoundEngine.AK_DEFAULT_POOL_ID, out BankID);
		if (result != AKRESULT.AK_Success)
		{
			Debug.LogError("WwiseUnity: Failed load Init.bnk with result: " + result.ToString());
		}

#if UNITY_EDITOR
#if UNITY_2017_2_OR_NEWER
		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		EditorApplication.pauseStateChanged += OnPauseStateChanged;
#else
		EditorApplication.playmodeStateChanged += OnEditorPlaymodeStateChanged;
#endif
#endif
	}

	void OnDestroy()
	{
		if (ms_Instance == this)
		{
#if UNITY_EDITOR
#if UNITY_2017_2_OR_NEWER
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.pauseStateChanged -= OnPauseStateChanged;
#else
			EditorApplication.playmodeStateChanged -= OnEditorPlaymodeStateChanged;
#endif
#endif

			AkCallbackManager.SetMonitoringCallback(0, null);
			ms_Instance = null;
		}
		// Do nothing. AkTerminator handles sound engine termination.
	}

	void OnEnable()
	{
		//The sound engine was not terminated normally.  Make this instance the one that will manage
		//the updates and termination.
		//This happen when Unity resets everything when a script changes.
		if (ms_Instance == null && AkSoundEngine.IsInitialized())
		{
			ms_Instance = this;
#if UNITY_EDITOR
			//Redirect Wwise error messages into Unity console.
			AkCallbackManager.SetMonitoringCallback(ErrorLevel.ErrorLevel_All, CopyMonitoringInConsole);
#endif
		}
	}

	//Use LateUpdate instead of Update() to ensure all gameobjects positions, listener positions, environements, RTPC, etc are set before finishing the audio frame.
	void LateUpdate()
	{
		//Execute callbacks that occurred in last frame (not the current update)     
		if (ms_Instance != null)
		{
			AkCallbackManager.PostCallbacks();
			AkBankManager.DoUnloadBanks();
			AkAudioListener.DefaultListeners.Refresh();
			AkSoundEngine.RenderAudio();
		}
	}

#if UNITY_EDITOR

	void OnDisable()
	{
		// Unregister the callback that redirects the output to the Unity console.  If not done early enough (meaning, later than Disable), AkInitializer will leak.
		if (ms_Instance != null && AkSoundEngine.IsInitialized())
			AkCallbackManager.SetMonitoringCallback(0, null);
	}

	void CopyMonitoringInConsole(ErrorCode in_errorCode, ErrorLevel in_errorLevel, uint in_playingID, ulong in_gameObjID, string in_msg)
	{
		// Only log when logging from the engine is enabled. The callback remains active when the flag is disabled to ensure
		// it can be toggled on and off in a lively manner in Unity.
		if (engineLogging)
		{
			string msg = "Wwise: " + in_msg;
			if (in_gameObjID != AkSoundEngine.AK_INVALID_GAME_OBJECT)
			{
				GameObject obj = EditorUtility.InstanceIDToObject((int)in_gameObjID) as GameObject;
				string name = obj != null ? obj.ToString() : in_gameObjID.ToString();
				msg += "(Object: " + name + ")";
			}

			if (in_errorLevel == ErrorLevel.ErrorLevel_Error)
				Debug.LogError(msg);
			else
				Debug.Log(msg);
		}
	}
#endif

	//
	// Private members
	//

	private static AkInitializer ms_Instance;

#if !UNITY_EDITOR && !UNITY_IOS
    //Keep out of UNITY_EDITOR because the sound needs to keep playing when switching windows (remote debugging in Wwise, for example).
	//On iOS, application interruptions are handled in the sound engine already.
	void OnApplicationPause(bool pauseStatus) 
	{
		ActivateAudio(!pauseStatus);
	}

	void OnApplicationFocus(bool focus)
	{
		ActivateAudio(focus);
	}
#endif

#if UNITY_EDITOR
	// Enable/Disable the audio when pressing play/pause in the editor.
#if UNITY_2017_2_OR_NEWER
	private static void OnPlayModeStateChanged(PlayModeStateChange playMode)
	{
		if (playMode == PlayModeStateChange.EnteredPlayMode)
			ActivateAudio(true);
		else if (playMode == PlayModeStateChange.ExitingPlayMode)
			ActivateAudio(false);
	}

	private static void OnPauseStateChanged(PauseState pauseState)
	{
		ActivateAudio(pauseState != PauseState.Paused);
	}
#else
	private static void OnEditorPlaymodeStateChanged()
	{
		ActivateAudio(!EditorApplication.isPaused);
	}
#endif
#endif

	private static void ActivateAudio(bool activate)
	{
		if (ms_Instance != null)
		{
			if (activate)
				AkSoundEngine.WakeupFromSuspend();
			else
				AkSoundEngine.Suspend();

			AkSoundEngine.RenderAudio();
		}
	}
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.