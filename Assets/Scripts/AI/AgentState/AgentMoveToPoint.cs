using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMoveToPoint : Action
{
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;

    float distance;

    public float time;
    public float timer;

    public bool startMove;
    public override void StartAction()
    {
        _character.agent.isStopped = false;
        state = ActionState.Cast;
        _character.MovePoint = new Vector3(_character.target.transform.position.x, _character.transform.position.y, _character.target.transform.position.z);
        timer = time;
        _character.animator.SetBool(StateName, true);
    }

    public override void UpdateAction()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (state == ActionState.Cast)
            {
                _character.agent.isStopped = false;
                state = ActionState.Being;
                _character.MovePoint = new Vector3(_character.target.transform.position.x, _character.transform.position.y, _character.target.transform.position.z);
                _character.agent.SetDestination(_character.target.transform.position);
            }
        }

        distance = Vector3.Distance(_character.transform.position, _character.MovePoint);
        if (distance <= _character.StopDistance)
        {
            EndAction();
        }
    }

    public override void EndAction()
    {
        _character.agent.isStopped = true;
        _character.animator.SetBool(StateName, false);
        state = ActionState.End;
    }
}
