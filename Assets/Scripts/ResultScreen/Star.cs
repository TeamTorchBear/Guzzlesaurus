using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    [SerializeField]
    private Results results;

    public void OnStarPop(){
        results.PopStar();
    }
}
