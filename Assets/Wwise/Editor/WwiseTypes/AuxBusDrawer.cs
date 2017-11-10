using System;
using UnityEditor;

namespace AK.Wwise.Editor
{
	[CustomPropertyDrawer(typeof(AuxBus))]
	public class AuxBusDrawer : BaseTypeDrawer
	{
		public override string UpdateIds(Guid[] in_guid)
		{
			var list = AkWwiseProjectInfo.GetData().AuxBusWwu;

			for (int i = 0; i < list.Count; i++)
			{
				var element = list[i].List.Find(x => new Guid(x.Guid).Equals(in_guid[0]));

				if (element != null)
				{
					ID.intValue = element.ID;
					return element.Name;
				}
			}

			ID.intValue = 0;
			return string.Empty;
		}

		public override void SetupSerializedProperties(SerializedProperty property)
		{
			m_objectType = AkWwiseProjectData.WwiseObjectType.AUXBUS;
			m_typeName = "AuxBus";

			m_guidProperty = new SerializedProperty[1];
			m_guidProperty[0] = property.FindPropertyRelative("valueGuid.Array");
		}
	}
}