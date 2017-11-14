#define DISPLAY_GROUP_NAME_AND_VALUE_NAME

using System;
using UnityEditor;

namespace AK.Wwise.Editor
{
	[CustomPropertyDrawer(typeof(State))]
	public class StateDrawer : BaseTypeDrawer
	{
		SerializedProperty groupID;

		public override string UpdateIds(Guid[] in_guid)
		{
			var list = AkWwiseProjectInfo.GetData().StateWwu;

			for (int i = 0; i < list.Count; i++)
			{
				var group = list[i].List.Find(x => new Guid(x.Guid).Equals(in_guid[1]));

				if (group != null)
				{
					int index = group.ValueGuids.FindIndex(x => new Guid(x.bytes).Equals(in_guid[0]));

					if (index < 0)
						break;

					groupID.intValue = group.ID;
					ID.intValue = group.valueIDs[index];

#if DISPLAY_GROUP_NAME_AND_VALUE_NAME
					return group.Name + "/" + group.values[index];
#else
				return group.values[index];
#endif // DISPLAY_GROUP_NAME_AND_VALUE_NAME
				}
			}

			groupID.intValue = ID.intValue = 0;
			return string.Empty;
		}

		public override void SetupSerializedProperties(SerializedProperty property)
		{
			m_objectType = AkWwiseProjectData.WwiseObjectType.STATE;
			m_typeName = "State";

			groupID = property.FindPropertyRelative("groupID");
			m_guidProperty = new SerializedProperty[2];
			m_guidProperty[0] = property.FindPropertyRelative("valueGuid.Array");
			m_guidProperty[1] = property.FindPropertyRelative("groupGuid.Array");
		}
	}
}