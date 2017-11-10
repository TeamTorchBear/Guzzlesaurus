#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using AK.Wwise.TreeView;


public class Postprocessor : AssetPostprocessor
{
    //This function will be called before script compilation and will save the picker's expantion 
	static void OnPostprocessAllAssets(	string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)		
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
        {
            return;
        }
 
		AkWwisePicker.treeView.SaveExpansionStatus ();	
	}	
}


class AkWwisePicker : EditorWindow
{
	public static bool WwiseProjectFound = true;
	
    [UnityEditor.MenuItem("Window/Wwise Picker", false, (int)AkWwiseWindowOrder.WwisePicker)] 
    public static void init()
    {
		EditorWindow.GetWindow<AkWwisePicker>("Wwise Picker", true, typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow"));
        PopulateTreeview();   
 	} 

    public static AkWwiseTreeView treeView = new AkWwiseTreeView();

    void OnEnable()
    {
        if (string.IsNullOrEmpty(WwiseSettings.LoadSettings().WwiseProjectPath))
        {
            return;
        }
        
        treeView.SaveExpansionStatus();
        if (AkWwiseWWUBuilder.Populate())
            PopulateTreeview();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal("Box");
        
        AkWwiseProjectInfo.GetData().autoPopulateEnabled = GUILayout.Toggle(AkWwiseProjectInfo.GetData().autoPopulateEnabled, "Auto populate");

        if (AkWwiseProjectInfo.GetData().autoPopulateEnabled && WwiseProjectFound)
        {
            AkWwiseWWUBuilder.StartWWUWatcher();
        }
        else
        {
            AkWwiseWWUBuilder.StopWWUWatcher();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh Project", GUILayout.Width(200)))
        {
			treeView.SaveExpansionStatus();
            if (AkWwiseProjectInfo.Populate())
                PopulateTreeview(); 
        }
        
        if (GUILayout.Button("Generate SoundBanks", GUILayout.Width(200)))
        {
            if (AkUtilities.IsSoundbankGenerationAvailable())
            {
                AkUtilities.GenerateSoundbanks();
            }
            else
            {
                string errorMessage;

#if UNITY_EDITOR_WIN
                errorMessage = "Access to Wwise is required to generate the SoundBanks. Please select the Wwise Windows Installation Path from the Edit > Wwise Settings... menu.";
#elif UNITY_EDITOR_OSX
				errorMessage = "Access to Wwise is required to generate the SoundBanks. Please select the Wwise Application from the Edit > Wwise Settings... menu.";
#endif

                Debug.LogError(errorMessage);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        treeView.DisplayTreeView(TreeViewControl.DisplayTypes.USE_SCROLL_VIEW);

        if (GUI.changed && WwiseProjectFound)
        {
        	EditorUtility.SetDirty(AkWwiseProjectInfo.GetData());             
        }
        // TODO: RTP Parameters List
    }

    //////////////////////////////////////////////////////////

    public static void PopulateTreeview()
    {	
        treeView.AssignDefaults(); 
        treeView.SetRootItem(System.IO.Path.GetFileNameWithoutExtension(WwiseSetupWizard.Settings.WwiseProjectPath), AkWwiseProjectData.WwiseObjectType.PROJECT);
		treeView.PopulateItem(treeView.RootItem, "Events", AkWwiseProjectInfo.GetData().EventWwu); 
        treeView.PopulateItem(treeView.RootItem, "Switches", AkWwiseProjectInfo.GetData().SwitchWwu);
        treeView.PopulateItem(treeView.RootItem, "States", AkWwiseProjectInfo.GetData().StateWwu);
		treeView.PopulateItem(treeView.RootItem, "SoundBanks", AkWwiseProjectInfo.GetData().BankWwu);
        treeView.PopulateItem(treeView.RootItem, "Auxiliary Busses", AkWwiseProjectInfo.GetData().AuxBusWwu);
		//treeView.PopulateItem(treeView.RootItem, "Game Parameters", AkWwiseProjectInfo.GetData().RtpcWwu);
		//treeView.PopulateItem(treeView.RootItem, "Triggers", AkWwiseProjectInfo.GetData().TriggerWwu);
	}

}
#endif