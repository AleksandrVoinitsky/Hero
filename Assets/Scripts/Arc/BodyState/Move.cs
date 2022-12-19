using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainEngine
{
    public class Move : Action
    {
        [SerializeField] public float maxSpeed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float StopDistance;
        public float acciliration;
        public float MoveTime;
        public float moveTimer;
        public float speed;
        Quaternion _lookRotation;
        Vector3 moveVector;
        public override void StartAction()
        {
            state = ActionState.Being;
            moveTimer = MoveTime;
            float distance = Vector3.Distance(_character.transform.position, _character.target.transform.position);
            _character.animator.SetBool(StateName, true);
        }

        public override void UpdateAction()
        {
            if (moveTimer > 0)
            {
                if (moveTimer > MoveTime / 3)
                {
                    float value = 0;
                    value += acciliration;
                    if (speed < maxSpeed - value) { speed += value; }
                }
                else
                {
                    float value = 0;
                    value += acciliration;
                    if (speed - value > 0) { speed -= value; }
                }

                moveTimer -= Time.deltaTime;

                var tarPos = _character.target.transform.position;
                var palyerPos = transform.position;
                var dirToTarget = tarPos - palyerPos;
                dirToTarget.y = 0;
                var lookAtTargetRot = Quaternion.LookRotation(dirToTarget);
                moveVector = lookAtTargetRot * _character.MoveDirection * speed * Time.deltaTime;
                _character.body.velocity = moveVector * Time.fixedDeltaTime * Time.timeScale;
            }
            else
            {
                speed = 0;
                _character.body.velocity = new Vector3(0, 0, 0);
                EndAction();
            }

            _character.LookDirection = (moveVector - _character.transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_character.LookDirection);
            Vector3 rotation = new Vector3(0, Quaternion.Slerp(_character.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed).eulerAngles.y, 0);
            _character.body.MoveRotation(Quaternion.Euler(rotation));
        }

        public override void EndAction()
        {
            _character.animator.SetBool(StateName, false);
            state = ActionState.End;
        }
    }
}