using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region Fields

    private Image joystickAreaImg; //The background of the joystick
    private Image joystickHandleImg; //The joystick handle or knob that will move with inputs
    private Vector2 joystickHandleImgInitialPosition;
    private Vector2 joystickInput;

    [Tooltip("The area upto which joystick handle can be dragged. The lesser value means larger area.")]
    [SerializeField] [Range(0.1f, 2f)] private float handleArea = 0.6f; //Area upto which joystick handle can be dragged

    #endregion

    #region Unity Methods

    void Awake()
    {
        joystickAreaImg = transform.GetChild(0).GetComponent<Image>();
        joystickHandleImg = joystickAreaImg.transform.GetChild(0).GetComponent<Image>();

        //Storing intial position of the handle
        joystickHandleImgInitialPosition = joystickHandleImg.rectTransform.anchoredPosition;
    }

    void OnDisable()
    {
        //Setting input to zero and the position of joystick handle to intial position if is disabled
        joystickInput = Vector2.zero;
        joystickHandleImg.rectTransform.anchoredPosition = joystickHandleImgInitialPosition;
    }

    #endregion

    #region Callback Events

    //When pointer is put down and just clicked
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //Reference ondrag to handle the event
        OnDrag(eventData);
    }

    //when pointer is being dragged after being clicked
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        //Checking if  the touch was within the range of joystickAreaImg
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickAreaImg.rectTransform,
        eventData.position, eventData.pressEventCamera, out position))
        {
            //Setting position relative to the size of joystick background
            position.x = position.x / joystickAreaImg.rectTransform.sizeDelta.x;
            position.y = position.y / joystickAreaImg.rectTransform.sizeDelta.y;

            //setting its value such that it remains within -1,1
            joystickInput = new Vector2(position.x * 2, position.y * 2);

            //Normalizing so that it doesn't go past -1,1
            if (joystickInput.magnitude > 1)
            {
                joystickInput = joystickInput.normalized;
            }

            //Changing the position of the joystick handle as per input and handle area
            joystickHandleImg.rectTransform.anchoredPosition = new Vector2(joystickInput.x * (joystickHandleImg.rectTransform.sizeDelta.x / handleArea),
            joystickInput.y * (joystickHandleImg.rectTransform.sizeDelta.y / handleArea));

        }
    }

    //When pointer is moved up
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        //Setting input to zero and the position of joystick handle to intial position
        joystickInput = Vector2.zero;
        joystickHandleImg.rectTransform.anchoredPosition = joystickHandleImgInitialPosition;
    }


    #endregion

    #region Private Methods

    //Returns horizontal (x-axis) input
    public float HorizontalInput()
    {
        return joystickInput.x;
    }

    //Returns Vertical (y-axis) input
    public float VerticalInput()
    {
        return joystickInput.y;
    }

    #endregion
}
