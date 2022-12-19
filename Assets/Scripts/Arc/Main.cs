using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainEngine
{
    public class Main : MonoBehaviour
    {
        [Header("_____[Main]_____")]
        [SerializeField] bool paused = false;
        [SerializeField]List<SceneComponent> InitComponents;

        List<SceneComponent> UpdateComponents;
        List<SceneComponent> FixedUpdateComponents;
        List<SceneComponent> LateUpdateComponents;
        

        [Header("_____[UserContolls/Ui]_____")]
        [SerializeField] bool enableUserSwipe;
        [SerializeField] bool enableUserTap;
        public bool isMobile = false;
        public float deadZone = 40;
        [SerializeField] SwipeDetector SwipeDetector;

        [Header("_____[Player]_____")]
        public GameObject PlayerRootGameObject;
        public GameObject PlayerTarget;
        public Player PlayerScript;

        public int maxDamage = 30;
        public int minDamage = 20;
        public float hp = 100;
        public float maxXP = 100;
        public float time;
        public float timer;
        public bool AttackFlag;
        public bool flag = true;
        public int Serial = 0;
        public bool rndMove = false;

        [Header("_____[PlayerCamera]_____")]
        public GameObject Camera;
        public Vector3 camPos1;
        public Vector3 camPos2;
        public float RotationSpeed = 10;
        public float MoveSpeed = 10;

        [Header("_____[Enemies]_____")]
        public GameObject EnemySpawner;

        [Header("_____[LevelContent]_____")]
        public GameObject LevelContent;

        [Header("_____[LevelEvents]_____")]
        public GameObject LevelEvents;

        [Header("_____[SceneManager]_____")]
        public GameObject SceneManagerClass;

        [Header("_____[Data]_____")]
        public Data data;

        #region Properties//==========================================
        public bool Paused
        {
            get { return paused; }
            set
            {
                if (value){PauseOn();}
                else{PauseOff();}
                paused = value;
            }
        }

        public bool EnableUserSwipe
        {
            get { return EnableUserSwipe; }
            set
            {
                if (value) {  }
                else {  }
                EnableUserSwipe = value;
            }
        }

        public bool EnableUserTap
        {
            get { return EnableUserTap; }
            set
            {
                if (value) { }
                else {}
                EnableUserTap = value;
            }
        }

        #endregion
        #region CyclesBlock//==========================================
        void Init()
        {
            UpdateComponents = new List<SceneComponent>();
            FixedUpdateComponents = new List<SceneComponent>();
            LateUpdateComponents = new List<SceneComponent>();

            if (InitComponents.Count <= 0) { return; }
            for (int i = 0; i < InitComponents.Count; i++)
            {
                InitComponents[i].InitComponent(this);
            }
            SwipeDetector.SwipeEvent += Swipe;
            SwipeDetector.TapEvent += Tap;
        }

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
           
        }

        void Update()
        {
            if (UpdateComponents.Count <= 0) { return; }
            for (int i = 0; i < UpdateComponents.Count; i++)
            {
                UpdateComponents[i].MainUpdate();
            }
        }

        void LateUpdate()
        {
            if (LateUpdateComponents.Count <= 0) { return; }
            for (int i = 0; i < LateUpdateComponents.Count; i++)
            {
                LateUpdateComponents[i].MainLateUpdate();
            }
        }

        void FixedUpdate()
        {
            if (FixedUpdateComponents.Count <= 0) { return; }
            for (int i = 0; i < FixedUpdateComponents.Count; i++)
            {
                FixedUpdateComponents[i].MainFixedUpdate();
            }
        }

        #endregion
        #region PublickMethods//==========================================
        public void AddMainUpdate(SceneComponent component)
        {
            UpdateComponents.Add(component);
        }

        public void AddMainFixedUpdate(SceneComponent component)
        {
            FixedUpdateComponents.Add(component);
        }

        public void AddMainLateUpdate(SceneComponent component)
        {
            LateUpdateComponents.Add(component);
        }

        public void SlowMo(float slow, float time)
        {
            float s = slow;
            float t = time;
            if (s <= 0) return;
            if (s > 1) s = 1;
            if (t <= 0) t = 0.1f;
            if (t >= 10) t = 10f;
            StartCoroutine(SlowMoCorutine(s, t));
        }
        #endregion
        #region PrivateMethods//==========================================

        void PauseOn()
        {
            Time.timeScale = 0;
        }

        void PauseOff()
        {
            Time.timeScale = 1;
        }

        void SlowMoOn(float slowMo)
        {
            //0.2f
            Time.timeScale = slowMo;
            Time.fixedDeltaTime = Time.timeScale * slowMo/100;
        }

        void SlowMoOff()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        IEnumerator SlowMoCorutine(float slow, float time)
        {
            SlowMoOn(slow);
            yield return new WaitForSeconds(time);
            SlowMoOff();
        }
        #endregion
        #region EventMethods//==========================================
        void Tap()
        {
            if (enableUserTap)
            {
                Debug.Log("tap");
                PlayerScript.Tap();
            }
        }

        void Swipe(Vector2 swipeVector)
        {
            if (enableUserSwipe)
            {
                Debug.Log("Swipe");
                PlayerScript.Swipe(swipeVector);
            }
        }
        #endregion
    }
}
