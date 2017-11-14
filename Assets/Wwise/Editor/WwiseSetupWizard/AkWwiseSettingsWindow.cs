#if UNITY_EDITOR

using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class AkWwiseSettingsWindow : EditorWindow
{
	static bool m_oldCreateWwiseGlobal = true;
	static bool m_oldCreateWwiseListener = true;
	static bool m_oldShowMissingRigidBodyWarning = true;
	static string m_WwiseVersionString;

	protected static GUIStyle WelcomeStyle = null;
	protected static GUIStyle HelpStyle = null;
	protected static GUIStyle VersionStyle = null;

	void SetTextColor(GUIStyle style, Color color)
	{
		style.active.textColor = color;
		style.focused.textColor = color;
		style.hover.textColor = color;
		style.normal.textColor = color;
		style.onActive.textColor = color;
		style.onFocused.textColor = color;
		style.onHover.textColor = color;
		style.onNormal.textColor = color;
	}

	// Initialize our required styles
	protected void InitGuiStyles()
	{
		WelcomeStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
		WelcomeStyle.fontSize = 20;
		WelcomeStyle.alignment = TextAnchor.MiddleCenter;
		if (!Application.HasProLicense())
		{
			SetTextColor(WelcomeStyle, Color.black);
		}

		VersionStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
		if (!Application.HasProLicense())
		{
			SetTextColor(VersionStyle, Color.black);
		}

		HelpStyle = GUI.skin.GetStyle("box");
		HelpStyle.wordWrap = true;
		HelpStyle.alignment = TextAnchor.UpperLeft;
		HelpStyle.normal.textColor = EditorStyles.textField.normal.textColor;

	}

	public void DrawSettingsPart()
	{
		string description;
		string tooltip;

		GUILayout.Label("Wwise Project", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal("box");
		description = "Wwise Project Path*:";
		tooltip = "Location of the Wwise project associated with this game. It is recommended to put it in the Unity Project root folder, outside the Assets folder.";
		GUILayout.Label(new GUIContent(description, tooltip), GUILayout.Width(330));
		EditorGUILayout.SelectableLabel(WwiseSetupWizard.Settings.WwiseProjectPath, "textfield", GUILayout.Height(17));
		if (GUILayout.Button("...", GUILayout.Width(30)))
		{
			string OpenInPath = Path.GetDirectoryName(AkUtilities.GetFullPath(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath));
			string WwiseProjectPathNew = EditorUtility.OpenFilePanel("Select your Wwise Project", OpenInPath, "wproj");
			if (WwiseProjectPathNew.Length != 0)
			{
				if (WwiseProjectPathNew.EndsWith(".wproj") == false)
				{
					EditorUtility.DisplayDialog("Error", "Please select a valid .wproj file", "Ok");
				}
				else
				{
					// No need to check if the file exists (the FilePanel does it for us).

					// MONO BUG: https://github.com/mono/mono/pull/471
					// In the editor, Application.dataPath returns <Project Folder>/Assets. There is a bug in
					// mono for method Uri.GetRelativeUri where if the path ends in a folder, it will
					// ignore the last part of the path. Thus, we need to add fake depth to get the "real"
					// relative path.
					WwiseSetupWizard.Settings.WwiseProjectPath = AkUtilities.MakeRelativePath(Application.dataPath + "/fake_depth", WwiseProjectPathNew);
				}
			}
			Repaint();
		}

		GUILayout.EndHorizontal();

		string labelTitle;

#if UNITY_EDITOR_OSX
		description = "Wwise Application:";
		tooltip = "Location of the Wwise Application. This is required to generate the SoundBanks in Unity.";
		labelTitle = "Wwise Application";
#else
		description = "Wwise Windows Installation Path:";
		tooltip = "Location of the Wwise Windows Installation Path. This is required to generate the SoundBanks in Unity.";
		labelTitle = "Wwise Windows Installation Path";
#endif

		GUILayout.Label(labelTitle);

		GUILayout.BeginHorizontal("box");


		GUILayout.Label(new GUIContent(description, tooltip), GUILayout.Width(330));

		string wwiseInstallationPath;

#if UNITY_EDITOR_OSX
		wwiseInstallationPath = WwiseSetupWizard.Settings.WwiseInstallationPathMac;
#else
		wwiseInstallationPath = WwiseSetupWizard.Settings.WwiseInstallationPathWindows;
#endif

		EditorGUILayout.SelectableLabel(wwiseInstallationPath, "textfield", GUILayout.Height(17));

		if (GUILayout.Button("...", GUILayout.Width(30)))
		{
#if UNITY_EDITOR_OSX
			string installationPathNew = EditorUtility.OpenFilePanel("Select your Wwise application.", "/Applications/", "");
#else
			string installationPathNew = EditorUtility.OpenFolderPanel("Select your Wwise application.", Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "");
#endif

			if (installationPathNew.Length != 0)
			{
				wwiseInstallationPath = Path.GetFullPath(installationPathNew);

#if UNITY_EDITOR_OSX
				WwiseSetupWizard.Settings.WwiseInstallationPathMac = wwiseInstallationPath;
#else
				WwiseSetupWizard.Settings.WwiseInstallationPathWindows = wwiseInstallationPath;
#endif
			}

			Repaint();
		}

		GUILayout.EndHorizontal();

		GUILayout.Label("Asset Management", EditorStyles.boldLabel);
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		description = "SoundBanks Path* (relative to StreamingAssets folder):";
		tooltip = "Location of the SoundBanks are for the game. This has to reside within the StreamingAssets folder.";
		GUILayout.Label(new GUIContent(description, tooltip), GUILayout.Width(330));
		EditorGUILayout.SelectableLabel(WwiseSetupWizard.Settings.SoundbankPath, "textfield", GUILayout.Height(17));

		if (GUILayout.Button("...", GUILayout.Width(30)))
		{
			string OpenInPath = Path.GetDirectoryName(AkUtilities.GetFullPath(Application.streamingAssetsPath, WwiseSetupWizard.Settings.SoundbankPath));
			string SoundbankPathNew = EditorUtility.OpenFolderPanel("Select your SoundBanks destination folder", OpenInPath, "");
			if (SoundbankPathNew.Length != 0)
			{
				int stremingAssetsIndex = Application.dataPath.Split('/').Length;
				string[] folders = SoundbankPathNew.Split('/');

				if (folders.Length - 1 < stremingAssetsIndex || !String.Equals(folders[stremingAssetsIndex], "StreamingAssets", StringComparison.OrdinalIgnoreCase))
				{
					EditorUtility.DisplayDialog("Error", "The soundbank destination folder must be located within the Unity project 'StreamingAssets' folder.", "Ok");
				}
				else
				{
					// MONO BUG: https://github.com/mono/mono/pull/471
					// Need to add fake depth to the streaming assets path because of this bug. Directories should end in /.
					WwiseSetupWizard.Settings.SoundbankPath = AkUtilities.MakeRelativePath(Application.streamingAssetsPath + "/fake_depth", SoundbankPathNew) + "/";
				}
			}
			Repaint();
		}

		GUILayout.EndHorizontal();
		description = "Create WwiseGlobal GameObject";
		tooltip = "The WwiseGlobal object is a GameObject that contains the Initializing and Terminating scripts for the Wwise Sound Engine. In the Editor workflow, it is added to every scene, so that it can be properly be previewed in the Editor. In the game, only one instance is created, in the first scene, and it is persisted throughout the game. It is recommended to leave this box checked.";
		WwiseSetupWizard.Settings.CreateWwiseGlobal = GUILayout.Toggle(WwiseSetupWizard.Settings.CreateWwiseGlobal, new GUIContent(description, tooltip));

		description = "Automatically add Listener to Main Camera";
		tooltip = "In order for positioning to work, the Ak Audio Listener script needs to be attached to the main camera in every scene. If you wish for your listener to be attached to another GameObject, uncheck this box";
		WwiseSetupWizard.Settings.CreateWwiseListener = GUILayout.Toggle(WwiseSetupWizard.Settings.CreateWwiseListener, new GUIContent(description, tooltip));

		GUILayout.EndVertical();

		GUILayout.Label("In Editor Warnings", EditorStyles.boldLabel);

		GUILayout.BeginVertical("box");
		description = "Show Warning for Missing RigidBody";
		tooltip = "AkGameObj-AkEnvironment interactions require a Rigidbody component on the object or the environment. It is recommended to leave this box checked.";
		WwiseSetupWizard.Settings.ShowMissingRigidBodyWarning = GUILayout.Toggle(WwiseSetupWizard.Settings.ShowMissingRigidBodyWarning, new GUIContent(description, tooltip));
		GUILayout.EndVertical();


		GUILayout.BeginHorizontal();
		GUILayout.Label("* Mandatory settings");
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.EndHorizontal();
	}

	[UnityEditor.MenuItem("Edit/Wwise Settings...", false, (int)AkWwiseWindowOrder.WwiseSettings)]
	public static void Init()
	{
		// Get existing open window or if none, make a new one:
		EditorWindow window = EditorWindow.GetWindow(typeof(AkWwiseSettingsWindow));

		window.position = new Rect(100, 100, 850, 330);

		window.titleContent = new GUIContent("Wwise Settings");

		m_oldCreateWwiseGlobal = WwiseSetupWizard.Settings.CreateWwiseGlobal;
		m_oldCreateWwiseListener = WwiseSetupWizard.Settings.CreateWwiseListener;
		m_oldShowMissingRigidBodyWarning = WwiseSetupWizard.Settings.ShowMissingRigidBodyWarning;

		uint temp = AkSoundEngine.GetMajorMinorVersion();
		uint temp2 = AkSoundEngine.GetSubminorBuildVersion();
		m_WwiseVersionString = "Wwise v" + (temp >> 16) + "." + (temp & 0xFFFF);
		if ((temp2 >> 16) != 0)
		{
			m_WwiseVersionString += "." + (temp2 >> 16);
		}

		m_WwiseVersionString += " Build " + (temp2 & 0xFFFF) + " Settings.";
	}

	bool ApplyNewProject = false;
	void OnGUI()
	{
		// Make sure everything is initialized
		// Use soundbank path, because Wwise project path can be empty.
		if (String.IsNullOrEmpty(WwiseSetupWizard.Settings.SoundbankPath) && WwiseSetupWizard.Settings.WwiseProjectPath == null)
		{
			WwiseSetupWizard.Settings = WwiseSettings.LoadSettings();
		}

		string initialProject = WwiseSetupWizard.Settings.WwiseProjectPath;

		if (VersionStyle == null)
		{
			InitGuiStyles();
		}
		GUILayout.Label(m_WwiseVersionString, VersionStyle);

		DrawSettingsPart();

		string newProject = WwiseSetupWizard.Settings.WwiseProjectPath; // DrawSettingsPart modifies WwiseSetupWizard.Settings.WwiseProjectPath directly.
		if (initialProject != newProject)
		{
			ApplyNewProject = true;
		}

		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("OK", GUILayout.Width(60)))
		{
			if (string.IsNullOrEmpty(WwiseSetupWizard.Settings.SoundbankPath))
			{
				EditorUtility.DisplayDialog("Error", "Please fill in the required settings", "Ok");
			}

			if (AkWwiseSettingsWindow.m_oldCreateWwiseGlobal != WwiseSetupWizard.Settings.CreateWwiseGlobal)
			{
				AkInitializer[] AkInitializers = UnityEngine.Object.FindObjectsOfType(typeof(AkInitializer)) as AkInitializer[];
				if (WwiseSetupWizard.Settings.CreateWwiseGlobal == true)
				{
					if (AkInitializers.Length == 0)
					{
						//No Wwise object in this scene, create one so that the sound engine is initialized and terminated properly even if the scenes are loaded
						//in the wrong order.
						GameObject objWwise = new GameObject("WwiseGlobal");

						//Attach initializer and terminator components
						AkInitializer init = objWwise.AddComponent<AkInitializer>();
						AkWwiseProjectInfo.GetData().CopyInitSettings(init);
					}
				}
				else
				{
					if (AkInitializers.Length != 0 && AkInitializers[0].gameObject.name == "WwiseGlobal")
					{
						GameObject.DestroyImmediate(AkInitializers[0].gameObject);
					}
				}
			}

			if (AkWwiseSettingsWindow.m_oldCreateWwiseListener != WwiseSetupWizard.Settings.CreateWwiseListener)
			{
				if (Camera.main != null)
				{
					AkAudioListener akListener = Camera.main.GetComponentInChildren<AkAudioListener>();

					if (WwiseSetupWizard.Settings.CreateWwiseListener)
					{
						if (akListener == null)
						{
							akListener = Undo.AddComponent<AkAudioListener>(Camera.main.gameObject);
							AkGameObj akGameObj = akListener.GetComponentInChildren<AkGameObj>();
							akGameObj.isEnvironmentAware = false;
						}

						// If Unity had already an audio listener, we want to remove it when adding our own.
						AudioListener unityListener = Camera.main.GetComponentInChildren<AudioListener>();
						if (unityListener != null)
						{
							Component.DestroyImmediate(unityListener);
						}
					}
				}
			}

			if (m_oldShowMissingRigidBodyWarning != WwiseSetupWizard.Settings.ShowMissingRigidBodyWarning)
			{
				InternalEditorUtility.RepaintAllViews();
			}

			WwiseSettings.SaveSettings(WwiseSetupWizard.Settings);

			CloseWindow();

			// Pop the Picker window so the user can start working right away
			AkWwiseProjectInfo.GetData(); // Load data
			if (ApplyNewProject)
			{
				//Clear the data, the project path changed.
				AkWwiseProjectInfo.GetData().Reset();
				ApplyNewProject = false;
				AkWwisePicker.WwiseProjectFound = true;
			}
			AkWwiseProjectInfo.Populate();
			AkWwisePicker.PopulateTreeview();
			AkWwisePicker.init();
		}

		if (GUILayout.Button("Cancel", GUILayout.Width(60)))
		{
			WwiseSetupWizard.Settings = WwiseSettings.LoadSettings(true);
			CloseWindow();
		}
		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
		GUILayout.EndHorizontal();

		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
		GUILayout.EndVertical();
	}

	void CloseWindow()
	{
		EditorWindow SetupWindow = EditorWindow.GetWindow(typeof(AkWwiseSettingsWindow));
		SetupWindow.Close();
	}
}

#endif // UNITY_EDITOR