#if !(UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

public partial class AkSoundEngine
{
	#region User Hooks - Extended for Auto-Registration
	class AutoObject
	{
		public AutoObject(UnityEngine.GameObject go)
		{
			gameObject = go;
			RegisterGameObj(gameObject, gameObject != null ? "AkAutoObject for " + gameObject.name : "AkAutoObject");
		}

		~AutoObject()
		{
			UnregisterGameObj(gameObject);
		}

		UnityEngine.GameObject gameObject;
	}

	static partial void PreGameObjectAPICallUserHook(UnityEngine.GameObject gameObject, ulong id)
	{
		if (!IsInRegisteredList(id))
		{
			//UnityEngine.Debug.Log("Consider adding an AkGameObj to " + gameObject.name);

			// If the object is not active, attaching an AkGameObj will not work.
			if (gameObject == null || !gameObject.activeInHierarchy)
			{
				new AutoObject(gameObject);
			}
			else
			{
				gameObject.AddComponent<AkGameObj>();
			}
		}
	}

	static System.Collections.Generic.HashSet<ulong> RegisteredGameObjects = new System.Collections.Generic.HashSet<ulong>();

	static partial void PostRegisterGameObjUserHook(AKRESULT result, UnityEngine.GameObject gameObject, ulong id)
	{
		if (result == AKRESULT.AK_Success)
		{
			RegisteredGameObjects.Add(id);
		}
	}

	static partial void PostUnregisterGameObjUserHook(AKRESULT result, UnityEngine.GameObject gameObject, ulong id)
	{
		if (result == AKRESULT.AK_Success)
		{
			RegisteredGameObjects.Remove(id);
		}
	}

	// Helper method that a user might want to implement
	static bool IsInRegisteredList(ulong id)
	{
		return RegisteredGameObjects.Contains(id);
	}

	// Helper method that a user might want to implement
	public static bool IsGameObjectRegistered(UnityEngine.GameObject in_gameObject)
	{
		return IsInRegisteredList(GetAkGameObjectID(in_gameObject));
	}
	#endregion
}
#endif // #if !(UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
