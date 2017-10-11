using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public abstract bool OnInteractionStart(Vector3 position);
    public abstract void OnInteractionHold(Vector3 position);
    public abstract void OnInteractionEnd(Vector3 position);
}
