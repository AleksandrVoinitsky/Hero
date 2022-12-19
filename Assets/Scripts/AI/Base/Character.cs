using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Rigidbody body;
    public NavMeshAgent agent;
    public Animator animator;
    [Space(10)]
    public Character anyCharacter;
    public GameObject target;
    public GameObject Firepoint;
    public GameObject NumbersPrefab;
    [Space(10)]
    public Vector3 MoveDirection;
    public Vector3 LookDirection;
    public Quaternion LookRotation;
    public Vector3 MovePoint;
    [Space(10)]
    public float MoveSpeed;
    public float Acciliration;
    public float RotationSpeed;
    [Space(10)]
    public float StopDistance;
    public float AttackDistance;
    public float RangeAttackDistance;
    [Space(10)]
    public Image hpImage;
    public Image hpImage2;
    public Image hpImage3;
    [Space(10)]
    public List<Action> IdleStateList;
    public List<Action> MoveStateList;
    public List<Action> AttackStateList;
    public List<Action> SpecialStateList;

    protected Action _action;


    public bool dead = false;
    public Action Action
    {
        get { return _action; }
        set
        {
            _action.EndAction();
            _action = value;
            Action.StartAction();
            OnActionChenged();
        }
    }

    public virtual void Awake()
    {
        foreach (var item in IdleStateList)
        {
            item.InitAction(this);
        }
        foreach (var item in MoveStateList)
        {
            item.InitAction(this);
        }
        foreach (var item in AttackStateList)
        {
            item.InitAction(this);
        }
        foreach (var item in SpecialStateList)
        {
            item.InitAction(this);
        }
    }

    public Action ChangeAction(ActionType type)
    {
        if (type == ActionType.Idle)
        {
            return (IdleStateList[Random.Range(0, IdleStateList.Count)]);
        }
        else if (type == ActionType.Move)
        {
            return (MoveStateList[Random.Range(0, MoveStateList.Count)]);
        }
        else if (type == ActionType.Attack)
        {
            return (AttackStateList[Random.Range(0, AttackStateList.Count)]);
        }
        else if (type == ActionType.Special)
        {
            return (SpecialStateList[Random.Range(0, SpecialStateList.Count)]);
        }
        else
        {
            return (IdleStateList[Random.Range(0, IdleStateList.Count)]);
        }
    }

    public Action ChangeAction(string name)
    {
        foreach (var item in SpecialStateList)
        {
            if (item.StateName == name)
            {
                return item;
            }
        }

        return (ChangeAction(ActionType.Idle));
    }

    public virtual void OnActionChenged()
    {

    }


    public virtual void SetCharacterEvent(string eventName)
    {

    }

    public virtual void Hit(int damage,AttackType type)
    {
        
    }

    public virtual void Targeted()
    {

    }

    public virtual void TargetDeath()
    {

    }

    public virtual float SetHp(float val,float maxVal)
    {
        if (val > maxVal)
        {
            maxVal = val;
        }

        float BarFill;
        if (val == 0)
        {
            BarFill = val;
        }
        else
        {
            BarFill = ((val / maxVal) * 100);
        }

        return BarFill / 100;
    }
}
public enum AttackType
{
    Critical,
    Base,
    Miss
}
