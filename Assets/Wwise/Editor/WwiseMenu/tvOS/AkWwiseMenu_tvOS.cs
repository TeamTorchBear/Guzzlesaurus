#if UNITY_EDITOR
public class AkWwiseMenu_tvOS
{
	private const string MENU_PATH = "Help/Wwise Help/";
	private const string Platform = "tvOS";

	[UnityEditor.MenuItem(MENU_PATH + Platform, false, (int)AkWwiseHelpOrder.WwiseHelpOrder)]
	public static void OpenDoc() { AkDocHelper.OpenDoc(Platform); }
}
#endif // #if UNITY_EDITOR
