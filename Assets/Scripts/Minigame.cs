using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guzzlesaurus/Minigame")]
public class Minigame : ScriptableObject {
    public string minigameName;
    public string answer;
    public string timer;
    public float timeToPromt = 1f;
    public float promptTime = 4f;

    public virtual void StartMinigame() { }
}
