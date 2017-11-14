#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2012 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AkAuxSendArray
{
    const int MAX_COUNT = AkEnvironment.MAX_NB_ENVIRONMENTS;
    int SIZE_OF_AKAUXSENDVALUE = AkSoundEnginePINVOKE.CSharp_AkAuxSendValueProxy_GetSizeOf();

    public AkAuxSendArray()
    {
		m_Buffer = Marshal.AllocHGlobal(MAX_COUNT * SIZE_OF_AKAUXSENDVALUE);
		m_Count = 0;
    }

	~AkAuxSendArray()
	{
		Marshal.FreeHGlobal(m_Buffer);
		m_Buffer = IntPtr.Zero;
	}

	public void Reset()
	{
		m_Count = 0;
	}

	public void Add(UnityEngine.GameObject in_listenerGameObj, uint in_EnvID, float in_fValue)
	{
		if (isFull)
			return;

		AkSoundEnginePINVOKE.CSharp_AkAuxSendValueProxy_Set(GetObjectPtr(m_Count), AkSoundEngine.GetAkGameObjectID(in_listenerGameObj), in_EnvID, in_fValue);
		m_Count++;
    }

	public void Add(uint in_EnvID, float in_fValue)
	{
		if (isFull)
			return;

		AkSoundEnginePINVOKE.CSharp_AkAuxSendValueProxy_Set(GetObjectPtr(m_Count), AkSoundEngine.AK_INVALID_GAME_OBJECT, in_EnvID, in_fValue);
		m_Count++;
	}

	public bool Contains(UnityEngine.GameObject in_listenerGameObj, uint in_EnvID)
	{
		if (m_Buffer == IntPtr.Zero)
			return false;

		for (int i = 0; i < m_Count; i++)
			if (AkSoundEnginePINVOKE.CSharp_AkAuxSendValueProxy_IsSame(GetObjectPtr(i), AkSoundEngine.GetAkGameObjectID(in_listenerGameObj), in_EnvID))
				return true;

		return false;
	}

	public bool Contains(uint in_EnvID)
	{
		if (m_Buffer == IntPtr.Zero)
			return false;

		for (int i = 0; i < m_Count; i++)
			if (AkSoundEnginePINVOKE.CSharp_AkAuxSendValueProxy_IsSame(GetObjectPtr(i), AkSoundEngine.AK_INVALID_GAME_OBJECT, in_EnvID))
				return true;

		return false;
	}

	public bool isFull
	{
		get { return m_Count >= MAX_COUNT || m_Buffer == IntPtr.Zero; }
	}

    public IntPtr GetBuffer()
    {
        return m_Buffer;
    }

    public int Count()
    {
        return m_Count;
    }

    IntPtr GetObjectPtr(int index)
    {
        return (IntPtr)(m_Buffer.ToInt64() + SIZE_OF_AKAUXSENDVALUE * index);
    }

    private IntPtr m_Buffer;
    private int m_Count;
};
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.