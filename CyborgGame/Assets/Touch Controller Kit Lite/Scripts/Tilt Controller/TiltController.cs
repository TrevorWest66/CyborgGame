using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltController : MonoBehaviour
{
    #region Fields

    private Vector3 inputVector;

    [SerializeField] public bool enableTilt = true;

    #endregion

    #region Unity Methods

    void Update()
    {
        if (enableTilt)
        {
            inputVector.x = Input.acceleration.x;
            inputVector.y = Input.acceleration.y;
            inputVector.z = Input.acceleration.z;
        }
    }

    #endregion

    #region Private Methods

    public Vector3 GetTilt()
    {
        return inputVector;
    }

    #endregion
}
