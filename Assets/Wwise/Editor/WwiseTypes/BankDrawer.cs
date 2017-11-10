using System;
using UnityEditor;

namespace AK.Wwise.Editor
{
	[CustomPropertyDrawer(typeof(Bank))]
	public class BankDrawer : BaseTypeDrawer
	{
		SerializedProperty bankNameProperty;

		public override string UpdateIds(Guid[] in_guid)
		{
			var list = AkWwiseProjectInfo.GetData().BankWwu;

			for (int i = 0; i < list.Count; i++)
			{
				var element = list[i].List.Find(x => new Guid(x.Guid).Equals(in_guid[0]));

				if (element != null)
				{
					ID.intValue = element.ID;
					bankNameProperty.stringValue = element.Name;
					return bankNameProperty.stringValue;
				}
			}

			ID.intValue = 0;
			bankNameProperty.stringValue = string.Empty;
			return bankNameProperty.stringValue;
		}

		public override void SetupSerializedProperties(SerializedProperty property)
		{
			m_objectType = AkWwiseProjectData.WwiseObjectType.SOUNDBANK;
			m_typeName = "Bank";

			m_guidProperty = new SerializedProperty[1];
			m_guidProperty[0] = property.FindPropertyRelative("valueGuid.Array");

			bankNameProperty = property.FindPropertyRelative("name");
		}
	}
}