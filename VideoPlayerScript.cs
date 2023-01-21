using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerScript : MonoBehaviour
{
    public GameObject frameBar;
    public GameObject buttons;
    public GameObject projectCreator;
    private int audioDuration;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleUI(bool value)
    {
        frameBar.SetActive(value);
        buttons.SetActive(value);
    }
    public IEnumerator PlayAudio(int i)
    {
        yield return new WaitForSeconds(1.5f);
        projectCreator.GetComponent<StoryProjectScript>().DisplayFrame(i);
        projectCreator.GetComponent<StoryProjectScript>().currentFrameIndex = i;
        projectCreator.GetComponent<StoryProjectScript>().PlayFrameAudio();

    }
    public void WalkthroughProject()
    {
        projectCreator.GetComponent<StoryProjectScript>().WalkthroughFrames();
    }
    public void ExportVideo()
    {
        this.ToggleUI(false);
        WalkthroughProject();
    }
}
