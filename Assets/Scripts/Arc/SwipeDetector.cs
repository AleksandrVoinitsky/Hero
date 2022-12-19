using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainEngine
{
    public class SwipeDetector : SceneComponent
    {
        public delegate void OnSwipeInput(Vector2 direction);
        public static event OnSwipeInput SwipeEvent;

        public delegate void OnTapInput();
        public static event OnTapInput TapEvent;

        Vector2 TapPosition;
        Vector2 SwipeDelta;
        bool isSwiping;
        bool swipe = false;

        public override void InitComponent(Main mainClass)
        {
            base.InitComponent(mainClass);
            main.AddMainUpdate(this);
        }

        public override void MainUpdate()
        {
            base.MainUpdate();
            if (!main.isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    swipe = false;
                    isSwiping = true;
                    TapPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    CheckTap();
                    ResetSwipe();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        isSwiping = true;
                        TapPosition = Input.GetTouch(0).position;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        ResetSwipe();
                    }
                }
            }
            CheckSwipe();
        }

        void CheckTap()
        {
            SwipeDelta = Vector2.zero;
            if (!swipe && SwipeDelta.magnitude < main.deadZone)
            {
                TapEvent();
            }
        }

        void CheckSwipe()
        {
            SwipeDelta = Vector2.zero;

            if (isSwiping)
            {
                if (!main.isMobile && Input.GetMouseButton(0))
                {
                    SwipeDelta = (Vector2)Input.mousePosition - TapPosition;
                }
                else if (Input.touchCount > 0)
                {
                    SwipeDelta = Input.GetTouch(0).position - TapPosition;
                }
            }

            if (SwipeEvent != null){}

            if (SwipeEvent != null)
            {
                if (SwipeDelta.magnitude > main.deadZone)
                {
                    swipe = true;
                    if (Mathf.Abs(SwipeDelta.x) > Mathf.Abs(SwipeDelta.y))
                    {
                        SwipeEvent?.Invoke(SwipeDelta.x > 0 ? Vector2.right : Vector2.left);
                    }
                    else
                    {
                        SwipeEvent?.Invoke(SwipeDelta.y > 0 ? Vector2.up : Vector2.down);
                    }
                    ResetSwipe();
                }
            }
        }

        void ResetSwipe()
        {
            isSwiping = false;
            TapPosition = Vector2.zero;
            SwipeDelta = Vector2.zero;
        }
    }
}
