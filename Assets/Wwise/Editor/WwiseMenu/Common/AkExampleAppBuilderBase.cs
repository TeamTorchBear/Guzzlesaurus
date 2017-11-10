#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2013 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

/// @brief This is an example that shows the steps to create a custom build for Unity applications that use Wwise.
/// The build can be started by selecting a target platform and adding scenes to the build in the build settings (File->Build Settings) 
/// and by clicking on "File->Build Unity-Wwise project" on the menu bar.
/// The steps to build a Unity-Wwise project are as follow:
/// 1) 	Copy the soundbank of the current target platform from the Wwise project to the specified folder in the unity project. 
/// 2) 	Build all the scenes defined in unity's build settings.
/// 3) 	Delete the soundbank folder from the Unity project. This step is needed to prevent future builds for other platforms from using that bank.

public class AkExampleAppBuilderBase : MonoBehaviour
{
	//[UnityEditor.MenuItem("File/Build Unity-Wwise Project")] 
	public static bool Build()
    {
        //Choose app name and location
        string appPath = EditorUtility.SaveFilePanel("Build Unity-Wwise project", 										//window title
                                                        Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), //Default app location (unity project root directory)
                                                        "Unity_Wwise_app", 													//Default app name
                                                        getPlatFormExtension()												//app extension (depends on target platform)
                                                     );
        //check if the build was canceled
        bool isUserCancelledBuild = appPath == "";
        if (isUserCancelledBuild)
        {
            UnityEngine.Debug.Log("WwiseUnity: User canceled the build.");
            return false;
        }

        //get Wwise project file (.wproj) path
        string wwiseProjFile = Path.Combine(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath).Replace('/', '\\');

        //get Wwise project root folder path
        string wwiseProjectFolder = wwiseProjFile.Remove(wwiseProjFile.LastIndexOf(Path.DirectorySeparatorChar));

        //get Wwise platform string (the string isn't the same as unity for some platforms)
        string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());

        //get soundbank location in the Wwise project for the current platform target
        string sourceSoundBankFolder = Path.Combine(wwiseProjectFolder, AkBasePathGetter.GetPlatformBasePath());

        //get soundbank destination in the Unity project for the current platform target
        string destinationSoundBankFolder = Path.Combine(Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets", 								//Soundbank must be inside the StreamingAssets folder
                                                                Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString)
                                                            );

        //Copy the soundbank from the Wwise project to the unity project (Inside the StreamingAssets folder as defined in Window->Wwise Settings)
        if (!AkUtilities.DirectoryCopy(sourceSoundBankFolder, 		//source folder
                                            destinationSoundBankFolder, //destination
                                            true						//copy subfolders
                                        )
           )
        {
            UnityEngine.Debug.LogError("WwiseUnity: The soundbank folder for the " + wwisePlatformString + " platform doesn't exist. Make sure it was generated in your Wwise project");
            return false;
        }

        //Get all the scenes to build as defined in File->Build Settings
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        //Build the app
        BuildPipeline.BuildPlayer(scenes,										//scenes to build 
                                        appPath, 									//Location of the app to create
                                        EditorUserBuildSettings.activeBuildTarget,	//Platform for which to build the app 
                                        BuildOptions.None
                                    );

        //Delete the soundbank from the unity project so they don't get copied in the game folder of future builds
        Directory.Delete(destinationSoundBankFolder, true);

        return true;
    }


    private static string UnityToWwisePlatformString(string unityPlatormString)
    {
        if (unityPlatormString == BuildTarget.StandaloneWindows.ToString()
            ||
            unityPlatormString == BuildTarget.StandaloneWindows64.ToString()
           )
            return "Windows";

        else if (unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString()
                    ||
                    unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString()
                    ||
                    unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()
                )
            return "Mac";

        else if (unityPlatormString == BuildTarget.iOS.ToString())
            return "iOS";

        return unityPlatormString;
    }

    private static string getPlatFormExtension()
    {
        string unityPlatormString = EditorUserBuildSettings.activeBuildTarget.ToString();

        if (unityPlatormString == BuildTarget.StandaloneWindows.ToString()
            ||
            unityPlatormString == BuildTarget.StandaloneWindows64.ToString()
           )
            return "exe";

        else if (unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString()
                    ||
                    unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString()
                    ||
                    unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()
                )
            return "app";

        else if (unityPlatormString == BuildTarget.iOS.ToString())
            return "ipa";

        else if (unityPlatormString == BuildTarget.Android.ToString())
            return "apk";
        return "";
    }
}
#endif // #if UNITY_EDITOR