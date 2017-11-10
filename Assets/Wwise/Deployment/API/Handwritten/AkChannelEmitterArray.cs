#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2012 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class AkChannelEmitterArray : IDisposable
{
    public AkChannelEmitterArray(uint in_Count)
    {
		// Three vectors of 3 floats, plus a mask
        m_Buffer = Marshal.AllocHGlobal((int)in_Count * (sizeof(float) * 9 + sizeof(uint)));
        m_Current = m_Buffer;
        m_MaxCount = in_Count;
        m_Count = 0;
    }

    public uint Count
    { 
        get{ return m_Count;} 
    }

    ~AkChannelEmitterArray()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (m_Buffer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(m_Buffer);
            m_Buffer = IntPtr.Zero;
            m_MaxCount = 0;
        }
    }

    public void Reset()
    {
        m_Current = m_Buffer;
        m_Count = 0;
    }

    public void Add(Vector3 in_Pos, Vector3 in_Forward, Vector3 in_Top, uint in_ChannelMask)
    {
        if (m_Count >= m_MaxCount)
            throw new IndexOutOfRangeException("Out of range access in AkChannelEmitterArray");

        //Marshal doesn't do floats.  So copy the bytes themselves.  Grrr.
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.x), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.y), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Forward.z), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.x), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.y), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Top.z), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.x), 0));  
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.y), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, BitConverter.ToInt32(BitConverter.GetBytes(in_Pos.z), 0));
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(float));
        Marshal.WriteInt32(m_Current, (int)in_ChannelMask);
        m_Current = (IntPtr)(m_Current.ToInt64() + sizeof(uint));

        m_Count++;
    }

    public IntPtr m_Buffer;
    private IntPtr m_Current;
    private uint m_MaxCount;
    private uint m_Count;
};
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.