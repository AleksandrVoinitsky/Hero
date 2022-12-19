using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class Enemy : Character
{
    [SerializeField] GameObject TriagleImage;
    [SerializeField] GameObject AoePartikle;
    [SerializeField] GameObject AttackMarker;
    
    public bool aggressive = false;
    public bool visible = false;
    [SerializeField] float hp = 100;
    [SerializeField]float maxHp = 100;


    public float time;
    public float timer;
    public bool AttackFlag;


    public MaterialPropertyBlock props;
    public Renderer[] renderers;

    [SerializeField] GameObject Bullet;
    public override void Awake()
    {
        base.Awake();
        
        _action = ChangeAction(ActionType.Idle);
        _action.StartAction();

        hpImage.rectTransform.sizeDelta = new Vector2(1, 0.06f);
        hpImage2.rectTransform.sizeDelta = new Vector2(1, 0.06f);
        hpImage3.rectTransform.sizeDelta = new Vector2(1, 0.06f);
    }

    private void Start()
    {
        TriagleImage.SetActive(false);
        props = new MaterialPropertyBlock();
    }

    public override void Hit(int damage, AttackType type = AttackType.Base)
    {
        StartCoroutine(HitShaderEffect());

       if (dead) return;
       if (Random.Range(0,100) > 50)
       {
            Action = ChangeAction("Hit1");
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
       }
       else
       {
            Action = ChangeAction("Hit2");
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
       }

        if( type == AttackType.Base)
        {
            GameObject p = Instantiate(NumbersPrefab, transform.position + new Vector3(Random.Range(-1, 1), 1, 0), new Quaternion(0, 0, 0, 0));
            p.GetComponentInChildren<DamageNumbers>().StartNumber(damage.ToString(),AttackType.Base);
            hp -= damage;
        }
        else if (type == AttackType.Miss)
        {
            GameObject p = Instantiate(NumbersPrefab, transform.position + new Vector3(Random.Range(-1, 1), 1, 0), new Quaternion(0, 0, 0, 0));
            p.GetComponentInChildren<DamageNumbers>().StartNumber("Missed", AttackType.Miss);
        }
        else if(type == AttackType.Critical)
        {
            GameObject p = Instantiate(NumbersPrefab, transform.position + new Vector3(Random.Range(-1, 1), 1, 0), new Quaternion(0, 0, 0, 0));
            p.GetComponentInChildren<DamageNumbers>().StartNumber((damage * 2).ToString(), AttackType.Critical);
            hp -= damage *2;
        }

        if( hp <= damage)
        {
            if(Random.Range(0,100) < 30)
            {
                GameObject.FindObjectOfType<Player>().SlowMo();
            }
        }
        hpImage.fillAmount = SetHp(hp, maxHp);
        hpImage2.DOFillAmount(SetHp(hp, maxHp), 0.8f);

        if(hp <= 0)
        {
            TriagleImage.SetActive(false);
            Action = ChangeAction("Dead");
            dead = true;
            hpImage.DOFade(0, 0.5f);
            hpImage2.DOFade(0, 0.5f);
            hpImage3.DOFade(0, 0.5f);
            Invoke("TargetDeathNow", 0.5f);
        }
    }

    void TargetDeathNow()
    {
        GetComponent<Collider>().isTrigger = true;
        anyCharacter.TargetDeath();
    }

    public override void Targeted()
    {
        TriagleImage.SetActive(true);
        GetComponent<Collider>().isTrigger = false;
        base.Targeted();
        hpImage.rectTransform.sizeDelta = new Vector2(1, 0.16f);
        hpImage2.rectTransform.sizeDelta = new Vector2(1, 0.16f);
        hpImage3.rectTransform.sizeDelta = new Vector2(1, 0.16f);
        aggressive = true;
    }


    private void FixedUpdate()
    {
        if (dead) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            AttackFlag = true;
        }


        if (Action.state == ActionState.End)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > StopDistance)
            {
                if (!aggressive) return;
                Action = ChangeAction(ActionType.Move);
            }
            else
            {
                if (AttackFlag)
                {
                    Action = ChangeAction(ActionType.Idle);

                    AttackFlag = false;
                    timer = time;
                    Action = ChangeAction(ActionType.Attack);
                }
                else
                {
                    Action = ChangeAction(ActionType.Idle);
                }
                    
            }
        }
        _action.UpdateAction();
    }

    public override void SetCharacterEvent(string eventName)
    {
        if(eventName == "Attack")
        {
            Collider[] colliders = Physics.OverlapSphere(Firepoint.transform.position, 1f);
            foreach (var item in colliders)
            {
                if(item.gameObject == target)
                {
                    anyCharacter.Hit(Random.Range(5,10),AttackType.Base);
                }
            }
        }

        if(eventName == "RangeAttack")
        {
            Instantiate(Bullet, Firepoint.transform.position, Firepoint.transform.rotation);
        }

        if (eventName == "AoeAttack")
        {
            Instantiate(AoePartikle, Firepoint.transform.position, AoePartikle.transform.rotation);
            Collider[] colliders = Physics.OverlapSphere(Firepoint.transform.position, 4f);
            foreach (var item in colliders)
            {
                if (item.gameObject == target)
                {
                    anyCharacter.Hit(Random.Range(5, 10), AttackType.Base);
                }
            }
        }
    }

    IEnumerator HitShaderEffect()
    {
        props.SetFloat("_Hit", 1f);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(props);
        }
        yield return new WaitForSeconds(0.15f);
        props.SetFloat("_Hit", 0f);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(props);
        }

    }
}
