using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TeleportScene : MonoBehaviour
{
    [SerializeField] GameObject CameraConteiner;
    [SerializeField] Transform TopObject;
    [SerializeField] Transform[] Points;
    [SerializeField] Transform[] Place;
    [SerializeField] RectTransform[] MapPoints;
    [SerializeField] CanvasGroup group;
    [SerializeField] CanvasGroup teleportGroup;
    [SerializeField] CanvasGroup WhiteImageGroup;
    [SerializeField] GameObject orbitalParticle;
    [SerializeField] int PointNumer;
    [SerializeField] bool start;
    [SerializeField] string[] SceneNames;
    
    

    void Start()
    {
        teleportGroup.alpha = 0;
        group.alpha = 0;
        WhiteImageGroup.alpha = 0;
        group.interactable = false;
        group.gameObject.SetActive(true);
        teleportGroup.interactable = false;
        teleportGroup.gameObject.SetActive(false);
        CameraConteiner.transform.DOMove(TopObject.position, 2.5f);
        CameraConteiner.transform.DORotate(TopObject.rotation.eulerAngles, 2.5f).OnComplete(() =>
        {
            for (int i = 0; i < MapPoints.Length; i++)
            {
                MapPoints[i].transform.position = Camera.main.WorldToScreenPoint(Place[i].position);
            }
            group.DOFade(1, 0.25f).OnComplete(() => group.interactable = true );
        });
       
    }

    void Update()
    {
    }

    public void CameraDown(int number)
    {
        PointNumer = number;
        MoveCamera(Points[PointNumer].position, Points[PointNumer].rotation.eulerAngles);
        group.DOFade(0, 0.25f).OnComplete(() =>
        {
            group.interactable = false;
            group.gameObject.SetActive(false);
            teleportGroup.gameObject.SetActive(true);
            teleportGroup.DOFade(1, 0.25f).OnComplete(() => teleportGroup.interactable = true);
        });
        
    }

    public void Back()
    {
        teleportGroup.DOFade(0, 0.25f).OnComplete(()=> {
            teleportGroup.interactable = false;
            teleportGroup.gameObject.SetActive(false);
            group.gameObject.SetActive(true);
        });
        CameraConteiner.transform.DOMove(TopObject.position, 1.5f);
        CameraConteiner.transform.DORotate(TopObject.rotation.eulerAngles, 1.5f).OnComplete(() =>
        {
            for (int i = 0; i < MapPoints.Length; i++)
            {
                MapPoints[i].transform.position = Camera.main.WorldToScreenPoint(Place[i].position);
            }
            group.DOFade(1, 0.25f).OnComplete(() => group.interactable = true);
        });
    }

    public void StartTeleport()
    {
        Instantiate(orbitalParticle, Place[PointNumer].transform.position, orbitalParticle.transform.rotation);
        CameraConteiner.transform.DOMove(Place[PointNumer].transform.position, 6f);
        teleportGroup.interactable = false;
        teleportGroup.DOFade(0, 0.25f).OnComplete(() => {
            teleportGroup.gameObject.SetActive(false);
        });

        WhiteImageGroup.DOFade(0, 3f).OnComplete(() => 
        {
            WhiteImageGroup.DOFade(1, 1f).OnComplete(() => {

                SceneManager.LoadScene(SceneNames[PointNumer]);
                Debug.Log("");
            });
        });
        
    }

    public void MoveCamera(Vector3 pos, Vector3 rot)
    {
        CameraConteiner.transform.DOMove(pos, 1.5f);
        CameraConteiner.transform.DORotate(rot, 1.5f).OnComplete(() => {
            
        });
    }
}
