using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopUp : MonoBehaviour
{
    public void Open()
	{
		gameObject.SetActive(true);
	}
	public void Close()
	{
		gameObject.SetActive(false);
	}
	public void OnSubmitName(string name)
	{
		Debug.Log(name);
	}
	public void OnSpeedSlider(float speed)
	{
		Debug.Log("First speed change Value: " + speed);
		Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
	}
}
