using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControllerGameManager : MonoBehaviour
{
    #region Fields

    [Header("General")]
    [SerializeField] private float eventTextResetTime = 5f; //time after which event text will reset 

    [Header("Script References")]
    [SerializeField] private VisualizeTouch VisualizeTouch;
    [SerializeField] private SwipeManager SwipeManager;
    [SerializeField] private JoystickMovement Joystick;
    [SerializeField] private TiltController TiltController;
    [SerializeField] private OrthoZoomAndPan OrthoZoomAndPan;

    [Header("UI References")]
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private Text hintText;
    [SerializeField] private Text eventText;
    [SerializeField] private Toggle animateTouchToggle;

    //*Variables handled by code only
    private Camera mainCamera;
    private bool tilting = false; //whether the tilt controller is on or not
    private bool triggerEventTextTimer = true;
    private float eventTextTimerStartTime;

    //Initial camera default values
    private float origCamOrthoSize;
    private float origCamPresFOV;
    private Vector3 origCamPos;
    private Quaternion origCamRotation;

    //Movement states to control joystick and touchpad
    private MovementStates MovementState = MovementStates.None; //None means neither the joystick nor the touchpad is being used
    private enum MovementStates
    {
        None,
        Joystick,
    }

    //Default text for hint and event text boxes
    private string hintDefaultText = "Select an input type from the drop down above.\n<Hints related  to input type will appear here>";
    private string eventDefaultText = "<Event that is triggered will be shown here>";

    #endregion

    #region Unity Methods

    void Awake()
    {
        //deactive all controllers in awake only to avoid refernce errors
        DeactivateAllControllers();
    }

    void Start()
    {
        mainCamera = Camera.main;
        origCamPresFOV = mainCamera.fieldOfView;
        origCamOrthoSize = mainCamera.orthographicSize;

        //Set this to active as it will always be in use
        VisualizeTouch.gameObject.SetActive(true);

        //Initialize according to intial toggle values
        InitializeTouchVisualization();

        origCamPos = mainCamera.transform.position;
        origCamRotation = mainCamera.transform.rotation;
    }

    //Subscribe to delegates here
    void OnEnable()
    {
        //Subscribing to events
        SwipeEvents.OnSwipeUp += OnSwipeUp;
        SwipeEvents.OnSwipeDown += OnSwipeDown;
        SwipeEvents.OnSwipeLeft += OnSwipeLeft;
        SwipeEvents.OnSwipeRight += OnSwipeRight;
        SwipeManager.OnSingleTap += OnSingleTap;
        SwipeManager.OnDoubleTap += OnDoubleTap;
    }

    //Unubscribe to delegates here
    void OnDisable()
    {
        //! Always unsubscribe to events if you have subscribed to them
        //Unsubscribing to events
        SwipeEvents.OnSwipeUp -= OnSwipeUp;
        SwipeEvents.OnSwipeDown -= OnSwipeDown;
        SwipeEvents.OnSwipeLeft -= OnSwipeLeft;
        SwipeEvents.OnSwipeRight -= OnSwipeRight;
        SwipeManager.OnSingleTap -= OnSingleTap;
        SwipeManager.OnDoubleTap -= OnDoubleTap;
    }

    void Update()
    {
        //Resets event text every few seconds
        ResetEventTextDefault();

        //Controls joystick and touch input
        ShowMovementInput();

        //Controls tilt input
        ShowTiltInput();
    }

    #endregion

    #region Private Methods

    //Resets event text to default text after certain time of inactivity
    private void ResetEventTextDefault()
    {
        if (Input.touchCount == 0 || !Input.GetMouseButton(0) && MovementState == MovementStates.None)
        {
            //Register startime only once for every reset
            if (triggerEventTextTimer)
            {
                triggerEventTextTimer = false;
                eventTextTimerStartTime = Time.time;
            }

            //if time has reached then set event text to default
            if (Time.time > eventTextTimerStartTime + eventTextResetTime)
            {
                triggerEventTextTimer = true;
                eventText.text = eventDefaultText;
            }
        }

        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            triggerEventTextTimer = true;
        }
    }

    //Shows tilt input on screen
    private void ShowTiltInput()
    {
        //if tilt dropdown is on we tilt
        if (tilting)
        {
            Vector3 tiltInput = TiltController.GetTilt();

            eventText.text = "(" + tiltInput.x + "," + tiltInput.y + "," + tiltInput.z + ")";
        }
    }

    //Shows joystick or touchpad inputs on screen
    private void ShowMovementInput()
    {
        //if movement state is joystick or touchpad then get inputs and show them on event text
        if (MovementState == MovementStates.Joystick)
        {
            float xInput = Joystick.HorizontalInput();
            float yInput = Joystick.VerticalInput();

            eventText.text = "Input Value: (" + xInput + " , " + yInput + ")";
        }
    }

    //Sets the intial default values to the camera
    private void SetCameraDefaults()
    {
        mainCamera.transform.position = origCamPos;
        mainCamera.transform.rotation = origCamRotation;

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = origCamOrthoSize;
        }
        else
        {
            mainCamera.fieldOfView = origCamPresFOV;
        }

        //Set orthographic to false as it will be overiden according to dropdown value selected 
        mainCamera.orthographic = false;
    }

    //Deactivate all controllers as to be overiden according to dropdown value selected
    private void DeactivateAllControllers()
    {
        SwipeManager.gameObject.SetActive(false);
        Joystick.gameObject.SetActive(false);
        TiltController.gameObject.SetActive(false);
        OrthoZoomAndPan.gameObject.SetActive(false);
    }

    //Initailize touch visualization according to initial toggle values
    private void InitializeTouchVisualization()
    {
        if (animateTouchToggle.isOn)
        {
            VisualizeTouch.enableTouchVisualization = true;
        }
        else
        {
            VisualizeTouch.enableTouchVisualization = false;
        }
    }

    //* Following functions sets-up the respective controllers when they are selected 

    private void SetUp4DirectionalSwipe()
    {
        //Sets to true and switches to 4-dir swipe
        SwipeManager.gameObject.SetActive(true);
        hintText.text = "Swipe in 4-directions (up, down, left, right) and tap or double tap to trigger respective events. Supports mouse and touch.";
    }

    private void SetUpTiltControls()
    {
        TiltController.gameObject.SetActive(true);
        hintText.text = "Tilt the device in any axis to see its input value on screen.";
        tilting = true;
    }

    private void SetUpJoystick()
    {
        //As joystick requires all three types of input controllers set them all to active
        Joystick.gameObject.SetActive(true);
        hintText.text = "Use the joystick to see its input value on screen or touch and drag on right side of the screen to rotate the camera in the desired direction. Supports only touch for camera rotation.";
        MovementState = MovementStates.Joystick; //Set the movement state to joystick
    }

    private void SetUp2DZoomAndPan()
    {
        //As it is 2d we set the camera type to orthographic and we also dont need event text for this
        Camera.main.orthographic = true;
        OrthoZoomAndPan.gameObject.SetActive(true);
        eventText.gameObject.SetActive(false);
        hintText.text = "Zoom and Pan (2D). Supports mouse and touch.";
    }

    #endregion

    //Delegate event callbacks
    #region Event Callback Functions

    //Swipe Callbacks
    private void OnSwipeUp()
    {
        eventText.text = "Up Swipe";
    }

    private void OnSwipeDown()
    {
        eventText.text = "Down Swipe";
    }

    private void OnSwipeLeft()
    {
        eventText.text = "Left Swipe";
    }

    private void OnSwipeRight()
    {
        eventText.text = "Right Swipe";
    }

    //Tap Callbacks
    private void OnSingleTap()
    {
        eventText.text = "Single Tap";
    }

    private void OnDoubleTap()
    {
        eventText.text = "Double Tap";
    }

    #endregion

    #region Public Functions To Drive UI

    //DropDown onValuechange functions
    public void DropDownManager(int index)
    {
        DeactivateAllControllers(); //Deactivate all controllers as we will reset them according to the dropdown value selected
        SetCameraDefaults(); //Set default initial values of camera as it wil be overriden according to the dropdown value selected
        MovementState = MovementStates.None; //Set this to none as it will be overriden according to according to the dropdown value selected
        tilting = false; //turn the tilting state to false as it will be overriden if respective dropdown is chosen.
        eventText.gameObject.SetActive(true); //Enable and reset the text to default
        eventText.text = eventDefaultText;

        //Setup events according to the dropdown value selected
        switch (index)
        {
            case 1: //4-directional swipe
                SetUp4DirectionalSwipe();
                break;
            case 2: //Tilt Controller
                SetUpTiltControls();
                break;
            case 3: //Joystick and camera rotation
                SetUpJoystick();
                break;
            case 4: //2d zoom and pan
                SetUp2DZoomAndPan();
                break;
            default: //index 0 or any other option not specified
                hintText.text = hintDefaultText;
                break;
        }
    }

    //Toggle Onvaluechange functions
    public void AnimateTouch(bool isOn)
    {
        if (isOn) //Toggle On
        {
            VisualizeTouch.enableTouchVisualization = true;
        }
        else //Toggle off
        {
            VisualizeTouch.enableTouchVisualization = false;
        }
    }

    #endregion

}
