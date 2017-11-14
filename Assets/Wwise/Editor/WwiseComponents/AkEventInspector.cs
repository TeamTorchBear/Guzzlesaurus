#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CanEditMultipleObjects]
[CustomEditor(typeof(AkEvent))]
public class AkEventInspector : AkBaseInspector
{
	SerializedProperty eventID;
	SerializedProperty enableActionOnEvent;
	SerializedProperty actionOnEventType;
	SerializedProperty curveInterpolation;
	SerializedProperty transitionDuration;
	SerializedProperty callbackData;

	AkUnityEventHandlerInspector m_UnityEventHandlerInspector = new AkUnityEventHandlerInspector();

	public void OnEnable()
	{
		m_UnityEventHandlerInspector.Init(serializedObject);
		
		eventID				= serializedObject.FindProperty("eventID");
		enableActionOnEvent	= serializedObject.FindProperty("enableActionOnEvent");
		actionOnEventType	= serializedObject.FindProperty("actionOnEventType");
		curveInterpolation	= serializedObject.FindProperty("curveInterpolation");
		transitionDuration	= serializedObject.FindProperty("transitionDuration");

		callbackData = serializedObject.FindProperty("m_callbackData");

		m_guidProperty = new SerializedProperty[1];
		m_guidProperty[0]	= serializedObject.FindProperty("valueGuid.Array");
		
		//Needed by the base class to know which type of component its working with
		m_typeName		= "Event";
		m_objectType	= AkWwiseProjectData.WwiseObjectType.EVENT;
	}

	public override void OnChildInspectorGUI()
	{	
		serializedObject.Update();

		m_UnityEventHandlerInspector.OnGUI();

		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		GUILayout.BeginVertical("Box");
		{
			EditorGUILayout.PropertyField(enableActionOnEvent, new GUIContent("Action On Event: "));

			if(enableActionOnEvent.boolValue)
			{
				EditorGUILayout.PropertyField(actionOnEventType, new GUIContent("Action On EventType: "));
				EditorGUILayout.PropertyField(curveInterpolation, new GUIContent("Curve Interpolation: "));
				EditorGUILayout.Slider(transitionDuration, 0.0f, 60.0f, new GUIContent("Fade Time (secs): "));
			}
		}
		GUILayout.EndVertical();

		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		GUILayout.BeginVertical("Box");
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(callbackData);
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}
		GUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}

	public override string UpdateIds(Guid[] in_guid)
	{
		for(int i = 0; i < AkWwiseProjectInfo.GetData().EventWwu.Count; i++)
		{
			AkWwiseProjectData.Event e = AkWwiseProjectInfo.GetData().EventWwu[i].List.Find(x => new Guid(x.Guid).Equals(in_guid[0]));
			
			if(e != null)
			{
				eventID.intValue = e.ID;
				serializedObject.ApplyModifiedProperties();

				return e.Name;
			}
		}

		return string.Empty;
	}
}
#endif
