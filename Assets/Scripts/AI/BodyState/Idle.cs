using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Action
{
    //[SerializeField] Transform target;
    [SerializeField] float rotationSpeed;
    Quaternion _lookRotation;
    public override void StartAction()
    {
        _character.animator.SetBool(StateName, true);
    }

    public override void UpdateAction()
    {
        _character.body.velocity = new Vector3(0, 0, 0);
       // _character.LookDirection = (_character.target.transform.position - _character.body.position).normalized;
       // _lookRotation = Quaternion.LookRotation(_character.LookDirection);
       // Vector3 rotation = new Vector3(0, Quaternion.Slerp(_character.body.rotation, _lookRotation, Time.deltaTime * rotationSpeed).eulerAngles.y, 0);
       // _character.body.MoveRotation(Quaternion.Euler(rotation));
    }

    public override void EndAction()
    {
        _character.animator.SetBool(StateName, false);
    }
}
