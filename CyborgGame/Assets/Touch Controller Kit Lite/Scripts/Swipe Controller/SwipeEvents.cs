using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEvents : MonoBehaviour
{
    #region Fields
    //Delegate
    public delegate void SwipeHandler();

    //Swipe Event Lists
    public static event SwipeHandler OnSwipeUp;
    public static event SwipeHandler OnSwipeRight;
    public static event SwipeHandler OnSwipeDown;
    public static event SwipeHandler OnSwipeLeft;

    //For Swipe
    private SwipeManager swipeManager;
    private bool canSwipe = true;

    #endregion

    #region Unity Methods

    void Awake()
    {
        swipeManager = GetComponent<SwipeManager>();
    }

    void Update()
    {
        PreventMultipleTriggersSimultaneously();
        SwipeInputs();
    }

    #endregion

    #region Private Methods

    //Checking and calling respective delegate events
    void SwipeInputs()
    {
        if (swipeManager.SwipeUp && canSwipe)
        {
            canSwipe = false;

            //Calling delegate event
            if (OnSwipeUp != null)
            {
                OnSwipeUp.Invoke();
            }
        }
        else if (swipeManager.SwipeRight && canSwipe)
        {
            canSwipe = false;

            //Calling delegate event
            if (OnSwipeRight != null)
            {
                OnSwipeRight.Invoke();
            }
        }
        else if (swipeManager.SwipeDown && canSwipe)
        {
            canSwipe = false;

            //Calling delegate event
            if (OnSwipeDown != null)
            {
                OnSwipeDown.Invoke();
            }
        }
        else if (swipeManager.SwipeLeft && canSwipe)
        {
            canSwipe = false;

            //Calling delegate event
            if (OnSwipeLeft != null)
            {
                OnSwipeLeft.Invoke();
            }
        }
    }

    //* Ensures that same swipe does not cause an event to be triggerd more than once */
    void PreventMultipleTriggersSimultaneously()
    {
        //For ensuring swipe only gets triggered once for mouse input
        #region Mouse Input Multiple Swipe Check

        if (Input.GetMouseButtonDown(0))
        {

        }
        else if (Input.GetMouseButtonUp(0))
        {
            canSwipe = true;
        }
        #endregion

        //For ensuring swipe only gets triggered once for touch input
        #region Mobile Input Multiple Swipe Check

        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
            }
            else if ((Input.touches[0].phase == TouchPhase.Ended) || (Input.touches[0].phase == TouchPhase.Canceled))
            {
                canSwipe = true;
            }
        }
        #endregion

    }

    #endregion
}
