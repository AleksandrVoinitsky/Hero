using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    [SerializeField] string CollisionTag = "Player";
    [SerializeField]  GameObject DestroyedObject;

    private void Awake()
    {
        DestroyedObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == CollisionTag)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SlowMo();
            DestroyedObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

   
}
