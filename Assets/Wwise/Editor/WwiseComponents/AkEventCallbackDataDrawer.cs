#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using AK.Wwise.Editor;

[CustomPropertyDrawer(typeof(AkEventCallbackData))]
class AkEventCallbackDataDrawer : PropertyDrawer
{
	float deltaHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
	float spacerHeight = EditorGUIUtility.standardVerticalSpacing;

	float callbackDeltaHeight = EditorGUIUtility.singleLineHeight;
	float callbackSpacerHeight = 5;
	float callbackSpacerWidth = 4;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float height = deltaHeight;

		var callbackData = (AkEventCallbackData)property.objectReferenceValue;
		if (callbackData != null)
		{
			height += (callbackDeltaHeight + callbackSpacerHeight) * callbackData.callbackGameObj.Count;
			height += deltaHeight * 2 + spacerHeight * 3;
		}

		return height;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		Rect initialRect = position;

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Use Callback:"));
		position.height = deltaHeight;

		bool useCallback = property.objectReferenceValue != null;

		EditorGUI.BeginChangeCheck();

		useCallback = GUI.Toggle(position, useCallback, "");

		if (EditorGUI.EndChangeCheck())
		{
			if (useCallback && property.objectReferenceValue == null)
			{
				var callbackData = ScriptableObject.CreateInstance<AkEventCallbackData>();
				callbackData.callbackFunc.Add(string.Empty);
				callbackData.callbackFlags.Add(0);
				callbackData.callbackGameObj.Add(null);
				property.objectReferenceValue = callbackData;
			}
			else if (!useCallback && property.objectReferenceValue != null)
			{
				Undo.RecordObject(property.objectReferenceValue, "Use Callback Change");

				property.objectReferenceValue = null;
				GUIUtility.keyboardControl = 0;
				GUIUtility.hotControl = 0;
			}
		}

		if (useCallback)
		{
			position.y += deltaHeight + spacerHeight;
			float removeButtonWidth = 20;
			float callbackFieldsWidth = initialRect.width - removeButtonWidth;
			position.width = callbackFieldsWidth / 3 - callbackSpacerWidth;

			position.x = initialRect.x + 0 * callbackFieldsWidth / 3;
			GUI.Label(position, "Game Object");

			position.x = initialRect.x + 1 * callbackFieldsWidth / 3;
			GUI.Label(position, "Callback Function");

			position.x = initialRect.x + 2 * callbackFieldsWidth / 3;
			GUI.Label(position, "Callback Flags");

			var callbackData = (AkEventCallbackData)property.objectReferenceValue;
			callbackData.uFlags = 0;

			for (int i = 0; i < callbackData.callbackFunc.Count; i++)
			{
				EditorGUI.BeginChangeCheck();

				position.y += callbackDeltaHeight + callbackSpacerHeight;
				position.x = initialRect.x + 0 * callbackFieldsWidth / 3;
				position.width = callbackFieldsWidth / 3 - callbackSpacerWidth;

				var gameObj = (GameObject)EditorGUI.ObjectField(position, callbackData.callbackGameObj[i], typeof(GameObject), true);

				position.x = initialRect.x + 1 * callbackFieldsWidth / 3;
				string func = EditorGUI.TextField(position, callbackData.callbackFunc[i]);

				position.x = initialRect.x + 2 * callbackFieldsWidth / 3;

				//Since some callback flags are unsupported, some bits are not used.
				//But when using EditorGUILayout.MaskField, clicking the third flag will set the third bit to one even if the third flag in the AkCallbackType enum is unsupported.
				//This is a problem because clicking the third supported flag would internally select the third flag in the AkCallbackType enum which is unsupported.
				//To solve this problem we use a mask for display and another one for the actual callback
				int displayMask = CallbackFlagsDrawer.GetDisplayMask(callbackData.callbackFlags[i]);
				displayMask = EditorGUI.MaskField(position, displayMask, CallbackFlagsDrawer.SupportedCallbackFlags);
				int flags = CallbackFlagsDrawer.GetWwiseCallbackMask(displayMask);

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(callbackData, "Modified Callback");

					callbackData.callbackGameObj[i] = gameObj;
					callbackData.callbackFunc[i] = func;
					callbackData.callbackFlags[i] = flags;
				}

				position.x = initialRect.x + callbackFieldsWidth;
				position.width = removeButtonWidth;

				if (GUI.Button(position, "X"))
				{
					Undo.RecordObject(callbackData, "Remove Callback");

					GUIUtility.keyboardControl = 0;
					GUIUtility.hotControl = 0;

					if (callbackData.callbackFunc.Count == 1)
					{
						callbackData.callbackFunc[0] = string.Empty;
						callbackData.callbackFlags[0] = 0;
						callbackData.callbackGameObj[0] = null;
					}
					else
					{
						callbackData.callbackFunc.RemoveAt(i);
						callbackData.callbackFlags.RemoveAt(i);
						callbackData.callbackGameObj.RemoveAt(i);

						i--;
						continue;
					}
				}

				callbackData.uFlags |= callbackData.callbackFlags[i];
			}

			position.x = initialRect.x;
			position.width = initialRect.width;
			position.y += deltaHeight + spacerHeight;

			if (GUI.Button(position, "Add"))
			{
				Undo.RecordObject(callbackData, "Add Callback");

				callbackData.callbackFunc.Add(string.Empty);
				callbackData.callbackFlags.Add(0);
				callbackData.callbackGameObj.Add(null);
			}
		}

		EditorGUI.EndProperty();
	}
}
#endif
