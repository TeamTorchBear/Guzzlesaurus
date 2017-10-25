using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction
{
    public string action;
    string PlayerAct(string act)
    {
        action = action + ',' + act;
        return action;
    }
}
