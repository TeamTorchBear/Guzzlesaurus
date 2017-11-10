#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

public class AkPluginActivator
{
	public const string ALL_PLATFORMS = "All";
	public const string CONFIG_DEBUG = "Debug";
	public const string CONFIG_PROFILE = "Profile";
	public const string CONFIG_RELEASE = "Release";

	private const string MENU_PATH = "Assets/Wwise/Activate Plugins/";
    private const BuildTarget INVALID_BUILD_TARGET = (BuildTarget)(-1);

	// Use reflection because projects that were created in Unity 4 won't have the CurrentPluginConfig field
	public static string GetCurrentConfig()
	{
		FieldInfo CurrentPluginConfigField = typeof(AkWwiseProjectData).GetField("CurrentPluginConfig");
		string CurrentConfig = string.Empty;
		AkWwiseProjectData data = AkWwiseProjectInfo.GetData();

		if (CurrentPluginConfigField != null && data != null)
		{
			CurrentConfig = (string)CurrentPluginConfigField.GetValue(data);
		}

		if (string.IsNullOrEmpty(CurrentConfig))
		{
			CurrentConfig = CONFIG_PROFILE;
		}

		return CurrentConfig;
	}

	static void SetCurrentConfig(string config)
	{
		FieldInfo CurrentPluginConfigField = typeof(AkWwiseProjectData).GetField("CurrentPluginConfig");
		AkWwiseProjectData data = AkWwiseProjectInfo.GetData();
		if (CurrentPluginConfigField != null)
		{
			CurrentPluginConfigField.SetValue(data, config);
		}
		EditorUtility.SetDirty(AkWwiseProjectInfo.GetData());
	}

	private static void ActivateConfig(string config)
	{
		string currentConfig = GetCurrentConfig();
		if (currentConfig != config)
		{
			ActivatePlugins(currentConfig, false);
			SetCurrentConfig(config);
			ActivatePlugins(config, true);
		}
		else
		{
			Debug.Log("WwiseUnity: AkSoundEngine Plugins already activated for " + config + ".");
		}

		CheckMenuItems(config);
	}

	[UnityEditor.MenuItem(MENU_PATH + CONFIG_DEBUG)]
	public static void ActivateDebug()
	{
		ActivateConfig(CONFIG_DEBUG);
	}

	[UnityEditor.MenuItem(MENU_PATH + CONFIG_PROFILE)]
	public static void ActivateProfile()
	{
		ActivateConfig(CONFIG_PROFILE);
	}

	[UnityEditor.MenuItem(MENU_PATH + CONFIG_RELEASE)]
	public static void ActivateRelease()
	{
		ActivateConfig(CONFIG_RELEASE);
	}

	public static void RefreshPlugins()
	{
		ActivatePlugins(GetCurrentConfig(), true);
	}

