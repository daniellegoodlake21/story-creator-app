using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameManager : MonoBehaviour
{
    private List<GameObject> frames;
    public GameObject btnFrameTemplate;
    // Start is called before the first frame update
    void Start()
    {
        this.frames = new List<GameObject>();
    }
    public void SetProjectFrames(List<GameObject> frames)
    {
        this.frames = frames;
    }
    // Update is called once per frame
    void Update()
    {
    }
    private float findXAxisPosition()
    {
        float positionY = (float)frames.Count*((float)btnFrameTemplate.GetComponent<RectTransform>().sizeDelta.x + 0.2f);
        return positionY;
    }
    private void AddFrame(GameObject frame)
    {
        RenderTexture texture = frame.GetComponent<Frame>().MergeSprites();
        GameObject btnFrame = (GameObject)Instantiate(btnFrameTemplate, new Vector3(transform.position.y, findXAxisPosition(), 0f), new Quaternion(0f, 0f, 0f, 0f));
        btnFrame.GetComponent<FrameButtonScript>().SetFrameButton(frame);
        btnFrame.GetComponent<Button>().onClick.AddListener(btnFrame.GetComponent<FrameButtonScript>().LoadFrame);
    }
    public void DisplayFramesBar()
    {
        foreach (GameObject frame in frames)
        {
            this.AddFrame(frame);
        }
    }
}
