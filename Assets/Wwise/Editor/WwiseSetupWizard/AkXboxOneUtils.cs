using UnityEditor;


public class AkXboxOneUtils
{
	[UnityEditor.MenuItem("Assets/Wwise/Xbox One/Enable Network Sockets")]
	public static void EnableXboxOneNetworkSockets()
	{
        string[] SocketNames = { "WwiseDiscoverySocket", "WwiseCommandSocket", "WwiseNotificationSocket" };
        string[] SocketPorts = { "24024", "24025", "24026" };
        int[] Protocols = { 1, 0, 0 };
        int[] Usages = { 0, 1, 4, 7 };
        string[] TemplateNames = { "WwiseDiscovery", "WwiseCommand", "WwiseNotification" };
        int SessionRequirement = 0;
        int[] DeviceUsages = { 0 };

        for (int i = 0; i < 3; i++)
        {
            PlayerSettings.XboxOne.SetSocketDefinition(SocketNames[i], SocketPorts[i], Protocols[i], Usages, TemplateNames[i], SessionRequirement, DeviceUsages);
        }
	}
}