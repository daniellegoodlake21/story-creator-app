using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePickerScript : MonoBehaviour
{
    private List<GameObject> frameButtons;
    public GameObject projectCreator;
    public GameObject frameButtonTemplate;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        this.frameButtons = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadFrames()
    {
        this.frameButtons = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        int count = 1;
        for (int i = 0; i < projectCreator.GetComponent<StoryProjectScript>().frames.Count; i++)
        {
            GameObject frameButton = Instantiate(frameButtonTemplate);
            frameButton.name = i.ToString();
            frameButton.transform.SetParent(this.gameObject.transform, false);
            frameButton.GetComponent<FramePickerFrameScript>().frameIndex = i;
            frameButton.transform.GetChild(0).GetComponent<Text>().text = "Frame " + count.ToString();
            int backgroundImageIndex = projectCreator.GetComponent<StoryProjectScript>().GetBackgroundImageIndex2(i.ToString());
            frameButton.GetComponent<Image>().sprite = GameObject.Find("BackgroundImageManager").GetComponent<BackgroundImageManagerScript>().GetBackgroundImageSprite(backgroundImageIndex);
            this.frameButtons.Add(frameButton);
            count++;
        }
    }
    public void SetFrame(int frameIndex)
    {
        projectCreator.GetComponent<StoryProjectScript>().DisplayFrame(frameIndex);
        GameObject.Find("FramePicker").SetActive(false);
    }
}
