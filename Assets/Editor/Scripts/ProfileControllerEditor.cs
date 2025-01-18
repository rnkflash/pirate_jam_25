using UnityEditor;

using UnityEngine;

public class ProfileControllerEditor
{
	#if UNITY_EDITOR
	[MenuItem("Tools/Browse profile file")]
	public static void OpenProfileFilePlace()
	{
		Application.OpenURL($"file://{Application.persistentDataPath}");
	}
	#endif
}
