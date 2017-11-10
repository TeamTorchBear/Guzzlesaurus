#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(AkAudioListener))]
public class AkAudioListenerInspector : Editor
{
	SerializedProperty m_isDefaultListener;

	void OnEnable()
	{
		m_isDefaultListener = serializedObject.FindProperty("isDefaultListener");
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical("Box");
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_isDefaultListener);
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}
		GUILayout.EndVertical();
	}
}
#endif
