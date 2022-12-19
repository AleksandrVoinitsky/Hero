using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTaran : Action
{
    public float time;
    public float timer;
    public GameObject AttackCollider;
    public GameObject Marker;
    GameObject marker;
    bool AttackComplete = false;
    bool taran = false;

    public override void StartAction()
    {
        _character.agent.isStopped = true;
        AttackComplete = false;
        state = ActionState.Cast;
        timer = time;
        marker = Instantiate(Marker, _character.Firepoint.transform.position, _character.Firepoint.transform.rotation);
        AttackCollider.SetActive(false);
    }

    public override void UpdateAction()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= time / 2)
            {
                if (!AttackComplete)
                {
                    AttackComplete = true;
                    _character.animator.SetTrigger(StateName);
                    _character.agent.enabled = false;
                }
                else
                {
                    AttackCollider.SetActive(true);
                    _character.body.isKinematic = false;
                    if (!taran)
                    {
                        taran = true;
                       
                    }
                    _character.body.AddForce(_character.transform.forward * 400);
                }
            }
        }
        else
        {
            AttackCollider.SetActive(false);
            EndAction();
        }
    }

    public override void EndAction()
    {
        AttackCollider.SetActive(false);
        _character.body.isKinematic = true;
        _character.agent.enabled = true;
        Destroy(marker);
        state = ActionState.End;
    }
}
