#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(AkBank))]
public class AkBankInspector : AkBaseInspector
{
	SerializedProperty bankName;
	SerializedProperty loadAsync;
	SerializedProperty decode;
	SerializedProperty saveDecoded;

	AkUnityEventHandlerInspector m_LoadBankEventHandlerInspector = new AkUnityEventHandlerInspector();
	AkUnityEventHandlerInspector m_UnloadBankEventHandlerInspector = new AkUnityEventHandlerInspector();
	
	void OnEnable()
	{
		m_LoadBankEventHandlerInspector.Init (serializedObject, "triggerList", "Load On: ", false);
		m_UnloadBankEventHandlerInspector.Init (serializedObject, "unloadTriggerList", "Unload On: ", false);

		bankName	= serializedObject.FindProperty("bankName");
		loadAsync	= serializedObject.FindProperty("loadAsynchronous");
		decode	= serializedObject.FindProperty("decodeBank");
		saveDecoded	= serializedObject.FindProperty("saveDecodedBank");
		
		m_guidProperty		= new SerializedProperty[1];
		m_guidProperty[0]	= serializedObject.FindProperty("valueGuid.Array");

		//Needed by the base class to know which type of component its working with
		m_typeName		= "Bank";
		m_objectType	= AkWwiseProjectData.WwiseObjectType.SOUNDBANK;
	}
	
	public override void OnChildInspectorGUI ()
	{				
		serializedObject.Update ();

		m_LoadBankEventHandlerInspector.OnGUI();
		m_UnloadBankEventHandlerInspector.OnGUI ();
		
		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		GUILayout.BeginVertical("Box");
		{
			bool oldDecodeValue = decode.boolValue;
            bool oldSaveDecodedValue = saveDecoded.boolValue;
			EditorGUILayout.PropertyField(loadAsync, new GUIContent("Asynchronous:"));
			EditorGUILayout.PropertyField(decode, new GUIContent("Decode compressed data:"));

            if (decode.boolValue)
            {
                if (decode.boolValue != oldDecodeValue && AkWwiseProjectInfo.GetData().preparePoolSize == 0)
                {
                    EditorUtility.DisplayDialog("Warning", "You will need to define a prepare pool size in the AkInitializer component options.", "Ok");
                }
                EditorGUILayout.PropertyField(saveDecoded, new GUIContent("Save decoded bank:"));
                if (oldSaveDecodedValue == true && saveDecoded.boolValue == false)
                {
                    string decodedBankPath = System.IO.Path.Combine(AkInitializer.GetDecodedBankFullPath(), bankName.stringValue + ".bnk");
					try
					{
						System.IO.File.Delete(decodedBankPath);
					}
					catch(Exception e)
					{
						Debug.Log("WwiseUnity: Could not delete existing decoded SoundBank. Please delete it manually. " + e.ToString());
					}
                }
            }
		}
		GUILayout.EndVertical ();

		serializedObject.ApplyModifiedProperties ();
	}
	
	public override string UpdateIds (Guid[] in_guid)
	{
		for(int i = 0; i < AkWwiseProjectInfo.GetData().BankWwu.Count; i++)
		{
			AkWwiseProjectData.AkInformation bank = AkWwiseProjectInfo.GetData().BankWwu[i].List.Find(x => new Guid(x.Guid).Equals(in_guid[0]));
			
			if(bank != null)
			{
				serializedObject.Update();
				bankName.stringValue = bank.Name;
				serializedObject.ApplyModifiedProperties();

				return bank.Name;
			}
		}

		return string.Empty;
	}
}
#endif