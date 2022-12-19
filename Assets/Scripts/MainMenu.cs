using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject CameraConteiner;
    [Space(10)]
    [SerializeField] Vector3 cameraPos1;
    [SerializeField] Vector3 cameraRot1;
    [Space(10)]
    [SerializeField] Vector3 cameraPos2;
    [SerializeField] Vector3 cameraRot2;
    [Space(10)]
    [SerializeField] Vector3 cameraPos3;
    [SerializeField] Vector3 cameraRot3;

    void Start()
    {
        MoveCamera(cameraPos3, cameraRot3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCamera(Vector3 pos,Vector3 rot)
    {
        CameraConteiner.transform.DOMove(pos, 3.5f);
        CameraConteiner.transform.DORotate(rot, 3.6f);
    }
}
