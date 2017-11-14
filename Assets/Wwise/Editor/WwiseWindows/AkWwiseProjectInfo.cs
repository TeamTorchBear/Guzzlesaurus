#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public static class AkWwiseProjectInfo
{
    public static AkWwiseProjectData m_Data;

	private const string WwiseEditorProjectDataDirectory = "Wwise/Editor/ProjectData";
	private const string AssetsWwiseProjectDataPath = "Assets/" + WwiseEditorProjectDataDirectory + "/AkWwiseProjectData.asset";

	public static AkWwiseProjectData GetData()
    {
        if (m_Data == null && Directory.Exists(Path.Combine(Application.dataPath, "Wwise")))
        {
			try
			{
				m_Data = (AkWwiseProjectData)AssetDatabase.LoadAssetAtPath(AssetsWwiseProjectDataPath, typeof(AkWwiseProjectData));

				if (m_Data == null)
				{
                    if (!Directory.Exists(Path.Combine(Application.dataPath, WwiseEditorProjectDataDirectory)))
                    {
                        Directory.CreateDirectory(Path.Combine(Application.dataPath, WwiseEditorProjectDataDirectory));
                    }

					m_Data = ScriptableObject.CreateInstance<AkWwiseProjectData>();
					string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(AssetsWwiseProjectDataPath);
					AssetDatabase.CreateAsset(m_Data, assetPathAndName);
				}
			}
			catch( Exception e )
			{
				Debug.Log("WwiseUnity: Unable to load Wwise Data: " + e.ToString());
			}
        }
		
        return m_Data;
    }


    public static bool Populate()
    {
		bool bDirty = false;
        if (AkWwisePicker.WwiseProjectFound)
		{
			bDirty = AkWwiseWWUBuilder.Populate();
			bDirty |= AkWwiseXMLBuilder.Populate();
			if(bDirty)
	        {            
	    		EditorUtility.SetDirty(AkWwiseProjectInfo.GetData ());
			}
		}
        return bDirty;
    }    
}
#endif