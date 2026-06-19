using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static float horizontalAxisRaw;
    public static Vector2[] fingerPoses = new Vector2[10];

    public static bool movingLeft;
    public static bool movingRight;

    public static int mobileMoveSystem;

    public static float swipeStartX;

    public static int leftTouchID = -1;
    public static Vector2 leftTouchPos;

    public static int rightTouchID = -1;
    public static Vector2 rightTouchPos;
    public static float GetHorizontalAxisRaw()
    {
        if (Application.isMobilePlatform)//|| UnityEditor.EditorApplication.isRemoteConnected)
        {
            if (mobileMoveSystem == 0)
            {
                if (Input.touchCount > 0)
                {
                    /* Old code
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Began)
                        {
                            fingerPoses[i] = touch.position;
                            if (fingerPoses[i].x > Screen.width - (Screen.width / 2)) { movingRight = true; }
                            if (fingerPoses[i].x < (Screen.width / 2)) { movingLeft = true; }
                        }
                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            if (fingerPoses[i].x > Screen.width - (Screen.width / 2)) { movingRight = false; }
                            if (fingerPoses[i].x < (Screen.width / 2)) { movingLeft = false; }
                            fingerPoses[i] = new Vector2(0, 0);
                        }
                    }
                    */
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);

                        if (touch.phase == TouchPhase.Began)
                        {
                            if (touch.position.x > Screen.width - (Screen.width / 2))
                            {
                                // This touch is for moving right
                                rightTouchID = touch.fingerId;
                                rightTouchPos = touch.position;
                            }
                            else if (touch.position.x < (Screen.width / 2))
                            {
                                // This touch is for moving left
                                leftTouchID = touch.fingerId;
                                leftTouchPos = touch.position;
                            }
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            if (touch.fingerId == rightTouchID)
                            {
                                rightTouchID = -1;
                            }
                            else if (touch.fingerId == leftTouchID)
                            {
                                leftTouchID = -1;
                            }
                        }
                    }
                    movingRight = rightTouchID != -1;
                    movingLeft = leftTouchID != -1;
                }
                else
                {
                    movingLeft = false;
                    movingRight = false;
                }
            }
            else if (mobileMoveSystem == 1)
            {
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Began){
                            swipeStartX = touch.position.x;
                            fingerPoses[i] = touch.position;
                        }
                        else if (touch.phase == TouchPhase.Moved){
                            movingRight = touch.position.x > swipeStartX;
                            movingLeft = touch.position.x < swipeStartX;
                            fingerPoses[i] = touch.position;
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled){
                            movingLeft = false; movingRight = false;
                            fingerPoses[i] = new Vector2(0,0);
                        }

                        /* Old Code
                        if (touch.phase == TouchPhase.Moved)
                        {
                            if (touch.deltaPosition.x > 10) {movingRight = true; movingLeft = false; }
                            else if (touch.deltaPosition.x < -10) { movingRight = false; movingLeft = true; }

                            //if (movingRight && touch.deltaPosition.x < -35) { movingRight = false; }
                            //if (movingLeft && touch.deltaPosition.x > 35) { movingLeft = false; }
                        }
                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            movingLeft = false; movingRight = false;
                        }
                        */
                    }
                }
                else
                {
                    movingLeft = false;
                    movingRight = false;
                }
            }
            else if (mobileMoveSystem == 2)
            {
                if (Input.acceleration.x < -0.2f){
                    movingLeft = true;
                    movingRight = false;
                }
                else if (Input.acceleration.x > 0.2f){
                    movingLeft = false;
                    movingRight = true;
                }
                else{
                    movingLeft = false;
                    movingRight = false;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow)) { movingLeft = true; }
            if (Input.GetKey(KeyCode.RightArrow)) { movingRight = true; }
            if (movingRight && !movingLeft) horizontalAxisRaw = 1;
            else if (movingLeft && !movingRight) horizontalAxisRaw = -1;
            else horizontalAxisRaw = 0;
            /*
            if (movingRight && !movingLeft) horizontalAxisRaw = 1;
            else if (!movingRight && movingLeft) horizontalAxisRaw = -1;
            else if ((movingRight && movingLeft) || (!movingRight && !movingLeft)) horizontalAxisRaw = 0;
            else horizontalAxisRaw = 0;
            */
        }
        else
        {
            horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
        }
        return horizontalAxisRaw;
    }
    public static void CancleHorizontalAxisRawInput()
    {
        movingLeft = false;
        movingRight = false;
        rightTouchID = -1;
        leftTouchID = -1;
        horizontalAxisRaw = 0;
    }

}
