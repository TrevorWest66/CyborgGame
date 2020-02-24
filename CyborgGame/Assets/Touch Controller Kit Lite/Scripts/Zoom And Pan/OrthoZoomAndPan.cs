using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrthoZoomAndPan : MonoBehaviour
{
    #region Fields

    //* Variables to be handeled by code */
    private Vector3 startTouchPosition;
    private Vector3 panDirection;
    private Camera mainCamera;
    private bool canPan = true;

    //* Variables used only for its value as defined in the inspector */
    [Header("Zoom Controls")]
    [SerializeField] private bool enableZoom = true;

    [Tooltip("Speed at which zoom occurs.")]
    [SerializeField] [Range(0f, 100f)] private float zoomSpeed = 1f;

    [Tooltip("Maximum zoom in amount")]
    [SerializeField] [Range(1f, 100f)] private float maxZoomInLevel = 100f;

    [Tooltip("Maximum zoom out amount")]
    [SerializeField] [Range(1f, 100f)] private float maxZoomOutLevel = 40f;

    [Header("Pan Controls")]
    [SerializeField] private bool enablePan = true;

    [Tooltip("Speed at which pan occurs.")]
    [SerializeField] [Range(0.01f, 1.6f)] private float panSpeed = 0.5f;

    [Tooltip("Enable pan in x-direction?")]
    [SerializeField] private bool enableXAxisPan = true;

    [Tooltip("Enable pan in y-direction?")]
    [SerializeField] private bool enableYAxisPan = true;

    [Header("Pan Axis Bounds")]
    [SerializeField] private float minXBounds = -30f;
    [SerializeField] private float maxXBounds = 30f;
    [SerializeField] private float minYBounds = -5f;
    [SerializeField] private float maxYBounds = 12f;

    #endregion

    #region Unity Methods

    void Start()
    {
        //Getting main camera
        mainCamera = Camera.main;

        //Camera is Prespective
        if (!mainCamera.orthographic)
        {
            //Log error as this script only works for ortho
            Debug.LogError("This script only works with Orthographic camera. For Prespective camera use the other prefab: PRESPECTCamSimpleZoomAndPan");
        }

        //We do not want to simualte touch with mouse input 
        Input.simulateMouseWithTouches = false;

        //Converting to orthographic size
        maxZoomInLevel = Mathf.Abs((maxZoomInLevel / 2) - 50) + 1;
        maxZoomOutLevel = maxZoomOutLevel / 2;

    }
    void Update()
    {
        //This prevents clicks and touches through UI
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        MouseInputPanAndZoom();

        TouchInputZoomAndPan();

    }

    #endregion

    #region Private Methods

    void MouseInputPanAndZoom()
    {
        if (enablePan)
        {
            MousePan();
        }

        if (enableZoom)
        {
            //Zoom with scroll wheel
            float zoomLevel = Input.GetAxis("Mouse ScrollWheel");
            Zoom(zoomLevel);
        }
    }

    void MousePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 newMousePostion = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            //Calculate pan direciton and pan accordingly
            panDirection = startTouchPosition - newMousePostion;
            Pan(panDirection);
        }
    }

    void TouchInputZoomAndPan()
    {
        if (Input.touchCount == 2 & enableZoom)
        {
            //for touch zoom
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            //Calcualte the distance of both the finger's movement
            Vector2 prevFirstTouchPosition = firstTouch.position - firstTouch.deltaPosition;
            Vector2 prevsecondTouchPosition = secondTouch.position - secondTouch.deltaPosition;

            //Calcuate its magnitude to apply to zoom
            float prevMagnitude = (prevFirstTouchPosition - prevsecondTouchPosition).magnitude;
            float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            //find zoomlevel by subtracting prev finger movement magnitude from current one
            float zoomLevel = currentMagnitude - prevMagnitude;
            Zoom(zoomLevel);

            //Set can pan to false to prevent abrupt pan after zoom
            canPan = false;

            if (firstTouch.phase == TouchPhase.Ended || secondTouch.phase == TouchPhase.Ended)
            {
                //After zoom has ended we set canPan back to true
                Invoke("ReturnCanPanToTrue", 0.2f);
            }
        }
        else if (Input.touchCount == 1 && enablePan)
        {
            //for pan
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 newTouchPostion = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                //Calculate pan direciton and pan accordingly
                panDirection = startTouchPosition - newTouchPostion;
                if (canPan)
                {
                    //only pan if we can pan
                    Pan(panDirection);
                }
            }


        }
    }

    //for zoom
    void Zoom(float zoomLevel)
    {
        //zoom by changing the orthographic size of camera
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomLevel * zoomSpeed * 2, maxZoomInLevel, maxZoomOutLevel);
    }

    //for pan
    void Pan(Vector3 direction)
    {
        //Calculating movement based on directon and pan speed. We only pan in x or y if it is enabled
        Vector3 movement = new Vector3(enableXAxisPan ? (direction.x * panSpeed) : 0.0f,
        enableYAxisPan ? (direction.y * panSpeed) : 0.0f, 0f);

        //Pan by adding the movement to the transform of maincamera
        mainCamera.transform.position += movement;

        Vector3 newMainCameraPosition = mainCamera.transform.position;

        if (enableXAxisPan)
        {
            newMainCameraPosition.x = Mathf.Clamp(mainCamera.transform.position.x, minXBounds, maxXBounds);
        }
        if (enableYAxisPan)
        {
            newMainCameraPosition.y = Mathf.Clamp(mainCamera.transform.position.y, minYBounds, maxYBounds);
        }

        mainCamera.transform.position = newMainCameraPosition;
    }

    //returns canPan value to true
    //! Called by function name with string reference
    void ReturnCanPanToTrue()
    {
        canPan = true;
    }

    #endregion
}