	private static void SetStandaloneTarget(PluginImporter pluginImporter, BuildTarget target)
	{
		switch (target)
		{
			case BuildTarget.StandaloneLinux:
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux, "CPU", "x86");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", "x86");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "None");
				return;
			case BuildTarget.StandaloneLinux64:
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux64, "CPU", "x86_64");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", "x86_64");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "None");
				return;
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel, "CPU", "AnyCPU");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel64, "CPU", "AnyCPU");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", "AnyCPU");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "None");
				return;
			case BuildTarget.StandaloneWindows:
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows, "CPU", "AnyCPU");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "None");
				return;
			case BuildTarget.StandaloneWindows64:
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinux64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXIntel64, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows, "CPU", "None");
				pluginImporter.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "AnyCPU");
				return;
			default:
				return;
		}
	}

	class StaticPluginRegistration
	{
		private bool Active = false;

		private string CppText;

		private HashSet<string> FactoriesHeaderFilenames = new HashSet<string>();

		public StaticPluginRegistration(BuildTarget platform)
		{
			if (platform == BuildTarget.iOS)
			{
				CppText = @"#define AK_IOS
namespace AK { class PluginRegistration; };
#define AK_STATIC_LINK_PLUGIN(_pluginName_) \
extern AK::PluginRegistration _pluginName_##Registration; \
void *_pluginName_##_fp = (void*)&_pluginName_##Registration;" + "\n";
			}
			else if (platform == SwitchBuildTarget)
			{
				CppText = @"#define AK_NX
namespace AK { class PluginRegistration; };
#define AK_STATIC_LINK_PLUGIN(_pluginName_) \
extern AK::PluginRegistration _pluginName_##Registration; \
void *_pluginName_##_fp = (void*)&_pluginName_##Registration;" + "\n";
			}
		}

		public void TryAddLibrary(string AssetPath)
		{
			Active = true;

			if (AssetPath.Contains(".a"))
			{
				//Extract the lib name, generate the registration code.
				int begin = AssetPath.LastIndexOf('/') + 4;
				int end = AssetPath.LastIndexOf('.') - begin;
				string LibName = AssetPath.Substring(begin, end);    //Remove the lib prefix and the .a extension                    

				if (!LibName.Contains("AkSoundEngine"))
					FactoriesHeaderFilenames.Add(LibName + "Factory.h");
			}
			else if (AssetPath.Contains("Factory.h"))
			{
				FactoriesHeaderFilenames.Add(Path.GetFileName(AssetPath));
			}
		}

		public void TryWriteToFile(string RelativePath)
		{
			if (!Active)
				return;

			try
			{
				string FullPath = Path.Combine(Application.dataPath, RelativePath);
				foreach (var filename in FactoriesHeaderFilenames)
					CppText += "#include \"" + filename + "\"\n";

				File.WriteAllText(FullPath, CppText);
				FactoriesHeaderFilenames.Clear();
			}
			catch (Exception e)
			{
				Debug.LogError("Wwise: Could not write <" + RelativePath + ">. Exception: " + e.Message);
			}
		}
	}

	private const string WwisePluginFolder = "Wwise/Deployment/Plugins";

	private static BuildTarget GetPlatformBuildTarget(string platform)
	{
		var targets = Enum.GetNames(typeof(BuildTarget));
		var values = Enum.GetValues(typeof(BuildTarget));

		for (int ii = 0; ii < targets.Length; ++ii)
			if (platform.Equals(targets[ii]))
				return (BuildTarget)values.GetValue(ii);

		return INVALID_BUILD_TARGET;
	}

	// The following check is required until "BuildTarget.Switch" is available on all versions of Unity that we support.
	private static BuildTarget SwitchBuildTarget = GetPlatformBuildTarget("Switch");

	// Properly set the platforms for Ak plugins. If platformToActivate is set to all platforms
	// will be activated.
	public static void ActivatePlugins(string configToActivate, bool Activate)
	{
		PluginImporter[] importers = PluginImporter.GetAllImporters();
		bool ChangedSomeAssets = false;

		StaticPluginRegistration iOSStaticPluginRegistration = new StaticPluginRegistration(BuildTarget.iOS);
		StaticPluginRegistration SwitchStaticPluginRegistration = new StaticPluginRegistration(SwitchBuildTarget);

		foreach (PluginImporter pluginImporter in importers)
		{
			if (pluginImporter.assetPath.StartsWith("Assets/Plugins", StringComparison.CurrentCultureIgnoreCase) && pluginImporter.assetPath.Contains("AkSoundEngine"))
			{
				AssetDatabase.DeleteAsset(pluginImporter.assetPath);
				continue;
			}

			if (!pluginImporter.assetPath.Contains(WwisePluginFolder))
				continue;

			string[] splitPath = pluginImporter.assetPath.Split('/');

			// Path is Assets/Wwise/Deployment/Plugins/Platform. We need the platform string
			string pluginPlatform = splitPath[4];

			// Architecture and configuration (Debug, Profile, Release) are platform-dependent
			string pluginArch = string.Empty;
			string pluginConfig = string.Empty;
			string editorCPU = string.Empty;
			string editorOS = string.Empty;
			List<BuildTarget> targetsToSet = new List<BuildTarget>();
			bool setEditor = false;
			switch (pluginPlatform)
			{
				case "Android":
					pluginConfig = splitPath[6];
					pluginArch = splitPath[5];
					targetsToSet.Add(BuildTarget.Android);
					if (pluginArch == "armeabi-v7a")
						pluginImporter.SetPlatformData(BuildTarget.Android, "CPU", "ARMv7");
					else if (pluginArch == "x86")
						pluginImporter.SetPlatformData(BuildTarget.Android, "CPU", "x86");
					else
					{
						Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
					}
					break;

				case "iOS":
					pluginConfig = splitPath[5];
					targetsToSet.Add(BuildTarget.iOS);
					break;

				case "tvOS":
					pluginConfig = splitPath[5];
					targetsToSet.Add(BuildTarget.tvOS);
					break;

				case "Linux":
					pluginArch = splitPath[5];
					pluginConfig = splitPath[6];
					if (pluginArch == "x86")
					{
						targetsToSet.Add(BuildTarget.StandaloneLinux);
						SetStandaloneTarget(pluginImporter, BuildTarget.StandaloneLinux);
					}
					else if (pluginArch == "x86_64")
					{
						targetsToSet.Add(BuildTarget.StandaloneLinux64);
						SetStandaloneTarget(pluginImporter, BuildTarget.StandaloneLinux64);
					}
					else
					{
						Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
					}
					targetsToSet.Add(BuildTarget.StandaloneLinuxUniversal);
					break;

				case "Mac":
					pluginConfig = splitPath[5];
					SetStandaloneTarget(pluginImporter, BuildTarget.StandaloneOSXIntel);
					SetStandaloneTarget(pluginImporter, BuildTarget.StandaloneOSXIntel64);
					targetsToSet.Add(BuildTarget.StandaloneOSXIntel);
					targetsToSet.Add(BuildTarget.StandaloneOSXIntel64);
					targetsToSet.Add(BuildTarget.StandaloneOSXUniversal);
					editorCPU = "AnyCPU";
					editorOS = "OSX";
					setEditor = true;
					break;

				case "WSA":
					pluginArch = splitPath[5];
					pluginConfig = splitPath[6];
					targetsToSet.Add(BuildTarget.WSAPlayer);

					if (pluginArch == "WSA_UWP_Win32")
					{
						pluginImporter.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
						pluginImporter.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "X86");
					}
					else if (pluginArch == "WSA_UWP_x64")
					{
						pluginImporter.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
						pluginImporter.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "X64");
					}
					else if (pluginArch == "WSA_UWP_ARM")
					{
						pluginImporter.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
						pluginImporter.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "ARM");
					}
					break;

				case "PS4":
					pluginConfig = splitPath[5];
					targetsToSet.Add(BuildTarget.PS4);
					break;

				case "Vita":
					pluginArch = splitPath[5];
					pluginConfig = splitPath[6];
					targetsToSet.Add(BuildTarget.PSP2);
					break;

				case "Windows":
					pluginArch = splitPath[5];
					pluginConfig = splitPath[6];
					if (pluginArch == "x86")
					{
						targetsToSet.Add(BuildTarget.StandaloneWindows);
						SetStandaloneTarget(pluginImporter, BuildTarget.StandaloneWindows);
						editorCPU = "X86";
					}
					else if (pluginArch == "x86_64")
					{
						targetsToSet.Add(BuildTarget.StandaloneWindows64);
						SetStandaloneTarget(pluginImporter, BuildTarget.StandaloneWindows64);
						editorCPU = "X86_64";
					}
					else
					{
						Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
					}
					setEditor = true;
					editorOS = "Windows";
					break;

				case "XboxOne":
					pluginConfig = splitPath[5];
					targetsToSet.Add(BuildTarget.XboxOne);
					break;

				case "Switch":
					pluginArch = splitPath[5];
					pluginConfig = splitPath[6];

					if (SwitchBuildTarget == INVALID_BUILD_TARGET)
						continue;

					if (pluginArch == "NX32" || pluginArch == "NX64")
						targetsToSet.Add(SwitchBuildTarget);
					else
						Debug.Log("WwiseUnity: Architecture not found: " + pluginArch);
					break;

				default:
					if (!pluginPlatform.Contains("_new"))
					{
						Debug.Log("WwiseUnity: Unknown platform: " + pluginPlatform);
					}
					continue;
			}

			bool AssetChanged = false;

			if (pluginImporter.GetCompatibleWithAnyPlatform())
			{
				pluginImporter.SetCompatibleWithAnyPlatform(false);
				AssetChanged = true;
			}

			bool bActivate = true;
			if (pluginConfig == "DSP")
			{
				// For WSA, we use the plugin info for Windows, since they share banks.
				string pluginDSPPlatform = pluginPlatform == "WSA" ? "Windows" : pluginPlatform;

				if (!s_PerPlatformPlugins.ContainsKey(pluginDSPPlatform))
					continue;   //XML not parsed, don't touch anything.

				bActivate = IsPluginUsed(pluginDSPPlatform, Path.GetFileNameWithoutExtension(pluginImporter.assetPath));
			}
			else
			{
				bActivate = pluginConfig == configToActivate;
			}

			if (pluginPlatform == "Vita")
			{
#if AK_ARCH_VITA_HW
				bActivate = (pluginArch == "HW") ? bActivate : false;
#else
				bActivate = (pluginArch == "SW") ? bActivate : false;
#endif
			}

			if (!bActivate)
			{
				// This is not the configuration we want to activate, make sure it is deactivated
				foreach (BuildTarget target in targetsToSet)
				{
					// Hack for TLS ALLOCATOR ALLOC_TEMP_THREAD ERROR: We need to explicitely deactivate the plugins for WSA
					// even if they are already reported as deactivated. If we don't, we get the ALLOC_TEMP_THREAD errors for
					// some reason...
					if (target == BuildTarget.WSAPlayer)
						AssetChanged = true;
					else
						AssetChanged |= pluginImporter.GetCompatibleWithPlatform(target);

					pluginImporter.SetCompatibleWithPlatform(target, false);
				}
				if (setEditor)
				{
					AssetChanged |= pluginImporter.GetCompatibleWithEditor();
					pluginImporter.SetCompatibleWithEditor(false);
				}
			}
			else
			{
				// Set this plugin
				foreach (BuildTarget target in targetsToSet)
				{
					AssetChanged |= (pluginImporter.GetCompatibleWithPlatform(target) != Activate);
					pluginImporter.SetCompatibleWithPlatform(target, Activate);

					StaticPluginRegistration PluginRegistration = null;

					if (target == BuildTarget.iOS)
						PluginRegistration = iOSStaticPluginRegistration;
					else if (target == SwitchBuildTarget)
						PluginRegistration = SwitchStaticPluginRegistration;

					if (PluginRegistration != null)
						PluginRegistration.TryAddLibrary(pluginImporter.assetPath);
				}

				if (setEditor)
				{
					AssetChanged |= (pluginImporter.GetCompatibleWithEditor() != Activate);
					pluginImporter.SetCompatibleWithEditor(Activate);
					pluginImporter.SetEditorData("CPU", editorCPU);
					pluginImporter.SetEditorData("OS", editorOS);
				}
			}

			if (AssetChanged)
			{
				ChangedSomeAssets = true;
				AssetDatabase.ImportAsset(pluginImporter.assetPath);
			}
		}

		if (ChangedSomeAssets)
		{
			iOSStaticPluginRegistration.TryWriteToFile("Wwise/Deployment/Plugins/iOS/DSP/AkiOSPlugins.cpp");
			SwitchStaticPluginRegistration.TryWriteToFile("Wwise/Deployment/Plugins/Switch/" + "NX64" + "/DSP/AkSwitchPlugins.cpp");

			if (Activate)
			{
				Debug.Log("WwiseUnity: Plugins successfully activated for " + configToActivate + ".");
			}
		}
	}

	static void CheckMenuItems(string config)
	{
		/// Set checkmark on menu item
		UnityEditor.Menu.SetChecked(MENU_PATH + CONFIG_DEBUG, config == CONFIG_DEBUG);
		UnityEditor.Menu.SetChecked(MENU_PATH + CONFIG_PROFILE, config == CONFIG_PROFILE);
		UnityEditor.Menu.SetChecked(MENU_PATH + CONFIG_RELEASE, config == CONFIG_RELEASE);
	}
    
	public static void DeactivateAllPlugins()
	{
		PluginImporter[] importers = PluginImporter.GetAllImporters();

		foreach (PluginImporter pluginImporter in importers)
		{
			if (!pluginImporter.assetPath.Contains(WwisePluginFolder))
				continue;
            
			pluginImporter.SetCompatibleWithAnyPlatform(false);
			AssetDatabase.ImportAsset(pluginImporter.assetPath);
		}
	}

	static Dictionary<string, DateTime> s_LastParsed = new Dictionary<string, DateTime>();
	static Dictionary<string, HashSet<string>> s_PerPlatformPlugins = new Dictionary<string, HashSet<string>>();

	static public bool IsPluginUsed(string in_UnityPlatform, string in_PluginName)
	{
		if (in_PluginName.Contains("AkSoundEngine"))
			return true;

		string pluginName = in_PluginName;
		if (in_PluginName.StartsWith("lib"))
		{
			//That's a unix-y type of plugin, just remove the prefix to find our name.
			pluginName = in_PluginName.Substring(3);
		}

		HashSet<string> plugins;

		if (s_PerPlatformPlugins.TryGetValue(in_UnityPlatform, out plugins))
		{
			if (in_UnityPlatform != "iOS" && in_UnityPlatform != "Switch")
				return plugins.Contains(pluginName);

			//iOS and Switch deal with the static libs directly, unlike all other platforms.
			//Luckily the DLL name is always a subset of the lib name.
			foreach (string pl in plugins)
			{
				if (!string.IsNullOrEmpty(pl) && pluginName.Contains(pl))
					return true;

				if (String.Compare(pl, "iZotope", false) == 0 && pluginName.StartsWith("iZ"))
				{
					return true;
				}
			}

			//Exceptions

			if (in_UnityPlatform == "iOS" && pluginName.Contains("AkiOSPlugins"))
				return true;

			if (in_UnityPlatform == "Switch" && pluginName.Contains("AkSwitchPlugins"))
				return true;

			if (plugins.Contains("AkSoundSeedAir") && (pluginName.Contains("SoundSeedWind") || pluginName.Contains("SoundSeedWoosh")))
				return true;
		}

		return false;
	}


	public static void Update()
	{
		//Gather all GeneratedSoundBanks folder from the project
		IDictionary<string, string> allPaths = AkUtilities.GetAllBankPaths();
		bool bNeedRefresh = false;
		string projectPath = Path.GetDirectoryName(AkUtilities.GetFullPath(Application.dataPath, WwiseSettings.LoadSettings().WwiseProjectPath));

		IDictionary<string, HashSet<string>> pfMap = AkUtilities.GetPlatformMapping();
		//Go through all BasePlatforms 
		foreach (KeyValuePair<string, HashSet<string>> pairPF in pfMap)
		{
			//Go through all custom platforms related to that base platform and check if any of the bank files were updated.
			bool bParse = false;
			List<string> fullPaths = new List<string>();
			foreach (string customPF in pairPF.Value)
			{
				string bankPath;
				if (!allPaths.TryGetValue(customPF, out bankPath))
					continue;

				string pluginFile = "";
				try
				{
					pluginFile = Path.Combine(projectPath, Path.Combine(bankPath, "PluginInfo.xml"));
					pluginFile = pluginFile.Replace('/', Path.DirectorySeparatorChar);
					if (!File.Exists(pluginFile))
					{
						//Try in StreamingAssets too.
						pluginFile = Path.Combine(Path.Combine(AkBasePathGetter.GetFullSoundBankPath(), customPF), "PluginInfo.xml");
						if (!File.Exists(pluginFile))
							continue;
					}
					fullPaths.Add(pluginFile);

					DateTime t = File.GetLastWriteTime(pluginFile);
					DateTime lastTime = DateTime.MinValue;
					s_LastParsed.TryGetValue(customPF, out lastTime);
					if (lastTime < t)
					{
						bParse = true;
						s_LastParsed[customPF] = t;
					}
				}
				catch (System.Exception ex)
				{
					Debug.LogError("Wwise: " + pluginFile + " could not be parsed. " + ex.Message);
				}
			}

			if (bParse)
			{
				string platform = pairPF.Key;

				HashSet<string> newDlls = ParsePluginsXML(platform, fullPaths);
				HashSet<string> oldDlls = null;

				//Remap base Wwise platforms to Unity platform folders names
				if (platform.Contains("Vita"))
					platform = "Vita";
				//else other platforms already have the right name

				s_PerPlatformPlugins.TryGetValue(platform, out oldDlls);
				s_PerPlatformPlugins[platform] = newDlls;

				//Check if there was any change.
				if (!bNeedRefresh && oldDlls != null)
				{
					if (oldDlls.Count == newDlls.Count)
						oldDlls.IntersectWith((IEnumerable<string>)newDlls);

					bNeedRefresh |= oldDlls.Count != newDlls.Count;
				}
				else
					bNeedRefresh |= newDlls.Count > 0;
			}
		}

		if (bNeedRefresh)
			RefreshPlugins();

		string currentConfig = GetCurrentConfig();
		CheckMenuItems(currentConfig);
	}

	enum PluginID
	{
		SineGenerator = 0x00640002, //Sine
		WwiseSilence = 0x00650002, //Wwise Silence
		ToneGenerator = 0x00660002, //Tone Generator
		WwiseParametricEQ = 0x00690003, //Wwise Parametric EQ
		Delay = 0x006A0003, //Delay
		WwiseCompressor = 0x006C0003, //Wwise Compressor
		WwiseExpander = 0x006D0003, //Wwise Expander
		WwisePeakLimiter = 0x006E0003, //Wwise Peak Limiter
		MatrixReverb = 0x00730003, //Matrix Reverb
		WwiseRoomVerb = 0x00760003, //Wwise RoomVerb
		WwiseMeter = 0x00810003, //Wwise Meter
		Gain = 0x008B0003, //Gain
		VitaReverb = 0x008C0003, //Vita Reverb
		VitaCompressor = 0x008D0003, //Vita Compressor
		VitaDelay = 0x008E0003, //Vita Delay
		VitaDistortion = 0x008F0003, //Vita Distortion
		VitaEQ = 0x00900003 //Vita EQ
	}

	static HashSet<uint> builtInPluginIDs = new HashSet<uint>
	{
		(uint)PluginID.SineGenerator,
		(uint)PluginID.WwiseSilence,
		(uint)PluginID.ToneGenerator,
		(uint)PluginID.WwiseParametricEQ,
		(uint)PluginID.Delay,
		(uint)PluginID.WwiseCompressor,
		(uint)PluginID.WwiseExpander,
		(uint)PluginID.WwisePeakLimiter,
		(uint)PluginID.MatrixReverb,
		(uint)PluginID.WwiseRoomVerb,
		(uint)PluginID.WwiseMeter,
		(uint)PluginID.Gain,
		(uint)PluginID.VitaReverb,
		(uint)PluginID.VitaCompressor,
		(uint)PluginID.VitaDelay,
		(uint)PluginID.VitaDistortion,
		(uint)PluginID.VitaEQ,
	};

	private static HashSet<string> ParsePluginsXML(string platform, List<string> in_pluginFiles)
	{
		HashSet<string> newDlls = new HashSet<string>();

		foreach (string pluginFile in in_pluginFiles)
		{
			if (!File.Exists(pluginFile))
				continue;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(pluginFile);
				XPathNavigator Navigator = doc.CreateNavigator();

				XPathNavigator pluginInfoNode = Navigator.SelectSingleNode("//PluginInfo");
				string boolMotion = pluginInfoNode.GetAttribute("Motion", "");

				XPathNodeIterator it = Navigator.Select("//Plugin");

				if (boolMotion == "true")
					newDlls.Add("AkMotion");

				foreach (XPathNavigator node in it)
				{
					uint pid = UInt32.Parse(node.GetAttribute("ID", ""));
					if (pid == 0)
						continue;

					string dll = string.Empty;

					if (platform == "Switch")
					{
						if (pid == (uint)PluginID.WwiseMeter)
							dll = "AkMeter";
					}
					else if (builtInPluginIDs.Contains(pid))
						continue;

					if (string.IsNullOrEmpty(dll))
						dll = node.GetAttribute("DLL", "");

					newDlls.Add(dll);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Wwise: " + pluginFile + " could not be parsed. " + ex.Message);
			}
		}

		return newDlls;
	}
}
#endif