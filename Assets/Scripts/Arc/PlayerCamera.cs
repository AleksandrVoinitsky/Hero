using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainEngine 
{ 
    public class PlayerCamera : SceneComponent
    {
        float t; // bug
        float a;// bug
        Vector3 dir;
        Quaternion lookRotation;
        Vector3 rotation;

        public override void InitComponent(Main mainClass)
        {
            main = mainClass;
            main.AddMainUpdate(this);
            main.AddMainLateUpdate(this);
        }

        public override void MainUpdate()
        {
            t = (main.PlayerRootGameObject.transform.position.magnitude - main.PlayerTarget.transform.position.magnitude) / 10;
            a = Mathf.Clamp(Mathf.Lerp(a, t, Time.deltaTime / 10), 0, 1);
            main.Camera.transform.localPosition = Vector3.Lerp(main.camPos1, main.camPos2, 1 - t);
            dir = (main.PlayerTarget.transform.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(dir);
            rotation = new Vector3(0, Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * main.RotationSpeed).eulerAngles.y, 0);
        }

        public override void MainLateUpdate()
        {
            transform.rotation = Quaternion.Euler(rotation);
            transform.position = Vector3.Lerp(transform.position, main.PlayerRootGameObject.transform.position, Time.deltaTime * main.MoveSpeed);
        }
    }
}
