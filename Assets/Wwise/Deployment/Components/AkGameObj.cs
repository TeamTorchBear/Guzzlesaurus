#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections.Generic;


[AddComponentMenu("Wwise/AkGameObj")]
///@brief This component represents a sound object in your scene tracking its position and other game syncs such as Switches, RTPC and environment values. You can add this to any object that will emit sound, and it will be added to any object that an AkAudioListener is attached to. Note that if it is not present, Wwise will add it automatically, with the default values, to any Unity Game Object that is passed to Wwise.
/// \sa
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__gameobj.html" target="_blank">Integration Details - Game Objects</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__events.html" target="_blank">Integration Details - Events</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__listeners.html" target="_blank">Integrating Listeners</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__switch.html" target="_blank">Integration Details - Switches</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__states.html" target="_blank">Integration Details - States</a> (Note: This is described in the Wwise SDK documentation.)
/// - <a href="https://www.audiokinetic.com/library/edge/?source=SDK&id=soundengine__environments.html" target="_blank">Integration Details - Environments and Game-defined Auxiliary Sends</a> (Note: This is described in the Wwise SDK documentation.)
[ExecuteInEditMode] //ExecuteInEditMode necessary to maintain proper state of isStaticObject.
public class AkGameObj : MonoBehaviour
{
	/// When not set to null, the position will be offset relative to the Game Object position by the Position Offset
	public AkGameObjPositionOffsetData m_positionOffsetData = null;

	/// Is this object affected by Environment changes?  Set to false if not affected in order to save some useless calls.  Default is true.
	public bool isEnvironmentAware = true;
	private AkGameObjEnvironmentData m_envData = null;

	/// Maintains and persists the Static setting of the gameobject, which is available only in the editor.
	[SerializeField]
	private bool isStaticObject = false;
	private AkGameObjPositionData m_posData = null;

	/// Cache the bounds to avoid calls to GetComponent()
	private Collider m_Collider;

	[SerializeField]
	private AkGameObjListenerList m_listeners = new AkGameObjListenerList();

	/// <summary>
	/// Adds an AkAudioListener to the container of listeners listening to this gameobject.
	/// </summary>
	/// <param name="listener"></param>
	/// <returns>Returns true if the listener was not previously in the list, false otherwise.</returns>
	public bool AddListener(AkAudioListener listener)
	{
		return m_listeners.Add(listener);
	}

	/// <summary>
	/// Removes an AkAudioListener from the container of listeners listening to this gameobject.
	/// </summary>
	/// <param name="listener"></param>
	/// <returns>Returns true if the listener was previously in the list, false otherwise.</returns>
	public bool RemoveListener(AkAudioListener listener)
	{
		return m_listeners.Remove(listener);
	}

	void Awake()
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating)
			return;

		// If the object was marked as static, don't update its position to save cycles.
		if (!UnityEditor.EditorApplication.isPlaying)
		{
			UnityEditor.EditorApplication.update += this.CheckStaticStatus;
			return;
		}
#endif
		if (!isStaticObject)
			m_posData = new AkGameObjPositionData();

		// Cache the bounds to avoid calls to GetComponent()
		m_Collider = GetComponent<Collider>();

		//Register a Game Object in the sound engine, with its name.
		AKRESULT res = AkSoundEngine.RegisterGameObj(gameObject, gameObject.name);
		if (res == AKRESULT.AK_Success)
		{
			// Get position with offset
			Vector3 position = GetPosition();

			//Set the original position
			AkSoundEngine.SetObjectPosition(
				gameObject,
				position.x, position.y, position.z,
				transform.forward.x, transform.forward.y, transform.forward.z,
				transform.up.x, transform.up.y, transform.up.z);

			if (isEnvironmentAware && m_Collider)
			{
				m_envData = new AkGameObjEnvironmentData();
				//Check if this object is also an environment.
				m_envData.AddAkEnvironment(m_Collider, m_Collider);
			}

			m_listeners.Initialize();
		}
	}

	private void CheckStaticStatus()
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating)
			return;

		if (gameObject != null && isStaticObject != gameObject.isStatic)
		{
			isStaticObject = gameObject.isStatic;
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
	}

	void OnEnable()
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating)
			return;
#endif

		//if enabled is set to false, then the update function wont be called
		enabled = !isStaticObject;
	}

	void OnDestroy()
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating)
			return;

		if (!UnityEditor.EditorApplication.isPlaying)
			UnityEditor.EditorApplication.update -= this.CheckStaticStatus;
#endif

		// We can't do the code in OnDestroy if the gameObj is unregistered, so do it now.		
		AkUnityEventHandler[] eventHandlers = gameObject.GetComponents<AkUnityEventHandler>();
		foreach (AkUnityEventHandler handler in eventHandlers)
		{
			if (handler.triggerList.Contains(AkUnityEventHandler.DESTROY_TRIGGER_ID))
				handler.DoDestroy();
		}

#if UNITY_EDITOR
		if (UnityEditor.EditorApplication.isPlaying)
#endif
		{
			if (AkSoundEngine.IsInitialized())
				AkSoundEngine.UnregisterGameObj(gameObject);
		}
	}

	void Update()
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating || !UnityEditor.EditorApplication.isPlaying)
			return;
