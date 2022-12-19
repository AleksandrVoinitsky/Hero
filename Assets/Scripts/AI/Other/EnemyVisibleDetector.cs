using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibleDetector : MonoBehaviour
{
    Enemy enemy;
    private void Start()
    {
      enemy =  transform.parent.GetComponent<Enemy>();
    }
    private void OnBecameVisible()
    {
        enemy.visible = true;
    }

    private void OnBecameInvisible()
    {
        enemy.visible = false;
    }
}
