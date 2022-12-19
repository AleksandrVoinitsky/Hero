using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainEngine
{
    public class SceneComponent : MonoBehaviour
    {
       protected Main main;

        public virtual void InitComponent(Main mainClass)
        {
            main = mainClass;
        }

        public virtual void MainUpdate()
        {

        }

        public virtual void MainFixedUpdate()
        {

        }

        public virtual void MainLateUpdate()
        {

        }
    }
}
