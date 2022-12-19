using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHit : Action
{
    public AudioSource source;
    public GameObject Particle;
    public GameObject ParticlePlace;


    public float time;
    public float timer;
    public override void StartAction()
    {
        _character.agent.isStopped = true;
        state = ActionState.Cast;
        timer = time;
        source.pitch = Random.Range(0.8f, 1.3f);
        source.Play();
        _character.animator.SetTrigger(StateName);
        Instantiate(Particle, ParticlePlace.transform.position, ParticlePlace.transform.rotation);
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

            if (_character.transform.rotation == _character.LookRotation)
            {
                EndAction();
            }
        }
    }

    public override void EndAction()
    {
        state = ActionState.End;
       //_character.animator.SetBool(StateName, false);
    }
}
