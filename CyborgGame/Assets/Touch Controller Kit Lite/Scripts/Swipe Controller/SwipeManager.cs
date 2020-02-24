using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    #region Fields

    //* Variables to be handeled by code */
    //bool events that triggers swipe and tap events
    private bool swipeUp, swipeRight, swipeDown, swipeLeft;
    private bool canTap = true, canDoubleTap = true;

    //Position and direction trackers
    private Vector2 startPositionOfTouch;
    private Vector2 endPositionOfTouch;
    private Vector2 swipeDirection;

    //Time of touch trackers
    private float startTimeOfTouch;
    private float endTimeOfTouch;
    private float totalDurationOftouch;

    //Length of swipe trackers
    private float initialLengthOfSwipe;
    private float finalLengthOfSwipe;

    //Tap trackers
    private float firstTapTime;
    private bool doubleTapInitialized;

    public delegate void TapHandler();
    public static event TapHandler OnSingleTap;
    public static event TapHandler OnDoubleTap;

    //Getters for bool event triggers
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeDown { get { return swipeDown; } }
    public bool SwipeLeft { get { return swipeLeft; } }

    //* Variables used only for its value as defined in the inspector */
    [Tooltip("Do you want to prevent clicks from passing through UI.")]
    [SerializeField] private bool preventClicksThroughUI = true;

    [Tooltip("Maximum time for recognizing a tap")]
    [SerializeField] [Range(0.1f, 1f)] private float maxtapTime = 0.5f;

    [Tooltip("Minimum length of swipe for recognizing it as an actual swipe")]
    [SerializeField] [Range(50f, 200f)] private float minSwipeLength = 100f;

    [Tooltip("Time between which consecutive taps will be considered a double or a triple tap")]
    [SerializeField] [Range(0.1f, 1f)] private float timeBetweenTaps = 0.5f;

    [Tooltip("This nullifies tap for specified seconds after being called once. Set this to zero if you want non-stop consecutive taps")]
    [SerializeField] [Range(0f, 5f)] private float tapNullifyTimeAfterTrigger = 1f;

    [Tooltip("This nullifies double tap for specified seconds after being called once. Set this to zero if you want non-stop consecutive taps")]
    [SerializeField] [Range(0f, 5f)] private float doubleTapNullifyTimeAfterTrigger = 1f;

    #endregion

    #region Unity Methods

    void Update()
    {
        //! Setting all bool triggers to false every frame to prevent them from being called several times
        swipeUp = swipeRight = swipeDown = swipeLeft = false;

        //This prevents clicks and touches through UI
        if ((EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        && preventClicksThroughUI)
        {
            return;
        }

        if (Input.touchSupported)
        {
            DetectAndTriggerSwipeForTouchInput();
        }
        else
        {
            DetectAndTriggerSwipeForMouseInput();
        }
    }

    #endregion

    #region Private Methods

    //Detect and trigger swipe events for touch inputs
    private void DetectAndTriggerSwipeForTouchInput()
    {
        //If there is a touch
        if (Input.touches.Length > 0)
        {
            //Get touch and store it into a touch variable
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //We store starting position and time of touch
                startPositionOfTouch = new Vector2(touch.position.x, touch.position.y);
                startTimeOfTouch = Time.time;
                ResetSwipeData();

            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                //We store next positon of touch when it is moved. We also store time of touch
                endPositionOfTouch = new Vector2(touch.position.x, touch.position.y);
                endTimeOfTouch = Time.time;
                UpdateSwipeData();

            }

            if (touch.phase == TouchPhase.Ended)
            {
                //When the touch is released we store final position and time of touch
                endPositionOfTouch = new Vector2(touch.position.x, touch.position.y);
                endTimeOfTouch = Time.time;

                UpdateSwipeData();

                TriggerSwipes();

                StartTapSequence();

            }

        }

    }

    private void DetectAndTriggerSwipeForMouseInput()
    {
        //Phase Began
        if (Input.GetMouseButtonDown(0))
        {
            //We store starting position and time of mouse click
            startPositionOfTouch = Input.mousePosition;
            startTimeOfTouch = Time.time;
            ResetSwipeData();

        }

        //Phase Moved
        if (Input.GetMouseButton(0))
        {
            //We store next positon of mouse cursor when it is moved. We also store time of click
            endPositionOfTouch = Input.mousePosition;
            endTimeOfTouch = Time.time;
            UpdateSwipeData();

        }

        //Phase Ended
        if (Input.GetMouseButtonUp(0))
        {
            //When the click is released we store final position and time of click
            endPositionOfTouch = Input.mousePosition;
            endTimeOfTouch = Time.time;

            UpdateSwipeData();

            TriggerSwipes();

            StartTapSequence();
        }
    }

    //We reset all swipe data values to zero for new swipe calculations to take place
    private void ResetSwipeData()
    {
        endPositionOfTouch = startPositionOfTouch;
        initialLengthOfSwipe = 0f;
        finalLengthOfSwipe = 0f;

        endTimeOfTouch = startTimeOfTouch;
        totalDurationOftouch = 0f;

    }

    //We update the swipe data values as per input
    private void UpdateSwipeData()
    {
        //Calculate total duration (final time - intial time)
        totalDurationOftouch = endTimeOfTouch - startTimeOfTouch;

        //Calculate swipe direcion by vector rule (direction = endpos - intial position)
        swipeDirection = endPositionOfTouch - startPositionOfTouch;

        //We calculate the distance of swipe
        initialLengthOfSwipe = Vector2.Distance(startPositionOfTouch, endPositionOfTouch);
        if (initialLengthOfSwipe > finalLengthOfSwipe)
        {
            finalLengthOfSwipe = initialLengthOfSwipe;
        }

    }

    //Triggers swipes in desired direction
    private void TriggerSwipes()
    {
        //Calculating angle of swipe w.r.t. the y direction
        float degreeOfSwipe = Vector2.Angle(Vector2.up, swipeDirection.normalized);

        if (finalLengthOfSwipe > minSwipeLength)
        {
            //Swipe for four directional swipe type
            //if x greater than 0 then it is a right sided swipe
            if (swipeDirection.x > 0)
            {
                //Right
                if (degreeOfSwipe < 45f) // 0 - 45 is swipe up
                {
                    swipeUp = true;
                }
                else if (degreeOfSwipe < 135f) // 45 - 135 is swipe right
                {
                    swipeRight = true;
                }
                else if (degreeOfSwipe < 180f) // 135 - 180 is swipe down
                {
                    swipeDown = true;
                }

            }
            else //else it is a right sided swipe since x is negative
            {
                //Left
                if (degreeOfSwipe < 45f) // 0 - 45 is swipe up
                {
                    swipeUp = true;
                }
                else if (degreeOfSwipe < 135f) // 45 - 135 is swipe left
                {
                    swipeLeft = true;
                }
                else if (degreeOfSwipe < 180f)  // 135 - 180 is swipe down
                {
                    swipeDown = true;
                }

            }
        }

    }

    //This starts the sequence which will trigger tap, double tap and triple tap
    private void StartTapSequence()
    {
        //Check if it was a genune tap
        if (totalDurationOftouch < maxtapTime && finalLengthOfSwipe < minSwipeLength)
        {
            //Trigger single tap in specified time
            Invoke("TriggerSingleTap", timeBetweenTaps);

            //Also check for double tap 
            if (!doubleTapInitialized)
            {
                doubleTapInitialized = true;
                firstTapTime = Time.time;
            }
            else if (Time.time - firstTapTime < timeBetweenTaps && doubleTapInitialized)
            {
                //if time of double tap is within the genuine range then cancel invoke of single tap and start invoke for double tap
                CancelInvoke("TriggerSingleTap");
                Invoke("TriggerDoubleTap", timeBetweenTaps);
            }

        }
    }

    //We trigger single tap
    private void TriggerSingleTap() //! Called by Function name with string reference
    {
        doubleTapInitialized = false;

        if (canTap)
        {
            //Calling delegate event
            if (OnSingleTap != null)
            {
                OnSingleTap.Invoke();
            }
        }
    }

    //We trigger double tap and set canTap and canDoubleTap to false so that neither of them triggers consecutively. We return those value back to true by using invoke.
    private void TriggerDoubleTap() //! Called by Function name with string reference
    {
        if (canDoubleTap)
        {
            //Calling delegate event
            if (OnDoubleTap != null)
            {
                OnDoubleTap.Invoke();
            }
        }

        canTap = false;
        canDoubleTap = false;
        Invoke("ReturnTapToTrue", tapNullifyTimeAfterTrigger);
        Invoke("ReturnDoubleTapToTrue", doubleTapNullifyTimeAfterTrigger);
        doubleTapInitialized = false;
    }


    private void ReturnTapToTrue() //! Called by Function name with string reference
    {
        canTap = true;
    }

    private void ReturnDoubleTapToTrue() //! Called by Function name with string reference
    {
        canDoubleTap = true;
    }

    #endregion
}
