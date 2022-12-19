using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public ActionType actionType;
    public string StateName;
    [Space(10)]
    public ActionState state;
    

    [HideInInspector]public Character _character;
    

    public virtual void InitAction(Character character)
    {
        _character = character;
    }

    public virtual void StartAction()
    {
        
        Debug.Log("Start " + StateName);
    }

    public virtual void UpdateAction()
    {
        Debug.Log("Update " + StateName);
    }

    public virtual void EndAction()
    {
        Debug.Log("End " + StateName);
    }
}

public enum ActionState
{
    Cast,
    Being,
    End
}

public enum ActionType
{
    Idle,
    Move,
    Attack,
    Special
}
