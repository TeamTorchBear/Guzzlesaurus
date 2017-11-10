#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(AkGameObjListenerList))]
class AkGameObjListenerListDrawer : PropertyDrawer
{
	const int deltaHeight = 18;
	const int spacerHeight = 3;
	const int spacerWidth = 4;

	const int listenerDeltaHeight = 16;
	const int listenerSpacerHeight = 5;
	const int listenerSpacerWidth = 4;

	const int removeButtonWidth = 20;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float height = deltaHeight * 2 + spacerHeight;

		var listenerListProperty = property.FindPropertyRelative("initialListenerList");
		if (listenerListProperty != null && listenerListProperty.isArray)
			height += (deltaHeight + spacerHeight) * listenerListProperty.arraySize + spacerHeight;

		return height;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Rect initialRect = position;

		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Initial Listener List"));

		var listenerListProperty = property.FindPropertyRelative("initialListenerList");
		if (listenerListProperty.isArray)
		{
			position.height = deltaHeight;

			bool usedDefaultListeners = true;
			for (int ii = 0; ii < listenerListProperty.arraySize; ++ii)
				if (listenerListProperty.GetArrayElementAtIndex(ii).objectReferenceValue != null)
					usedDefaultListeners = false;

			bool useDefaultListeners = GUI.Toggle(position, usedDefaultListeners, "Use Default Listeners");
			if (useDefaultListeners && !usedDefaultListeners)
				listenerListProperty.arraySize = 0;

			for (int ii = 0; ii < listenerListProperty.arraySize; ++ii)
			{
				float listenerFieldWidth = initialRect.width - removeButtonWidth;
				position.y += listenerDeltaHeight + listenerSpacerHeight;
				position.x = initialRect.x;
				position.width = listenerFieldWidth - listenerSpacerWidth;

				var listenerProperty = listenerListProperty.GetArrayElementAtIndex(ii);
				EditorGUI.PropertyField(position, listenerProperty, new GUIContent("Listener " + ii));

				position.x = initialRect.x + listenerFieldWidth;
				position.width = removeButtonWidth;

				if (GUI.Button(position, "X"))
				{
					listenerProperty.objectReferenceValue = null;
					listenerListProperty.DeleteArrayElementAtIndex(ii);
					--ii;
					continue;
				}
			}

			position.x = initialRect.x;
			position.width = initialRect.width;
			position.y += deltaHeight + spacerHeight;

			if (GUI.Button(position, "Add Listener"))
			{
				int lastPosition = listenerListProperty.arraySize;
				listenerListProperty.arraySize = lastPosition + 1;

				// Avoid copying the previous last array element into the newly added last position
				var listenerProperty = listenerListProperty.GetArrayElementAtIndex(lastPosition);
				listenerProperty.objectReferenceValue = null;
			}
		}

		EditorGUI.EndProperty();
	}
}
#endif
