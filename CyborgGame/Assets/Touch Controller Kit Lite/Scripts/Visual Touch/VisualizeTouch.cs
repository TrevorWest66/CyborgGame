using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeTouch : MonoBehaviour
{
    #region Fields

    private Image touchSpawnObj;

    [Tooltip("Enable to show touches on screen.")]
    [SerializeField] public bool enableTouchVisualization = true;

    [Tooltip("How much distance from camera the line or trail gets rendered.")]
    [SerializeField] [Range(0f, 100f)] private float cameraPlaneDistance = 1f; //distance between the camera plane (the rendered screen z position) and the camera

    #endregion

    #region Unity Methods

    void Awake()
    {
        //if no camera is set to the canvas then we set the main camera in the scene as the camera for the canvas
        if (GetComponent<Canvas>().worldCamera == null)
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        Input.simulateMouseWithTouches = false;

        touchSpawnObj = transform.GetChild(0).gameObject.GetComponent<Image>();

        touchSpawnObj.gameObject.SetActive(false);

    }

    void Update()
    {
        //* For touch inputs
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {

                //get touches wrt to value of i
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    //true as parameter means we are setting line renderer intially
                    PlaceObjectOnScreen(touch.position);
                }

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    //false as parameter means we are updating line renderer position and not setting it initially
                    PlaceObjectOnScreen(touch.position);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    //set all visualizations to inactive
                    ResetTouchObject();
                }

            } //if there is no touch
            else if (Input.touchCount == 0)
            {
                //we check if any of our touch visualizations are still active while there is no touch and deactivate them if there is one.
                //This is necessary in cases when maxixmum touch count allowed is not supported by the device.
                //In this cases there may be a glitch where an object may get stuck on screen if the user is trying to touch with more fingers than supported by his device.

                ResetTouchObject();

            }

        }
        else //* For Mouse inputs
        {
            if (Input.GetMouseButtonDown(0))
            {
                //true as parameter means we are setting line renderer intially
                PlaceObjectOnScreen(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                //false as parameter means we are updating line renderer position and not setting it initially
                PlaceObjectOnScreen(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                //set all visualizations to inactive
                ResetTouchObject();
            }

            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
            {
                /* we check if any of our touch visualizations are still active while there is no mouse input
                and deactivate them if there is one. */
                ResetTouchObject();
            }
        }

    }

    #endregion

    #region Private Methods

    //place visualizations on screen based on parameters.
    //positon is the postion to spawn, index is the index of touch which is equal to index of array to spawn obj, 
    //setLIne initially indicates whether we are drawing a line intially or just updating an existing one
    void PlaceObjectOnScreen(Vector3 position)
    {
        //convert touch position to screen point based on camera plane position
        Vector3 screenPointPosition = Camera.main.ScreenToWorldPoint(new Vector3(
        position.x, position.y, cameraPlaneDistance));

        if (enableTouchVisualization)
        {
            //if its inactive then set it to active
            if (!touchSpawnObj.gameObject.activeInHierarchy)
            {
                touchSpawnObj.gameObject.SetActive(true);
            }

            //move img rect transform with touch by using position of touch as we want it to be over ui(s).
            touchSpawnObj.rectTransform.position = position;
        }
    }

    //reset object and trail when there is to touch
    void ResetTouchObject()
    {
        if (touchSpawnObj.gameObject.activeInHierarchy)
        {
            touchSpawnObj.gameObject.SetActive(false);
        }
    }


    #endregion
}
