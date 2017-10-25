using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Answer ")]
public class Minigame : ScriptableObject {
    public string minigameName;
    public string answer;
    public string timer;
}
