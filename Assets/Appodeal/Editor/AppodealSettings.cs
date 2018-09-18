using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public class AppodealSettings : ScriptableObject
{
	#if UNITY_EDITOR
    [MenuItem("Appodeal/SDK Documentation")]
	public static void OpenDocumentation()
	{
		string url = "http://www.appodeal.com/sdk/choose_framework?framework=2&full=1&platform=1";
		Application.OpenURL(url);
	}
	
	[MenuItem("Appodeal/Appodeal Homepage")]
	public static void OpenAppodealHome()
	{
		string url = "http://www.appodeal.com";
		Application.OpenURL(url);
	}
	#endif

}