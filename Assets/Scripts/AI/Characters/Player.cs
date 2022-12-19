using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public int maxDamage = 30;
    public int minDamage = 20;
    [SerializeField] ActionType Actiontype;
    [SerializeField] string Actionname;
    [SerializeField] PlayerCamera playercamera;
    [SerializeField] DashEffect dashEffect;
    [SerializeField] HitEffect hitEffect;
    [SerializeField] public GameObject TrailPrefab;
    [SerializeField] public GameObject TrailPoint;
    [SerializeField] public GameObject CurrentTrail;
    [SerializeField] GameObject HitEffect;
    [SerializeField] Transform Point;
    MaterialPropertyBlock props;
    Renderer[] renderers;
    public float hp = 100;
    public float maxXP = 100;
    public float time;
    public float timer;
    public bool AttackFlag;
    bool flag = true;
    int Serial = 0;
    public bool rndMove = false;

    public override void Awake()
    {
        base.Awake();
        SwipeDetection.SwipeEvent += Swipe;
        SwipeDetection.TapEvent += Tap;
        _action = ChangeAction(ActionType.Idle);
        _action.StartAction();
    }

    public override void OnActionChenged()
    {
        if(Action.actionType == ActionType.Attack)
        {
            CurrentTrail = Instantiate(TrailPrefab, TrailPoint.transform.position, TrailPoint.transform.rotation, TrailPoint.transform);
        }
        else
        {
            if(CurrentTrail != null)
            {
                CurrentTrail.transform.parent = null;
            }
        }
    }

    private void Start()
    {
        props = new MaterialPropertyBlock();
        renderers = transform.GetComponentsInChildren<Renderer>();

        Invoke("TargetDeath", 0.1f);
    }

    void Swipe(Vector2 swipe)
    {
        if (swipe == Vector2.up)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < StopDistance)
            {
                if (Action.actionType != ActionType.Attack)
                {
                    if (!anyCharacter.dead)
                        Action = ChangeAction("Attack2");
                }   
            }
            else
            {
                MoveDirection = new Vector3(0, 0, 1);
                Action = RandomMove();
            }
        }
        if (swipe == Vector2.down)
        {
            MoveDirection = new Vector3(0, 0, -1);
            Action = RandomMove();
        }
        if (swipe == Vector2.right)
        {
            MoveDirection = new Vector3(1, 0, 0);
            Action = RandomMove();
        }
        if (swipe == Vector2.left)
        {
            MoveDirection = new Vector3(-1, 0, 0);
            Action = RandomMove();
        }
    }

    Action RandomMove()
    {

        if (!rndMove)
        {
            return ChangeAction("Move3");
        }
        else
        {
            if (Random.Range(0, 100) < 50)
            {
                return ChangeAction("Move3");
            }
            else
            {
                return ChangeAction(ActionType.Move);
            }
        }

       
       
    }

    void Tap()
    {
        if (Vector3.Distance(transform.position,anyCharacter.transform.position) < StopDistance)
        {
            if (Action.actionType != ActionType.Attack)
            {
                if (!anyCharacter.dead)
                {
                    switch (Serial)
                    {
                        case 0:
                            Action = ChangeAction("Attack");
                            Serial = 1;
                            break;
                        case 1:
                            Action = ChangeAction("Attack3");
                            Serial = 0;
                            break;
                        default:
                            Serial = 0;
                            break;
                    }
                }
            }
        }
        else if (Vector3.Distance(transform.position, anyCharacter.transform.position) < 20 && Vector3.Distance(transform.position, target.transform.position) > 10)
        {
            if (Action.actionType != ActionType.Attack)
            {
                if (!anyCharacter.dead)
                {
                    dashEffect.StartDash();
                    MoveDirection = new Vector3(0, 0, 1);
                    Action = ChangeAction("Dash1");
                }
            }
        }
        else if (Vector3.Distance(transform.position, anyCharacter.transform.position) <= 10 && Vector3.Distance(transform.position, target.transform.position) > StopDistance)
        {
            if (Action.actionType != ActionType.Attack)
            {
                if (!anyCharacter.dead)
                {
                    dashEffect.StartDash();
                    MoveDirection = new Vector3(0, 0, 1);
                    Action = ChangeAction("Dash2");
                }
            }
        }
        else
        {
            dashEffect.StartDash();
            MoveDirection = new Vector3(0, 0, 1);
            Action = ChangeAction("Move4");
        }
    }

    void AttackMethod()
    {
        if (!anyCharacter.dead)
        {
            if(Vector3.Distance(transform.position, anyCharacter.transform.position) < StopDistance * 2)
            {
                Action = ChangeAction(ActionType.Attack);
            }
        }
    }

    private void FixedUpdate()
    {
        Actiontype = Action.actionType;
        Actionname = Action.name;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            AttackFlag = true;
        }

        _action.UpdateAction();
        if(Action.actionType != ActionType.Idle)
        {
            if(Action.state == ActionState.End)
            {
                 Action = ChangeAction(ActionType.Idle);
            }
        }
    }

    private void Update()
    {
        if (flag)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, anyCharacter.transform.position, 50 * Time.deltaTime);
            if (Vector3.Distance(target.transform.position, anyCharacter.transform.position) <= 0.2)
            {
                target.transform.parent = anyCharacter.transform;
                target.transform.localPosition = new Vector3(0, 0, 0);
                flag = false;
            }
        }
    }

    public override void SetCharacterEvent(string eventName)
    {
        base.SetCharacterEvent(eventName);
        if(eventName == "Attack")
        {
            if(Vector3.Distance(transform.position, target.transform.position) < StopDistance)
            {
                if (Random.Range(0, 100) > 80)
                {
                    anyCharacter.Hit(Random.Range(minDamage, maxDamage), AttackType.Critical);
                    playercamera.CameraShake(0.4f, 0.2f);
                }
                else
                {
                    anyCharacter.Hit(Random.Range(minDamage, maxDamage),AttackType.Base);
                    playercamera.CameraShake(0.2f, 0.2f);
                }
            }
        }
        else if(eventName == "Hit")
        {
            playercamera.CameraShake(2.8f, 0.1f);
        }
    }

    public override void TargetDeath()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        List<Enemy> VisibleEnemies = new List<Enemy>();
        Enemy AnyAnamy = null;
        float distance = 0;
        foreach (var item in enemies)
        {
            if (!item.dead)
            {
                if (item.visible)
                {
                     VisibleEnemies.Add(item);
                }
            }
        }
       // Debug.Log(VisibleEnemies.Count);
        if(VisibleEnemies.Count > 0)
        {
            AnyAnamy = VisibleEnemies[0];
            distance = Vector3.Distance(gameObject.transform.position, AnyAnamy.transform.position);
            if(VisibleEnemies.Count > 1)
            {
                for (int i = 1; i < VisibleEnemies.Count; i++)
                {
                    if (Vector3.Distance(gameObject.transform.position, VisibleEnemies[i].transform.position) < distance)
                    {
                        AnyAnamy = VisibleEnemies[i];
                        distance = Vector3.Distance(gameObject.transform.position, VisibleEnemies[i].transform.position);
                    }
                }
            }
        }
        else
        {
            foreach (var item in enemies)
            {
                if (!item.dead)
                {
                    AnyAnamy = item;
                    break;
                }
            }
        }
        flag = true;
        anyCharacter = AnyAnamy;
        AnyAnamy.Targeted();
    }


    public void SlowMo()
    {
        StartCoroutine(TimeScaleCorutine());
    }

    public override void Hit(int damage, AttackType type = AttackType.Base)
    {
       // base.Hit(damage.ToString());
        hp -= damage;
        Instantiate(HitEffect, Point.transform.position, Point.transform.rotation);
        Swipe(Vector2.down);
        hpImage.fillAmount = SetHp(hp, maxXP);
        playercamera.CameraShake(0.1f, 0.3f);
        hpImage2.DOFillAmount(SetHp(hp, maxXP), 0.8f);
        hitEffect.StartHit();
        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        GameObject p = Instantiate(NumbersPrefab, transform.position + new Vector3(Random.Range(-1, 1), 1, 0), new Quaternion(0, 0, 0, 0));
        p.GetComponentInChildren<DamageNumbers>().StartNumber(damage.ToString(), AttackType.Base);
        StartCoroutine(HitShaderEffect());
    }

    IEnumerator TimeScaleCorutine()
    {
        if (AttackFlag)
        {
            AttackFlag = false;
            timer = time;
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
    IEnumerator HitShaderEffect()
    {
        props.SetFloat("_Hit", 1f);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(props);
        }
        yield return new WaitForSeconds(0.3f);
        props.SetFloat("_Hit", 0f);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(props);
        }
    }
}
