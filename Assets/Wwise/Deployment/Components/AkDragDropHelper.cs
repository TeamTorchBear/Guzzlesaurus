#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;


public class AkDragDropData
{
	public string name;
	public Guid guid;
	public int ID;
	public string typeName;
}

public class AkDragDropGroupData : AkDragDropData
{
	public Guid groupGuid;
	public int groupID;
}


/// <summary>
///  @brief This class is used to perform DragAndDrop operations from the AkWwisePicker to any GameObject.
///  We found out that DragAndDrop operations in Unity do not transfer components, but only scripts. This
///  prevented us to set the name and ID of our components before performing the drag and drop. To fix this,
///  the DragAndDrop operation always transfers a AkDragDropHelper component that gets instantiated on the 
///  target GameObject. On its first Update() call, it will parse the DragAndDrop structure, which contains
///  all necessary information to instantiate the correct component, with the correct information
/// </summary>
[ExecuteInEditMode]
public class AkDragDropHelper : MonoBehaviour
{
	public static string DragDropIdentifier = "AKWwiseDDInfo";

	void Awake()
	{
		var DDData = DragAndDrop.GetGenericData(DragDropIdentifier) as AkDragDropData;
		var DDGroupData = DDData as AkDragDropGroupData;

		if (DDGroupData != null)
		{
			switch (DDData.typeName)
			{
				case "State":
					CreateState(DDGroupData);
					break;
				case "Switch":
					CreateSwitch(DDGroupData);
					break;
			}
		}
		else if (DDData != null)
		{
			switch (DDData.typeName)
			{
				case "AuxBus":
					CreateAuxBus(DDData);
					break;
				case "Event":
					CreateAmbient(DDData);
					break;
				case "Bank":
					CreateBank(DDData);
					break;
			}
		}

		GUIUtility.hotControl = 0;
	}

	void Start()
	{
		// Don't forget to destroy the AkDragDropHelper when we're done!
		Component.DestroyImmediate(this);
	}

	bool HasSameEnvironment(Guid auxBusGuid)
	{
		AkEnvironment[] akEnvironments = gameObject.GetComponents<AkEnvironment>();
		for (int i = 0; i < akEnvironments.Length; i++)
		{
			if (new Guid(akEnvironments[i].valueGuid).Equals(auxBusGuid))
				return true;
		}

		return false;
	}

	void CreateAuxBus(AkDragDropData DDData)
	{
		if (HasSameEnvironment(DDData.guid))
			return;

		AkEnvironment akEnvironment = Undo.AddComponent<AkEnvironment>(gameObject);
		if (akEnvironment != null)
			SetTypeValue(ref akEnvironment.valueGuid, ref akEnvironment.m_auxBusID, DDData);
	}

	void CreateAmbient(AkDragDropData DDData)
	{
		AkAmbient ambient = Undo.AddComponent<AkAmbient>(gameObject);
		if (ambient != null)
			SetTypeValue(ref ambient.valueGuid, ref ambient.eventID, DDData);
	}

	void CreateBank(AkDragDropData DDData)
	{
		AkBank bank = Undo.AddComponent<AkBank>(gameObject);

		if (bank != null)
		{
			int valueID = 0;
			SetTypeValue(ref bank.valueGuid, ref valueID, DDData);
			bank.bankName = DDData.name;
		}
	}

	void CreateState(AkDragDropGroupData DDGroupData)
	{
		AkState akState = Undo.AddComponent<AkState>(gameObject);
		if (akState != null)
		{
			SetTypeValue(ref akState.valueGuid, ref akState.valueID, DDGroupData);
			SetGroupTypeValue(ref akState.groupGuid, ref akState.groupID, DDGroupData);
		}
	}

	void CreateSwitch(AkDragDropGroupData DDGroupData)
	{
		AkSwitch akSwitch = Undo.AddComponent<AkSwitch>(gameObject);
		if (akSwitch != null)
		{
			SetTypeValue(ref akSwitch.valueGuid, ref akSwitch.valueID, DDGroupData);
			SetGroupTypeValue(ref akSwitch.groupGuid, ref akSwitch.groupID, DDGroupData);
		}
	}

	void SetTypeValue(ref byte[] valueGuid, ref int ID, AkDragDropData DDData)
	{
		DDData.guid.ToByteArray().CopyTo(valueGuid, 0);
		ID = DDData.ID;
	}

	void SetGroupTypeValue(ref byte[] groupGuid, ref int groupID, AkDragDropGroupData DDGroupData)
	{
		DDGroupData.groupGuid.ToByteArray().CopyTo(groupGuid, 0);
		groupID = DDGroupData.groupID;
	}
}
#endif // UNITY_EDITOR
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.