using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : Action
{
    public float time;
    public float timer;
    public GameObject Marker;
    GameObject marker;
    bool AttackComplete = false;
   // public GameObject FirepointAction;
    public override void StartAction()
    {
        _character.agent.isStopped = true;
        AttackComplete = false;
        state = ActionState.Cast;
        timer = time;
        marker = Instantiate(Marker, _character.Firepoint.transform.position,  _character.Firepoint.transform.rotation);
    }

    public override void UpdateAction()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= time / 2)
            {
                if (!AttackComplete)
                {
                    AttackComplete = true;
                    _character.animator.SetTrigger(StateName);
                }
            }
        }
        else
        {
            EndAction();
        }
    }

    public override void EndAction()
    {
        Destroy(marker);
        state = ActionState.End;
    }
}
