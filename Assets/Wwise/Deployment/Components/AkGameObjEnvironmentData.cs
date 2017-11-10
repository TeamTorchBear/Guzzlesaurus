#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2014 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections.Generic;


public class AkGameObjEnvironmentData
{
	/// Contains all active environments sorted by priority, even those inside a portal.
	private List<AkEnvironment> activeEnvironmentsFromPortals = new List<AkEnvironment>();

	/// Contains all active environments sorted by default, excludeOthers and priority, even those inside a portal.
	private List<AkEnvironment> activeEnvironments = new List<AkEnvironment>();

	/// Contains all active portals.
	private List<AkEnvironmentPortal> activePortals = new List<AkEnvironmentPortal>();

	private AkAuxSendArray auxSendValues = new AkAuxSendArray();
	private bool isDirty = true;

	private void AddHighestPriorityEnvironmentsFromPortals(Vector3 position)
	{
		for (int i = 0; i < activePortals.Count; i++)
		{
			for (int j = 0; j < AkEnvironmentPortal.MAX_ENVIRONMENTS_PER_PORTAL; j++)
			{
				AkEnvironment env = activePortals[i].environments[j];
				if (env != null)
				{
					int index = activeEnvironmentsFromPortals.BinarySearch(env, AkEnvironment.s_compareByPriority);
					if (index >= 0 && index < AkEnvironment.MAX_NB_ENVIRONMENTS)
					{
						auxSendValues.Add(env.GetAuxBusID(), activePortals[i].GetAuxSendValueForPosition(position, j));
						if (auxSendValues.isFull)
							return;
					}
				}
			}
		}
	}

	private void AddHighestPriorityEnvironments(Vector3 position)
	{
		if (!auxSendValues.isFull && auxSendValues.Count() < activeEnvironments.Count)
		{
			for (int i = 0; i < activeEnvironments.Count; i++)
			{
				AkEnvironment env = activeEnvironments[i];
				uint auxBusID = env.GetAuxBusID();

				if ((!env.isDefault || i == 0) && !auxSendValues.Contains(auxBusID))
				{
					auxSendValues.Add(auxBusID, env.GetAuxSendValueForPosition(position));

					//No other environment can be added after an environment with the excludeOthers flag set to true
					if (env.excludeOthers || auxSendValues.isFull)
						break;
				}
			}
		}
	}

	public void UpdateAuxSend(GameObject gameObject, Vector3 position)
	{
		if (!isDirty)
			return;

        auxSendValues.Reset();
        AddHighestPriorityEnvironmentsFromPortals(position);
		AddHighestPriorityEnvironments(position);

		AkSoundEngine.SetGameObjectAuxSendValues(gameObject, auxSendValues, (uint)auxSendValues.Count());
		isDirty = false;
	}

	private void TryAddEnvironment(AkEnvironment env)
	{
		if (env != null)
		{
			int index = activeEnvironmentsFromPortals.BinarySearch(env, AkEnvironment.s_compareByPriority);
			if (index < 0)
			{
				activeEnvironmentsFromPortals.Insert(~index, env);

				index = activeEnvironments.BinarySearch(env, AkEnvironment.s_compareBySelectionAlgorithm);
				if (index < 0)
					activeEnvironments.Insert(~index, env);

				isDirty = true;
			}
		}
	}

	private void RemoveEnvironment(AkEnvironment env)
	{
		activeEnvironmentsFromPortals.Remove(env);
		activeEnvironments.Remove(env);
		isDirty = true;
	}

	public void AddAkEnvironment(Collider environmentCollider, Collider gameObjectCollider)
	{
		AkEnvironmentPortal portal = environmentCollider.GetComponent<AkEnvironmentPortal>();
		if (portal != null)
		{
			activePortals.Add(portal);

			for (int i = 0; i < AkEnvironmentPortal.MAX_ENVIRONMENTS_PER_PORTAL; i++)
				TryAddEnvironment(portal.environments[i]);
		}
		else
		{
			AkEnvironment env = environmentCollider.GetComponent<AkEnvironment>();
			TryAddEnvironment(env);
		}
	}

	private bool AkEnvironmentBelongsToActivePortals(AkEnvironment env)
	{
		for (int i = 0; i < activePortals.Count; i++)
			for (int j = 0; j < AkEnvironmentPortal.MAX_ENVIRONMENTS_PER_PORTAL; j++)
				if (env == activePortals[i].environments[j])
					return true;

		return false;
	}

	public void RemoveAkEnvironment(Collider environmentCollider, Collider gameObjectCollider)
	{
		AkEnvironmentPortal portal = environmentCollider.GetComponent<AkEnvironmentPortal>();
		if (portal != null)
		{
			for (int i = 0; i < AkEnvironmentPortal.MAX_ENVIRONMENTS_PER_PORTAL; i++)
			{
				AkEnvironment env = portal.environments[i];
				if (env != null && !gameObjectCollider.bounds.Intersects(env.GetCollider().bounds))
					RemoveEnvironment(env);
			}

			activePortals.Remove(portal);
			isDirty = true;
		}
		else
		{
			AkEnvironment env = environmentCollider.GetComponent<AkEnvironment>();
			if (env != null && !AkEnvironmentBelongsToActivePortals(env))
				RemoveEnvironment(env);
		}
	}
}

#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.