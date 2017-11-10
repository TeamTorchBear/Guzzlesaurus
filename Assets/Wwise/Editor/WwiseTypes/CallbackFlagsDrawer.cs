using UnityEngine;
using UnityEditor;
using System;

namespace AK.Wwise.Editor
{
	[CustomPropertyDrawer(typeof(CallbackFlags))]
	public class CallbackFlagsDrawer : PropertyDrawer
	{
		private static void SetupSupportedCallbackValuesAndFlags()
		{
			int[] callbacktypes = (int[])Enum.GetValues(typeof(AkCallbackType));
			int[] unsupportedCallbackValues =
			{
				(int)AkCallbackType.AK_SpeakerVolumeMatrix,
				(int)AkCallbackType.AK_MusicSyncAll,
				(int)AkCallbackType.AK_CallbackBits,
				(int)AkCallbackType.AK_Monitoring,
				(int)AkCallbackType.AK_AudioSourceChange,
				(int)AkCallbackType.AK_Bank,
				(int)AkCallbackType.AK_AudioInterruption
			};

			m_supportedCallbackFlags = new string[callbacktypes.Length - unsupportedCallbackValues.Length];
			m_supportedCallbackValues = new int[callbacktypes.Length - unsupportedCallbackValues.Length];

			int index = 0;
			for (int i = 0; i < callbacktypes.Length; i++)
			{
				if (!Contains(unsupportedCallbackValues, callbacktypes[i]))
				{
					m_supportedCallbackFlags[index] = Enum.GetName(typeof(AkCallbackType), callbacktypes[i]).Substring(3);
					m_supportedCallbackValues[index] = callbacktypes[i];
					index++;
				}
			}
		}

		public static string[] SupportedCallbackFlags
		{
			get
			{
				if (m_supportedCallbackFlags == null)
					SetupSupportedCallbackValuesAndFlags();

				return m_supportedCallbackFlags;
			}
		}

		static string[] m_supportedCallbackFlags = null;
		static int[] m_supportedCallbackValues = null;

		public static int GetDisplayMask(int in_wwiseCallbackMask)
		{
			if (m_supportedCallbackValues == null)
				SetupSupportedCallbackValuesAndFlags();

			int displayMask = 0;
			for (int i = 0; i < m_supportedCallbackValues.Length; i++)
				if ((m_supportedCallbackValues[i] & in_wwiseCallbackMask) != 0)
					displayMask |= (1 << i);

			return displayMask;
		}

		public static int GetWwiseCallbackMask(int in_displayMask)
		{
			if (m_supportedCallbackValues == null)
				SetupSupportedCallbackValuesAndFlags();

			int wwiseCallbackMask = 0;
			for (int i = 0; i < m_supportedCallbackValues.Length; i++)
				if ((in_displayMask & (1 << i)) != 0)
					wwiseCallbackMask |= m_supportedCallbackValues[i];

			return wwiseCallbackMask;
		}

		private static bool Contains(int[] in_array, int in_value)
		{
			for (int i = 0; i < in_array.Length; i++)
				if (in_array[i] == in_value)
					return true;

			return false;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.LabelField(position, label);

			position.x += EditorGUIUtility.labelWidth;
			position.width -= EditorGUIUtility.labelWidth;

			SerializedProperty value = property.FindPropertyRelative("value");

			//Since some callback flags are unsupported, some bits are not used.
			//But when using EditorGUILayout.MaskField, clicking the third flag will set the third bit to one even if the third flag in the AkCallbackType enum is unsupported.
			//This is a problem because clicking the third supported flag would internally select the third flag in the AkCallbackType enum which is unsupported.
			//To solve this problem we use a mask for display and another one for the actual callback
			int displayMask = GetDisplayMask(value.intValue);
			displayMask = EditorGUI.MaskField(position, displayMask, SupportedCallbackFlags);
			value.intValue = GetWwiseCallbackMask(displayMask);
		}
	}
}
