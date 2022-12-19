using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Character PalyerClass;
    [SerializeField] Player pl;
    [SerializeField] List<Character> characters;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] GameObject[] SpawnPoints;
    [SerializeField] GameObject[] Swords;
    [SerializeField] GameObject props;
    [SerializeField] GameObject props1;
    [SerializeField] GameObject props2;
    [SerializeField] GameObject props3;
    bool enableProps = false;
    bool enableProps1 = false;
    bool enableProps2 = false;
    bool enableProps3 = false;
   [SerializeField] public GameObject[] SwordsTrails;

    [SerializeField] GameObject Settings;
    [SerializeField] bool settingsActive;

    [SerializeField] Scrollbar SpeedMoveBar;
    [SerializeField] Move Move;
    [SerializeField] Move Move1;
    [SerializeField] Move Move2;
    [SerializeField] Move Move3;
    [SerializeField] Move Move4;
    [SerializeField] Move Move5;

    [SerializeField] Dash Dash1;
    [SerializeField] Scrollbar SpeedDash1Bar;
    [SerializeField] Dash Dash2;
    [SerializeField] Scrollbar SpeedDash2Bar;

    [SerializeField]
    Scrollbar MinDamageBar;

    [SerializeField]
    Scrollbar MaxDamageBar;

    [SerializeField] GameObject[] Arena1;
    [SerializeField] GameObject[] Arena2;


    private void Start()
    {
        if(characters.Count == 0)
        {
            CreateCharacters();
        }
        Invoke("Retarget", 0.5f);
    }

    private void FixedUpdate()
    {
        if (characters.Count == 0)
        {
            CreateCharacters();
        }
        else
        {
            foreach (var item in characters)
            {
                if(item.Action.StateName == "Dead")
                {
                    RemoveCharacter(item);
                    break;
                }
            }
        }
    }

    void RemoveCharacter(Character ch)
    {
        characters.Remove(ch);
        Destroy(ch.gameObject, 5);
    }

    void CreateCharacters()
    {
        for (int i = 0; i < Random.Range(1,8); i++)
        {
            CreateCharacter();
        }

        
    }

    void Retarget()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TargetDeath();
    }

    void CreateCharacter()
    {
       GameObject enemy = Instantiate(prefabs[Random.Range(0,prefabs.Length)], SpawnPoints[Random.Range(0,SpawnPoints.Length)].transform.position + new Vector3(Random.Range(0,5),0, Random.Range(0, 5)), SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.rotation);
        Enemy en = enemy.GetComponent<Enemy>();
        en.anyCharacter = PalyerClass;
        en.target = Player;
        en.aggressive = true;
        characters.Add(en);
        float rnd = Random.Range(0.8f, 1.2f);
        enemy.transform.localScale = new Vector3(rnd, rnd, rnd);
    }


    public void ShowSettings()
    {
        if (settingsActive)
        {
            Settings.SetActive(false);
            Time.timeScale = 1;
            settingsActive = false;
        }
        else
        {
            Settings.SetActive(true);
            Time.timeScale = 0;
            settingsActive = true;
        }
    }

    public void SwichSword(int number)
    {
        foreach (var item in Swords)
        {
            item.SetActive(false);
        }
        Swords[number].SetActive(true);
        Player p = FindObjectOfType<Player>();
        p.TrailPrefab = SwordsTrails[number];
    }

    public void SetMoveSpeed(string Action)
    {
        Player p = FindObjectOfType<Player>();
        float speed = 45000 * (SpeedMoveBar.value *2);
        Move.maxSpeed = speed;
        Move.acciliration = speed / 10;
        Move1.maxSpeed = speed;
        Move1.acciliration = speed / 10;
        Move2.maxSpeed = speed;
        Move2.acciliration = speed / 10;
        Move3.maxSpeed = speed;
        Move3.acciliration = speed / 10;
        Move4.maxSpeed = speed;
        Move4.acciliration = speed / 10;
        Move5.maxSpeed = speed;
        Move5.acciliration = speed / 10;
    }

    public void SetDash1Speed(string Action)
    {
        Player p = FindObjectOfType<Player>();
        float speed = 45000 * (SpeedDash1Bar.value * 2);
        Dash1.maxSpeed = speed;
        Dash1.acciliration = speed / 10;

    }

    public void SetDash2Speed(string Action)
    {
        Player p = FindObjectOfType<Player>();
        float speed = 45000 * (SpeedDash2Bar.value * 2);
        Dash2.maxSpeed = speed;
        Dash2.acciliration = speed / 10;
    }

    public void SetMiuDamage()
    {
        pl.minDamage = (int)(MinDamageBar.value * 100);
    }

    public void SetMaxDamage()
    {
        pl.maxDamage = (int)(MaxDamageBar.value * 100);
    }

    public void EnablrProps()
    {
        if (enableProps)
        {
            enableProps = false;
            props.SetActive(false);
        }
        else
        {
            enableProps = true;
            props.SetActive(true);
        }
    }

    public void EnablrProps1()
    {
        if (enableProps1)
        {
            enableProps1 = false;
            props1.SetActive(false);
        }
        else
        {
            enableProps1 = true;
            props1.SetActive(true);
        }
    }

    public void EnablrProps2()
    {
        if (enableProps2)
        {
            enableProps2 = false;
            props2.SetActive(false);
        }
        else
        {
            enableProps2 = true;
            props2.SetActive(true);
        }
    }

    public void EnablrProps3()
    {
        if (enableProps3)
        {
            enableProps3 = false;
            props2.SetActive(false);
        }
        else
        {
            enableProps3 = true;
            props3.SetActive(true);
        }
    }

    public void RndMove()
    {
        pl.rndMove = !pl.rndMove;
    }


    public void SwichArena(int num)
    {
        if ( num == 1)
        {
            foreach (var item in Arena1)
            {
                item.SetActive(true);
                
            }
            foreach (var item in Arena2)
            {
                item.SetActive(false);
            }
        }
        else
        {
            foreach (var item in Arena1)
            {
                item.SetActive(false);

            }
            foreach (var item in Arena2)
            {
                item.SetActive(true);
            }
        }
       
    }
}
