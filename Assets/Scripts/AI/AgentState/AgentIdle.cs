using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentIdle : Action
{
    public float time;
    public float timer;
    public override void StartAction()
    {
        _character.agent.isStopped = true;
        state = ActionState.Cast;
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
                _character.LookDirection = (_character.target.transform.position - _character.transform.position).normalized;
                _character.LookRotation = Quaternion.LookRotation(_character.LookDirection);
                state = ActionState.Being;
            }

            _character.transform.rotation = Quaternion.RotateTowards(_character.transform.rotation, _character.LookRotation, _character.RotationSpeed);

            if(_character.transform.rotation == _character.LookRotation)
            {
                EndAction();
            }
        }
    }

    public override void EndAction()
    {
        state = ActionState.End;
         _character.animator.SetBool(StateName, false);
    }
}
