#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

public class DefaultHandles
{
	public static bool Hidden
	{
		get
		{
			Type type = typeof(Tools);
			FieldInfo field = type.GetField("s_Hidden", BindingFlags.NonPublic | BindingFlags.Static);
			return ((bool)field.GetValue(null));
		}
		set
		{
			Type type = typeof(Tools);
			FieldInfo field = type.GetField("s_Hidden", BindingFlags.NonPublic | BindingFlags.Static);
			field.SetValue(null, value);
		}
	}
}

[CanEditMultipleObjects]
[CustomEditor(typeof(AkGameObj))]
public class AkGameObjectInspector : Editor
{
	AkGameObj m_AkGameObject;
	SerializedProperty listeners;

	bool hideDefaultHandle = false;

	void OnEnable()
	{
		m_AkGameObject = target as AkGameObj;
		listeners = serializedObject.FindProperty("m_listeners");

		DefaultHandles.Hidden = hideDefaultHandle;
	}

	void OnDisable()
	{
		DefaultHandles.Hidden = false;
	}

	public override void OnInspectorGUI()
	{
		// Unity tries to construct a AkGameObjPositionOffsetData all the time. Need this ugly workaround
		// to prevent it from doing this.
		if (m_AkGameObject.m_positionOffsetData != null)
		{
			if (!m_AkGameObject.m_positionOffsetData.KeepMe)
			{
				m_AkGameObject.m_positionOffsetData = null;
			}
		}

		AkGameObjPositionOffsetData positionOffsetData = m_AkGameObject.m_positionOffsetData;
		Vector3 positionOffset = Vector3.zero;

		EditorGUI.BeginChangeCheck();

		GUILayout.BeginVertical("Box");

		bool applyPosOffset = EditorGUILayout.Toggle("Apply Position Offset:", positionOffsetData != null);

		if (applyPosOffset != (positionOffsetData != null))
		{
			positionOffsetData = applyPosOffset ? new AkGameObjPositionOffsetData(true) : null;
		}

		if (positionOffsetData != null)
		{
			positionOffset = EditorGUILayout.Vector3Field("Position Offset", positionOffsetData.positionOffset);

			GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

			if (hideDefaultHandle)
			{
				if (GUILayout.Button("Show Main Transform"))
				{
					hideDefaultHandle = false;
					DefaultHandles.Hidden = hideDefaultHandle;
				}
			}
			else if (GUILayout.Button("Hide Main Transform"))
			{
				hideDefaultHandle = true;
				DefaultHandles.Hidden = hideDefaultHandle;
			}
		}
		else if (hideDefaultHandle)
		{
			hideDefaultHandle = false;
			DefaultHandles.Hidden = hideDefaultHandle;
		}

		GUILayout.EndVertical();

		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		GUILayout.BeginVertical("Box");

		bool isEnvironmentAware = EditorGUILayout.Toggle("Environment Aware:", m_AkGameObject.isEnvironmentAware);

		GUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "AkGameObj Parameter Change");

			m_AkGameObject.m_positionOffsetData = positionOffsetData;

			if (positionOffsetData != null)
				m_AkGameObject.m_positionOffsetData.positionOffset = positionOffset;

			m_AkGameObject.isEnvironmentAware = isEnvironmentAware;
		}

		if (isEnvironmentAware)
			RigidbodyCheck(m_AkGameObject.gameObject);

		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		GUILayout.BeginVertical("Box");
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(listeners);
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}
		GUILayout.EndVertical();
	}

	public static void RigidbodyCheck(GameObject gameObject)
	{
		if (WwiseSetupWizard.Settings.ShowMissingRigidBodyWarning && gameObject.GetComponent<Rigidbody>() == null)
		{
			GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

			GUILayout.BeginVertical("Box");

			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.red;
			style.wordWrap = true;
			GUILayout.Label("AkGameObj-AkEnvironment interactions require a Rigidbody component on the object or the environment.", style);
			if (GUILayout.Button("Add Rigidbody"))
			{
				Rigidbody rb = Undo.AddComponent<Rigidbody>(gameObject);
				rb.useGravity = false;
				rb.isKinematic = true;
			}

			GUILayout.EndVertical();
		}
	}

	void OnSceneGUI()
	{
		if (m_AkGameObject.m_positionOffsetData == null)
			return;

		EditorGUI.BeginChangeCheck();

		// Transform local offset to world coordinate
		Vector3 pos = m_AkGameObject.transform.TransformPoint(m_AkGameObject.m_positionOffsetData.positionOffset);

		// Get new handle position
		pos = Handles.PositionHandle(pos, Quaternion.identity);

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Position Offset Change");

			// Transform world offset to local coordinate
			m_AkGameObject.m_positionOffsetData.positionOffset = m_AkGameObject.transform.InverseTransformPoint(pos);
		}
	}
}
#endif