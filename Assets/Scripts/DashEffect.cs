using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DashEffect : MonoBehaviour
{
    [SerializeField] Image DashImage;
    bool flag = false;
    public void StartDash()
    {
        if (flag) return;
        flag = true;
        DashImage.rectTransform.localScale = new Vector3(1, 2, 1);
        DashImage.DOFade(0.2f, 0.5f).OnComplete(() => 
        {
            DashImage.DOFade(0, 0.5f);
            flag = false;
        });
        DashImage.rectTransform.DOScale(1.5f, 1f).OnComplete(() =>
        {
            DashImage.rectTransform.DOScale(1, 1f);
        });
    }
}
