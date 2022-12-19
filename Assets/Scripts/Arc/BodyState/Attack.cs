using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainEngine
{
    public class Attack : Action
    {
        [SerializeField] float rotationSpeed;
        [SerializeField] float StopDistance;
        public float MoveTime;
        public float moveTimer;
        Quaternion _lookRotation;
        Vector3 moveVector;
        public override void StartAction()
        {
            state = ActionState.Being;
            moveTimer = MoveTime;
            _character.animator.SetBool(StateName, true);
        }

        public override void UpdateAction()
        {
            _character.body.velocity = new Vector3(0, 0, 0);
            if (moveTimer > 0)
            {

            }
            else
            {
                EndAction();
            }

            moveTimer -= Time.deltaTime;
            _character.LookDirection = (_character.target.transform.position - _character.body.position).normalized;
            _lookRotation = Quaternion.LookRotation(_character.LookDirection);
            Vector3 rotation = new Vector3(0, Quaternion.Slerp(_character.body.rotation, _lookRotation, Time.deltaTime * rotationSpeed).eulerAngles.y, 0);
            _character.body.MoveRotation(Quaternion.Euler(rotation));

            if (!_character.animator.GetCurrentAnimatorStateInfo(0).IsName(StateName))
            {
                // EndAction();
            }

        }

        public override void EndAction()
        {
            _character.animator.SetBool(StateName, false);
            state = ActionState.End;
        }
    }
}