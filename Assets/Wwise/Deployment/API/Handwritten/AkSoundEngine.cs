#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2017 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////


using System;

public partial class AkSoundEngine
{
	#region WwiseMigration
	public static AKRESULT SetObjectPosition(UnityEngine.GameObject in_gameObject, float PosX, float PosY, float PosZ, float FrontX, float FrontY, float FrontZ)
	{
		throw new NotImplementedException("SetObjectPosition now requires a top vector. Please change your scripts to use the SetObjectPosition overload that specifies the top vector.");
	}

	// AK::SoundEngine::AddSecondaryOutput(AkUInt32,AkAudioOutputType,const AkGameObjectID *,AkUInt32,AkUInt32,AkUniqueID);
	public static AKRESULT AddSecondaryOutput() //TODO
	{
		return AKRESULT.AK_Success;
	}
	#endregion

	#region String Marshalling
	/// <summary>
	/// Converts "char*" C-strings to C# strings.
	/// </summary>
	/// <param name="ptr">"char*" memory pointer passed to C# as an IntPtr.</param>
	/// <returns>Converted string.</returns>
	public static string StringFromIntPtrString(IntPtr ptr)
	{
		return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
	}

	/// <summary>
	/// Converts "wchar_t*" C-strings to C# strings.
	/// </summary>
	/// <param name="ptr">"wchar_t*" memory pointer passed to C# as an IntPtr.</param>
	/// <returns>Converted string.</returns>
	public static string StringFromIntPtrWString(IntPtr ptr)
	{
		return System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);
	}

	/// <summary>
	/// Converts "AkOSChar*" C-strings to C# strings.
	/// </summary>
	/// <param name="ptr">"AkOSChar*" memory pointer passed to C# as an IntPtr.</param>
	/// <returns>Converted string.</returns>
	public static string StringFromIntPtrOSString(IntPtr ptr)
	{
		//return System.Runtime.InteropServices.Marshal.PtrToStringAuto(ptr);
#if UNITY_WSA
		return StringFromIntPtrWString(ptr);
#elif UNITY_EDITOR
		if (System.IO.Path.DirectorySeparatorChar == '/')
			return StringFromIntPtrString(ptr);
		else
			return StringFromIntPtrWString(ptr);
#elif UNITY_STANDALONE_WIN || UNITY_XBOXONE
		return StringFromIntPtrWString(ptr);
#else
		return StringFromIntPtrString(ptr);
#endif
	}
	#endregion

	#region GameObject Hash Function
	public delegate ulong GameObjectHashFunction(UnityEngine.GameObject gameObject);

	private static ulong InternalGameObjectHash(UnityEngine.GameObject gameObject)
	{
		return (gameObject == null) ? AK_INVALID_GAME_OBJECT : (ulong)gameObject.GetInstanceID();
	}

	public static GameObjectHashFunction GameObjectHash
	{
		get { return gameObjectHash; }
		set { gameObjectHash = (value == null) ? InternalGameObjectHash : value; }
	}

	private static GameObjectHashFunction gameObjectHash = InternalGameObjectHash;

	public static ulong GetAkGameObjectID(UnityEngine.GameObject gameObject)
	{
		return gameObjectHash(gameObject);
	}
	#endregion

	#region Registration Functions
	public static AKRESULT RegisterGameObj(UnityEngine.GameObject gameObject)
	{
		var id = GetAkGameObjectID(gameObject);
		var res = (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterGameObjInternal(id);
		PostRegisterGameObjUserHook(res, gameObject, id);
		return res;
	}

	public static AKRESULT RegisterGameObj(UnityEngine.GameObject gameObject, string name)
	{
		var id = GetAkGameObjectID(gameObject);
		var res = (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterGameObjInternal_WithName(id, name);
		PostRegisterGameObjUserHook(res, gameObject, id);
		return res;
	}

	public static AKRESULT UnregisterGameObj(UnityEngine.GameObject gameObject)
	{
		var id = GetAkGameObjectID(gameObject);
		var res = (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnregisterGameObjInternal(id);
		PostUnregisterGameObjUserHook(res, gameObject, id);
		return res;
	}
	#endregion

	#region User Hooks
	public static void PreGameObjectAPICall(UnityEngine.GameObject gameObject, ulong id)
	{
		PreGameObjectAPICallUserHook(gameObject, id);
	}

	/// <summary>
	/// User hook called within all Wwise integration functions that receive GameObjects and do not perform (un)registration. This is called
	/// before values are sent to the native plugin code. An example use could be to register game objects that were not previously registered.
	/// </summary>
	/// <param name="gameObject">The GameObject being processed.</param>
	/// <param name="id">The ulong returned from GameObjectHash that represents this GameObject in Wwise.</param>
	static partial void PreGameObjectAPICallUserHook(UnityEngine.GameObject gameObject, ulong id);

	/// <summary>
	/// User hook called after RegisterGameObj(). An example use could be to add the id and gameObject to a dictionary upon AK_Success.
	/// </summary>
	/// <param name="result">The result from calling RegisterGameObj() on gameObject.</param>
	/// <param name="gameObject">The GameObject that RegisterGameObj() was called on.</param>
	/// <param name="id">The ulong returned from GameObjectHash that represents this GameObject in Wwise.</param>
	static partial void PostRegisterGameObjUserHook(AKRESULT result, UnityEngine.GameObject gameObject, ulong id);

	/// <summary>
	/// User hook called after UnregisterGameObj(). An example use could be to remove the id and gameObject from a dictionary upon AK_Success.
	/// </summary>
	/// <param name="result">The result from calling UnregisterGameObj() on gameObject.</param>
	/// <param name="gameObject">The GameObject that UnregisterGameObj() was called on.</param>
	/// <param name="id">The ulong returned from GameObjectHash that represents this GameObject in Wwise.</param>
	static partial void PostUnregisterGameObjUserHook(AKRESULT result, UnityEngine.GameObject gameObject, ulong id);
	#endregion
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.