using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AK.Wwise.Editor
{
	public abstract class BaseTypeDrawer : PropertyDrawer
	{
		protected SerializedProperty[] m_guidProperty;  //all components have 1 guid except switches and states which have 2. Index zero is value guid and index 1 is group guid
		protected SerializedProperty ID;

		protected AkWwiseProjectData.WwiseObjectType m_objectType;
		protected string m_typeName;

		private static Rect s_pickerPos = new Rect();

		private static Rect s_pressedPosition = new Rect();
		private static bool s_buttonWasPressed = false;

		private static SerializedObject s_serializedObject;
		private static SerializedProperty[] s_guidProperty;
		private static AkWwiseProjectData.WwiseObjectType s_objectType;


		public abstract string UpdateIds(Guid[] in_guid);
		public abstract void SetupSerializedProperties(SerializedProperty property);

		// taken and modified from AkUtilities.GetLastRectAbsolute()
		public static Rect GetLastRectAbsolute()
		{
			Type inspectorType = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");

			FieldInfo currentInspectorFieldInfo = inspectorType.GetField("s_CurrentInspectorWindow", BindingFlags.Public | BindingFlags.Static);
			PropertyInfo positionPropInfo = inspectorType.GetProperty("position", BindingFlags.Public | BindingFlags.Instance);

			Rect InspectorPosition = (Rect)positionPropInfo.GetValue(currentInspectorFieldInfo.GetValue(null), null);

			Rect absolutePos = new Rect(InspectorPosition.x, InspectorPosition.y, InspectorPosition.width, 0);
			return absolutePos;
		}

		private AkDragDropData GetAkDragDropData()
		{
			AkDragDropData DDData = DragAndDrop.GetGenericData(AkDragDropHelper.DragDropIdentifier) as AkDragDropData;
			return (DDData != null && DDData.typeName.Equals(m_typeName)) ? DDData : null;
		}

		private void HandleDragAndDrop(UnityEngine.Event currentEvent, Rect dropArea)
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

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);

			SetupSerializedProperties(property);
			ID = property.FindPropertyRelative("ID");

			// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			/************************************************Update Properties**************************************************/
			Guid[] componentGuid = new Guid[m_guidProperty.Length];
			for (int i = 0; i < componentGuid.Length; i++)
			{
				byte[] guidBytes = AkUtilities.GetByteArrayProperty(m_guidProperty[i]);
				componentGuid[i] = guidBytes == null ? Guid.Empty : new Guid(guidBytes);
			}

			string componentName = UpdateIds(componentGuid);
			/*******************************************************************************************************************/


			/********************************************Draw GUI***************************************************************/
			var style = new GUIStyle(GUI.skin.button);
			style.alignment = TextAnchor.MiddleLeft;
			style.fontStyle = FontStyle.Normal;

			if (componentName.Equals(String.Empty))
			{
				componentName = "No " + m_typeName + " is currently selected";
				style.normal.textColor = Color.red;
			}

			if (GUI.Button(position, componentName, style))
			{
				s_pressedPosition = position;
				s_buttonWasPressed = true;

				// We don't want to set object as dirty only because we clicked the button.
				// It will be set as dirty if the wwise object has been changed by the tree view.
				GUI.changed = false;
			}

			var currentEvent = UnityEngine.Event.current;

			if (currentEvent.type == EventType.Repaint)
			{
				if (s_buttonWasPressed && s_pressedPosition.Equals(position))
				{
					s_serializedObject = property.serializedObject;
					s_pickerPos = GetLastRectAbsolute();
					s_guidProperty = m_guidProperty;
					s_objectType = m_objectType;

					EditorApplication.delayCall += DelayCreateCall;
					s_buttonWasPressed = false;
				}
			}

			HandleDragAndDrop(currentEvent, position);

			EditorGUI.EndProperty();
		}

		private void DelayCreateCall()
		{
			AkWwiseComponentPicker.Create(s_objectType, s_guidProperty, s_serializedObject, s_pickerPos);
		}
	}
}