#endif

		if (isEnvironmentAware && m_envData != null)
			m_envData.UpdateAuxSend(gameObject, transform.position);

		m_listeners.Refresh(gameObject);

		if (isStaticObject)
			return;

		// Get position with offset
		Vector3 position = GetPosition();

		//Didn't move.  Do nothing.
		if (m_posData.position == position && m_posData.forward == transform.forward && m_posData.up == transform.up)
			return;

		m_posData.position = position;
		m_posData.forward = transform.forward;
		m_posData.up = transform.up;

		//Update position
		AkSoundEngine.SetObjectPosition(
			gameObject,
			position.x, position.y, position.z,
			transform.forward.x, transform.forward.y, transform.forward.z,
			transform.up.x, transform.up.y, transform.up.z);
	}

	/// Gets the position including the position offset, if applyPositionOffset is enabled.
	/// \return  The position.
	public Vector3 GetPosition()
	{
		if (m_positionOffsetData != null)
		{
			// Get offset in world space
			Vector3 worldOffset = transform.rotation * m_positionOffsetData.positionOffset;

			// Add offset to game object position
			return transform.position + worldOffset;
		}

		return transform.position;
	}


	void OnTriggerEnter(Collider other)
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating || !UnityEditor.EditorApplication.isPlaying)
			return;
#endif

		if (isEnvironmentAware && m_envData != null)
			m_envData.AddAkEnvironment(other, m_Collider);
	}

	void OnTriggerExit(Collider other)
	{
#if UNITY_EDITOR
		if (AkUtilities.IsMigrating || !UnityEditor.EditorApplication.isPlaying)
			return;
#endif

		if (isEnvironmentAware && m_envData != null)
			m_envData.RemoveAkEnvironment(other, m_Collider);
	}

#if UNITY_EDITOR
	public void OnDrawGizmosSelected()
	{
		if (AkUtilities.IsMigrating)
			return;

		Vector3 position = GetPosition();
		Gizmos.DrawIcon(position, "WwiseAudioSpeaker.png", false);
	}
#endif

	#region WwiseMigration

#pragma warning disable 0414 // private field assigned but not used.

	[SerializeField]
	private AkGameObjPosOffsetData m_posOffsetData = null;

	// Wwise v2016.2 and below supported up to 8 listeners[0-7].
	const int AK_NUM_LISTENERS = 8;

	[SerializeField]
	/// Listener 0 by default.
	private int listenerMask = 1; 

#pragma warning restore 0414 // private field assigned but not used.

#if UNITY_EDITOR
	public void Migrate9()
	{
		Debug.Log("WwiseUnity: AkGameObj.Migrate9 for " + gameObject.name);

		const int ALL_LISTENER_MASK = (1 << AK_NUM_LISTENERS) - 1;
		if ((listenerMask & ALL_LISTENER_MASK) == ALL_LISTENER_MASK)
			listenerMask = 1;
	}

	public void Migrate10()
	{
		Debug.Log("WwiseUnity: AkGameObj.Migrate10 for " + gameObject.name);

		if (m_posOffsetData != null)
		{
			m_positionOffsetData = new AkGameObjPositionOffsetData(true);
			m_positionOffsetData.positionOffset = m_posOffsetData.positionOffset;
			m_posOffsetData = null;
		}
	}

	private class Migration14Data
	{
		List<AkAudioListener>[] listeners = new List<AkAudioListener>[AK_NUM_LISTENERS];

		public Migration14Data()
		{
			int fullSceneListenerMask = 0;

			// Get all AkAudioListeners in the scene.
			var listenerObjects = FindObjectsOfType(typeof(AkAudioListener)) as AkAudioListener[];
			foreach (var listener in listenerObjects)
			{
				// Add AkGameObj to AkAudioListeners
				if (listener.GetComponent<AkGameObj>() == null)
				{
					AkGameObj akGameObj = listener.gameObject.AddComponent<AkGameObj>();
					if (akGameObj)
					{
						akGameObj.isEnvironmentAware = false;
						Debug.Log("WwiseUnity: Added AkGameObj to <" + listener.gameObject.name + ">.");
					}
					else
						Debug.LogError("WwiseUnity: Failed to add AkGameObj to <" + listener.gameObject.name + ">.");
				}

				var listenerId = listener.listenerId;
				if (listenerId >= 0 && listenerId < AK_NUM_LISTENERS)
				{
					if (listeners[listenerId] == null)
						listeners[listenerId] = new List<AkAudioListener>();

					listeners[listenerId].Add(listener);
					fullSceneListenerMask |= (1 << listenerId);
				}
				else
				{
					Debug.LogError("WwiseUnity: Invalid listenerId <" + listenerId + "> found during migration.");
				}
			}

			if (fullSceneListenerMask == 0)
			{
				Debug.LogWarning("WwiseUnity: Listeners were not added via components within this Scene.");
				listeners = null;
			}
			else
			{
				for (int ii = 0; ii < AK_NUM_LISTENERS; ++ii)
					if (listeners[ii] != null && listeners[ii].Count > 1)
						Debug.LogWarning("WwiseUnity: Multiple listeners <" + listeners[ii].Count + "> with same listenerId <" + ii + "> found during migration.");

				if (fullSceneListenerMask == 1)
				{
					Debug.Log("WwiseUnity: Default listeners will be used for this Scene.");
					listeners = null;
				}
			}
		}

		public void Migrate(AkGameObj akGameObj)
		{
			if (listeners != null)
			{
				for (int ii = 0; ii < AK_NUM_LISTENERS; ++ii)
				{
					int idMask = (1 << ii);
					if ((akGameObj.listenerMask & idMask) != 0 && listeners[ii] != null)
					{
						foreach (var listener in listeners[ii])
							akGameObj.m_listeners.AddToInitialListenerList(listener);
					}
				}
			}
		}
	}

	static private Migration14Data migration14data = null;

	static public void PreMigration14()
	{
		migration14data = new Migration14Data();
	}

	public void Migrate14()
	{
		Debug.Log("WwiseUnity: AkGameObj.Migrate14 for " + gameObject.name);

		if (migration14data != null)
			migration14data.Migrate(this);
	}

	static public void PostMigration14()
	{
		migration14data = null;
	}

#endif

	#endregion
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.