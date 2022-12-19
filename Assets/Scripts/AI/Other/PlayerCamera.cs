using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Character Player;
    [SerializeField] Transform PosTarget;
    [SerializeField] GameObject cam;
    [SerializeField] Vector3 camPos1;
    [SerializeField] Vector3 camPos2;
    [SerializeField] float t;
    [SerializeField][Range(0,1)] float a;
    private Vector3 dir;
    private Quaternion lookRotation;
    public float RotationSpeed = 10;
    public float MoveSpeed = 10;
    Vector3 rotation;

    void Update()
    {
        t = (Player.transform.position.magnitude - Player.target.transform.position.magnitude) / 10;
        a = Mathf.Clamp(Mathf.Lerp(a, t, Time.deltaTime / 10), 0, 1);
        cam.transform.localPosition = Vector3.Lerp(camPos1, camPos2,1 - t);
        dir = (Player.target.transform.position - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(dir);
        rotation = new Vector3(0, Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed).eulerAngles.y, 0);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(rotation);
        transform.position = Vector3.Lerp(transform.position, PosTarget.position,Time.deltaTime * MoveSpeed);
    }

    private void FixedUpdate()
    {
        
        
    }

    public void CameraShake( float f1, float f2)
    {
        transform.DOShakePosition(f1,f2);
    }
}
