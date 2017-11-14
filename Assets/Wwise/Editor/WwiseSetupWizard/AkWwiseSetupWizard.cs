#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public class WwiseSetupWizard
{
    public static WwiseSettings Settings;

    static WwiseSetupWizard()
    {
        try
        {
            Settings = WwiseSettings.LoadSettings();
        }
        catch (Exception e)
        {
            Debug.LogError("WwiseUnity: Failed to load the settings, exception caught: " + e.ToString());
        }
    }

    public static void RunModify()
    {
        try
        {
            Debug.Log("WwiseUnity: Running modify setup...");

            AkSceneUtils.CreateNewScene();

            WwiseSetupWizard.ModifySetup();

            Debug.Log("WwiseUnity: Refreshing asset database.");
            AssetDatabase.Refresh();

            Debug.Log("WwiseUnity: End of modify setup, exiting Unity.");
            EditorApplication.Exit(0);
        }
        catch (Exception e)
        {
            Debug.LogError("WwiseUnity: Exception caught: " + e.ToString());
            EditorApplication.Exit(1);
        }
    }

    public static void RunSetup()
    {
        try
        {
            Debug.Log("WwiseUnity: Running install setup...");

            AkSceneUtils.CreateNewScene();

            WwiseSetupWizard.Setup();
		
		    Debug.Log("WwiseUnity: Refreshing asset database.");
		    AssetDatabase.Refresh();

            Debug.Log("WwiseUnity: End of setup, exiting Unity.");
		    EditorApplication.Exit(0);
        }
        catch (Exception e)
        {
            Debug.LogError("WwiseUnity: Exception caught: " + e.ToString());
            EditorApplication.Exit(1);
        }
    }

    public static void RunDemoSceneSetup()
    {
        try
        {
            Debug.Log("WwiseUnity: Running demo scene setup...");

            AkSceneUtils.CreateNewScene();

            WwiseSetupWizard.Setup();

            AkSceneUtils.OpenExistingScene("Assets/WwiseDemoScene/WwiseDemoScene.unity");

            Debug.Log("WwiseUnity: Refreshing asset database.");
            AssetDatabase.Refresh();

            Debug.Log("WwiseUnity: End of demo scene setup, exiting Unity.");
            EditorApplication.Exit(0);
        }
        catch (Exception e)
        {
            Debug.LogError("WwiseUnity: Exception caught: " + e.ToString());
            EditorApplication.Exit(1);
        }
    }
    
    private static void UpdateProgressBar(float progress)
    {
        EditorUtility.DisplayProgressBar("Wwise Integration", "Migration in progress - Please wait...", progress);
    }

    public static void RunMigrate()
    {
        try
        {
            Debug.Log("WwiseUnity: Running migration setup...");
			
            Debug.Log("WwiseUnity: Reading parameters...");

            string[] arguments = Environment.GetCommandLineArgs();
            string migrateStartString = null;
            int indexMigrateStart = Array.IndexOf(arguments, "-wwiseInstallMigrateStart");
        
            if (indexMigrateStart != -1)
            {
                migrateStartString = arguments[indexMigrateStart + 1];
            }
            else
            {
                Debug.LogError("WwiseUnity: ERROR: Missing parameter wwiseInstallMigrateStart.");
                EditorApplication.Exit(1);
            }

            string migrateStopString = null;
            int indexMigrateStop = Array.IndexOf(arguments, "-wwiseInstallMigrateStop");
        
            if (indexMigrateStop != -1)
            {
                migrateStopString = arguments[indexMigrateStop + 1];
            }
            else
            {
                Debug.LogError("WwiseUnity: ERROR: Missing parameter wwiseInstallMigrateStart.");
                EditorApplication.Exit(1);
            }

            int migrateStart;
            int migrateStop;

            if (!Int32.TryParse(migrateStartString, out migrateStart))
            {
                Debug.LogError("WwiseUnity: ERROR: wwiseInstallMigrateStart is not a number.");
                return;
            }

            if (!Int32.TryParse(migrateStopString, out migrateStop))
            {
                Debug.LogError("WwiseUnity: ERROR: wwiseInstallMigrateStop is not a number.");
                return;
            }

            PerformMigration(migrateStart, migrateStop);
		
		    Debug.Log("WwiseUnity: Refreshing asset database.");
		    AssetDatabase.Refresh();

            Debug.Log("WwiseUnity: End of setup, exiting Unity.");
		    EditorApplication.Exit(0);
        }
        catch (Exception e)
        {
            Debug.LogError("WwiseUnity: Exception caught: " + e.ToString());
            EditorApplication.Exit(1);
        }
    }

    private static void MigrateCurrentScene(FileInfo[] files, int migrateStart, int migrateStop)
    {
		var objectTypeMap = new Dictionary<Type, UnityEngine.Object[]>();

		foreach (var file in files)
		{
			string className = Path.GetFileNameWithoutExtension(file.Name);

			// Since monobehaviour scripts need to have the same name as the class it contains, we can use it to get the type of the object.
			Type objectType = Type.GetType(className + ", Assembly-CSharp");

			if (objectType.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				// Get all objects in the scene with the specified type.
				UnityEngine.Object[] objects = UnityEngine.Object.FindObjectsOfType(objectType);

				if (objects != null && objects.Length > 0)
					objectTypeMap[objectType] = objects;
			}
		}

		for (int ii = migrateStart; ii <= migrateStop; ++ii)
		{
			string migrationMethodName = "Migrate" + ii;
			string preMigrationMethodName = "PreMigration" + ii;
			string postMigrationMethodName = "PostMigration" + ii;

			foreach (var objectTypePair in objectTypeMap)
			{
				Type objectType = objectTypePair.Key;
				UnityEngine.Object[] objects = objectTypePair.Value;
				string className = objectType.Name;

				MethodInfo preMigrationMethodInfo = objectType.GetMethod(preMigrationMethodName, BindingFlags.Public | BindingFlags.Static);
				if (preMigrationMethodInfo != null)
				{
					Debug.Log("WwiseUnity: PreMigration step <" + ii + "> for class <" + className + ">");
					preMigrationMethodInfo.Invoke(null, null);
				}

				MethodInfo migrationMethodInfo = objectType.GetMethod(migrationMethodName, BindingFlags.Public | BindingFlags.Instance);
				if (migrationMethodInfo != null)
				{
					Debug.Log("WwiseUnity: Migration step <" + ii + "> for class <" + className +">");

					// Call the migration method of each object.
					foreach (var currentObject in objects)
						migrationMethodInfo.Invoke(currentObject, null);
				}

				MethodInfo postMigrationMethodInfo = objectType.GetMethod(postMigrationMethodName, BindingFlags.Public | BindingFlags.Static);
				if (postMigrationMethodInfo != null)
				{
					Debug.Log("WwiseUnity: PostMigration step <" + ii + "> for class <" + className + ">");
					postMigrationMethodInfo.Invoke(null, null);
				}
			}
		}
    }

    public static void PerformMigration(int migrateStart, int migrateStop)
    {
        UpdateProgressBar(0f);

        Debug.Log("WwiseUnity: Migrating incrementally to versions " + migrateStart.ToString() + " up to " + migrateStop.ToString());

        AkPluginActivator.DeactivateAllPlugins();
        AkPluginActivator.Update();
        AkPluginActivator.ActivatePlugins(AkPluginActivator.CONFIG_PROFILE, true);

        // Get the name of the currently opened scene.
        string currentScene = AkSceneUtils.GetCurrentScene().Replace('/', '\\');

        FileInfo[] files = new DirectoryInfo(Application.dataPath + "/Wwise/Deployment/Components").GetFiles("*.cs", SearchOption.AllDirectories);

        FileInfo[] sceneInfo = new DirectoryInfo(Application.dataPath).GetFiles("*.unity", SearchOption.AllDirectories);
        string[] scenes = new string[sceneInfo.Length];

        AkSceneUtils.CreateNewScene();
        AkUtilities.IsMigrating = true;

        for (int i = 0; i < scenes.Length; i++)
        {
            UpdateProgressBar((float)i / scenes.Length);

            string scene = "Assets" + sceneInfo[i].FullName.Substring(Application.dataPath.Length);
            Debug.Log("WwiseUnity: Migrating scene " + scene);

            AkSceneUtils.OpenExistingScene(scene);
            MigrateCurrentScene(files, migrateStart - 1, migrateStop - 1);
            AkSceneUtils.SaveCurrentScene(null);
        }

        UpdateProgressBar(1.0f);

        AkSceneUtils.CreateNewScene();

        AkUtilities.IsMigrating = false;

        // Reopen the scene that was opened before the migration process started.
        AkSceneUtils.OpenExistingScene(currentScene);

        Debug.Log("WwiseUnity: Removing lock for launcher.");

        // TODO: Moving one folder up is not nice at all. How to find the current project path?
        try
        {
            File.Delete(Application.dataPath + "/../.WwiseLauncherLockFile");
        }
        catch (Exception)
        {
            // Ignore if not present.
        }

        EditorUtility.ClearProgressBar();
    }
    
    public static void ModifySetup()
    {
        string currentConfig = AkPluginActivator.GetCurrentConfig();

        if (String.IsNullOrEmpty(currentConfig))
        {
            currentConfig = AkPluginActivator.CONFIG_PROFILE;
        }

        AkPluginActivator.DeactivateAllPlugins();
        AkPluginActivator.Update();
        AkPluginActivator.ActivatePlugins(currentConfig, true);
    }

    // Perform all necessary steps to use the Wwise Unity integration.
    private static void Setup()
    {
        AkPluginActivator.DeactivateAllPlugins();

        // 0. Make sure the soundbank directory exists
        string sbPath = AkUtilities.GetFullPath(Application.streamingAssetsPath, Settings.SoundbankPath);
		if (!Directory.Exists(sbPath))
        {
            Directory.CreateDirectory(sbPath);
        }

		// 1. Disable built-in audio
		if (!DisableBuiltInAudio())
		{
            Debug.LogWarning("WwiseUnity: Could not disable built-in audio. Please disable built-in audio by going to Project->Project Settings->Audio, and check \"Disable Audio\".");
		}

        // 2. Create a "WwiseGlobal" game object and set the AkSoundEngineInitializer and terminator scripts
        // 3. Set the SoundBank path property on AkSoundEngineInitializer
        CreateWwiseGlobalObject();

        // 5. Add AkAudioListener component to camera
        SetListener();

        // 6. Enable "Run In Background" in PlayerSettings (PlayerSettings.runInbackground property)
        PlayerSettings.runInBackground = true;

        AkPluginActivator.Update();
        AkPluginActivator.ActivatePlugins(AkPluginActivator.CONFIG_PROFILE, true);

        // 9. Activate WwiseIDs file generation, and point Wwise to the Assets/Wwise folder
        // 10. Change the SoundBanks options so it adds Max Radius information in the Wwise project
        if (!SetSoundbankSettings())
        {
            Debug.LogWarning("WwiseUnity: Could not modify Wwise Project to generate the header file!");
        }

        // 11. Activate XboxOne network sockets.
        AkXboxOneUtils.EnableXboxOneNetworkSockets();
    }

    // Create a Wwise Global object containing the initializer and terminator scripts. Set the soundbank path of the initializer script.
    // This game object will live for the whole project; there is no need to instanciate one per scene.
    private static void CreateWwiseGlobalObject()
    {
        // Look for a game object which has the initializer component
        AkInitializer[] AkInitializers = UnityEngine.Object.FindObjectsOfType(typeof(AkInitializer)) as AkInitializer[];
        GameObject WwiseGlobalGameObject = null;
        if (AkInitializers.Length > 0)
        {
            GameObject.DestroyImmediate(AkInitializers[0].gameObject);
        }

        WwiseGlobalGameObject = new GameObject("WwiseGlobal");

        // attach initializer component
        AkInitializer AkInit = WwiseGlobalGameObject.AddComponent<AkInitializer>();

        // Set the soundbank path property on the initializer
        AkInit.basePath = Settings.SoundbankPath;

        // Set focus on WwiseGlobal
        Selection.activeGameObject = WwiseGlobalGameObject;
    }

    private static bool DisableBuiltInAudio()
    {
        SerializedObject audioSettingsAsset = null;
        SerializedProperty disableAudioProperty = null;

        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/AudioManager.asset");
        if (assets.Length > 0)
        {
            audioSettingsAsset = new SerializedObject(assets[0]);
        }

        if (audioSettingsAsset != null)
        {
            disableAudioProperty = audioSettingsAsset.FindProperty("m_DisableAudio");
        }

        if (disableAudioProperty == null)
        {
            return false;
        }

        disableAudioProperty.boolValue = true;
        audioSettingsAsset.ApplyModifiedProperties();
        return true;
    }

    // Disable the built-in audio listener, and add the AkGameObj to the camera
    private static void SetListener()
    {
        WwiseSettings settings = WwiseSettings.LoadSettings();		
		
        // Remove the audio listener script
		if( settings.CreateWwiseListener && Camera.main != null )
		{
			AudioListener listener = Camera.main.gameObject.GetComponent<AudioListener>();
			if (listener != null)
			{
				Component.DestroyImmediate(listener);
			}

            // Add the AkGameObj script
            {
                Camera.main.gameObject.AddComponent<AkAudioListener>();

                AkGameObj akGameObj = Camera.main.gameObject.GetComponent<AkGameObj>();
				akGameObj.isEnvironmentAware = false;
            }
		}
    }

    // Modify the .wproj file to set needed soundbank settings
    private static bool SetSoundbankSettings()
    {
		if (string.IsNullOrEmpty(Settings.WwiseProjectPath))
        {
            // Nothing to do here, because setup should succeed if Wwise project is not given
            return true;
        }

        Regex r = new Regex("_WwiseIntegrationTemp.*?([/\\\\])");
        string SoundbankPath = AkUtilities.GetFullPath(r.Replace(Application.streamingAssetsPath, "$1"), Settings.SoundbankPath);
        string WprojPath = AkUtilities.GetFullPath(Application.dataPath, Settings.WwiseProjectPath);
#if UNITY_EDITOR_OSX
		SoundbankPath = "Z:" + SoundbankPath;
#endif

        if (AkUtilities.EnableBoolSoundbankSettingInWproj("SoundBankGenerateHeaderFile", WprojPath))
        {
            if (AkUtilities.SetSoundbankHeaderFilePath(WprojPath, SoundbankPath))
            {
                return AkUtilities.EnableBoolSoundbankSettingInWproj("SoundBankGenerateMaxAttenuationInfo", WprojPath);
            }
        }
        return false;
    }
}

#endif // UNITY_EDITOR