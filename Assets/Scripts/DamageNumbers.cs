using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] CanvasGroup group;
    [SerializeField] GameObject CritImage;
    Transform camera;

    public void StartNumber(string num,AttackType type)
    {
        if(type == AttackType.Critical)
        {
            CritImage.SetActive(true);
           // Text.DOColor(Color.yellow, 0.2f);
        }


        camera = Camera.main.transform;
        Text.text = num;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), 2);
        group.DOFade(0, 2);
        Destroy(gameObject, 3);
    }

    void Update()
    {
        transform.LookAt(camera);
    }
}
