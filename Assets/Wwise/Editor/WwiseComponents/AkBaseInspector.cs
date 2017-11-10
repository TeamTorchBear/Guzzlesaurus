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


public abstract class AkBaseInspector : Editor
{

	protected SerializedProperty[]					m_guidProperty;	//all components have 1 guid except switches and states which have 2. Index zero is value guid and index 1 is group guid
	protected AkWwiseProjectData.WwiseObjectType	m_objectType;
	protected string 								m_typeName;

	bool 			m_buttonWasPressed		= false;
	protected bool 	m_isInDropArea 			= false;		//is the mouse on top of the drop area(the button)	
	Rect 			m_dropAreaRelativePos 	= new Rect();	//button position relative to the inspector
	Rect			m_pickerPos = new Rect();

	public abstract void	OnChildInspectorGUI ();
	public abstract string	UpdateIds(Guid[] in_guid);  //set object properties and return its name

	private AkDragDropData GetAkDragDropData()
	{
		AkDragDropData DDData = DragAndDrop.GetGenericData(AkDragDropHelper.DragDropIdentifier) as AkDragDropData;
		return (DDData != null && DDData.typeName.Equals(m_typeName)) ? DDData : null;
	}

	private void HandleDragAndDrop(Event currentEvent, Rect dropArea)
	{
		if (currentEvent.type == EventType.DragExited)
		{
			// clear dragged data
			DragAndDrop.PrepareStartDrag();
		}
		else if (currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform)
		{
			if (dropArea.Contains(currentEvent.mousePosition))
			{
				var DDData = GetAkDragDropData();

				if (currentEvent.type == EventType.DragUpdated)
				{
					DragAndDrop.visualMode = DDData != null ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
				}
				else
				{
					DragAndDrop.AcceptDrag();

					if (DDData != null)
					{
						AkUtilities.SetByteArrayProperty(m_guidProperty[0], DDData.guid.ToByteArray());

						AkDragDropGroupData DDGroupData = DDData as AkDragDropGroupData;
						if (DDGroupData != null && m_guidProperty.Length > 1)
							AkUtilities.SetByteArrayProperty(m_guidProperty[1], DDGroupData.groupGuid.ToByteArray());

						//needed for the undo operation to work
						GUIUtility.hotControl = 0;
					}
				}
				currentEvent.Use();
			}
		}
	}


	public override void OnInspectorGUI()
	{
		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		OnChildInspectorGUI();

		serializedObject.ApplyModifiedProperties();

		var currentEvent = Event.current;
		HandleDragAndDrop(currentEvent, m_dropAreaRelativePos);

		/************************************************Update Properties**************************************************/
		Guid[] componentGuid = new Guid[m_guidProperty.Length];
		for(int i = 0; i < componentGuid.Length; i++)
		{
			byte[] guidBytes = AkUtilities.GetByteArrayProperty (m_guidProperty[i]);
			componentGuid[i] = guidBytes == null ? Guid.Empty : new Guid(guidBytes); 
		}

		string componentName = UpdateIds (componentGuid);
		/*******************************************************************************************************************/


		/********************************************Draw GUI***************************************************************/

		GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

		GUILayout.BeginHorizontal("box");
		{
			float inspectorWidth = Screen.width - GUI.skin.box.margin.left - GUI.skin.box.margin.right - 19;
			GUILayout.Label (m_typeName + " Name: ", GUILayout.Width (inspectorWidth * 0.4f));
			
			GUIStyle style = new GUIStyle(GUI.skin.button);
			style.alignment = TextAnchor.MiddleLeft;
			if(componentName.Equals(String.Empty))
			{
				componentName = "No " + m_typeName + " is currently selected";
				style.normal.textColor = Color.red;
			}
			
			if(GUILayout.Button(componentName, style, GUILayout.MaxWidth (inspectorWidth * 0.6f - GUI.skin.box.margin.right)))
			{
				m_buttonWasPressed = true;
				
				// We don't want to set object as dirty only because we clicked the button.
				// It will be set as dirty if the wwise object has been changed by the tree view.
				GUI.changed = false;
			}

			//GUILayoutUtility.GetLastRect and AkUtilities.GetLastRectAbsolute must be called in repaint mode 
			if(currentEvent.type == EventType.Repaint)
			{
				m_dropAreaRelativePos = GUILayoutUtility.GetLastRect();
				
				if(m_buttonWasPressed)
				{
					m_pickerPos = AkUtilities.GetLastRectAbsolute();
					EditorApplication.delayCall += DelayCreateCall;
					m_buttonWasPressed = false;
				}
			}
		}
		GUILayout.EndHorizontal ();
		
		/***********************************************************************************************************************/  

		if (GUI.changed)
		{
			EditorUtility.SetDirty(serializedObject.targetObject);
		}
	}

	void DelayCreateCall()
	{
		AkWwiseComponentPicker.Create(m_objectType, m_guidProperty, serializedObject, m_pickerPos);
	}
}

#endif