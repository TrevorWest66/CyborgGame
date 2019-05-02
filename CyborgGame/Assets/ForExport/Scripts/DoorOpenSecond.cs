using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenSecond : MonoBehaviour
{
    public GameObject theDoor1;

    private void OnTriggerEnter(Collider other)
    {
        theDoor1.GetComponent<Animator>().Play("DoorOpen1");
    }

}
