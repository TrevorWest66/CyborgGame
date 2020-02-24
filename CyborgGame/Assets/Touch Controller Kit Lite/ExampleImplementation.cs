
using UnityEngine;

public class ExampleImplementation : MonoBehaviour
{
    #region Unity Methods

    private Vector2 movementInput; //this will hold the input value for joystick or touchpad
    private Vector3 tiltInput; //This will hold the tilt value of the device

    JoystickMovement Joystick;
    [SerializeField] private TiltController TiltController;

    //Subscribe to delegate events in onenable
    void OnEnable()
    {
        //Swipe events
        SwipeEvents.OnSwipeUp += OnSwipeUp;
        SwipeEvents.OnSwipeDown += OnSwipeDown;
        SwipeEvents.OnSwipeLeft += OnSwipeLeft;
        SwipeEvents.OnSwipeRight += OnSwipeRight;

        //Tap events
        SwipeManager.OnSingleTap += OnSingleTap; //Single tap
        SwipeManager.OnDoubleTap += OnDoubleTap; //Double consecutive tap
    }

    //Unsubscribe to all events you have subscribed to in ondisable
    void OnDisable()
    {
        //! Always unsubscribe to events if you have subscribed to them
        //Swipe Events
        SwipeEvents.OnSwipeUp -= OnSwipeUp;
        SwipeEvents.OnSwipeDown -= OnSwipeDown;
        SwipeEvents.OnSwipeLeft -= OnSwipeLeft;
        SwipeEvents.OnSwipeRight -= OnSwipeRight;

        //Tap Events
        SwipeManager.OnSingleTap -= OnSingleTap;
        SwipeManager.OnDoubleTap -= OnDoubleTap;

    }

    void Update()
    {
        //We are getting the horizontal and vertical touch or joystick input
        movementInput.x = Joystick.HorizontalInput();
        movementInput.y = Joystick.VerticalInput();

        Debug.Log("Input Value: (" + movementInput.x + " , " + movementInput.y + ")"); //Logs the input value

        //Get the tilt vector
        tiltInput = TiltController.GetTilt();

        Debug.Log("Tilt Value: (" + tiltInput.x + " , " + tiltInput.y + "," + tiltInput.z + ")"); //Logs the tilt value
    }

    #endregion

    //All subscribed callback methods. We write our desired code inside these methods.
    #region CallBack Methods

    //Swipe Event Callbacks. These functions will get called when the respective event occurs.
    private void OnSwipeUp()
    {
        Debug.Log("SwipeUp");
    }

    private void OnSwipeDown()
    {
        Debug.Log("SwipeDown");
    }

    private void OnSwipeLeft()
    {
        Debug.Log("SwipeLeft");
    }

    private void OnSwipeRight()
    {
        Debug.Log("SwipeRight");
    }

    //Tap Event Callbacks
    private void OnSingleTap()
    {
        Debug.Log("SingleTap");
    }

    private void OnDoubleTap()
    {
        Debug.Log("DoubleTap");
    }

    #endregion
}
