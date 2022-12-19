using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    [SerializeField] Image DashImage;
    public void StartHit()
    {
        DashImage.DOFade(1f, 0.2f).OnComplete(() =>
        {
            DashImage.DOFade(0, 0.5f);
        });
    }
}
