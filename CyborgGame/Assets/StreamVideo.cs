using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class StreamVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    // Use this for initialization
    void OnEnable()
    {
		StartCoroutine(PlayVideo());
    }
	
    IEnumerator PlayVideo()
    {
		
		videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
		
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }
}