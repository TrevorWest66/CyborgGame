using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenFirst : MonoBehaviour
{
    public GameObject theDoor;

    private void OnTriggerEnter(Collider other)
    {
        theDoor.GetComponent<Animator>().Play("DoorOpen");
    }

}
