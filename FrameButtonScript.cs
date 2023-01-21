using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameButtonScript : MonoBehaviour
{
    private GameObject frame;
    public void SetFrameButton(GameObject frameObject)
    {
        this.frame = frameObject;
    }
    public void LoadFrame()
    {
        GameObject frameEditor = GameObject.Find("FrameEditor");
        frameEditor.GetComponent<Image>().material.SetTexture("_MainTex", frame.GetComponent<Frame>().GetBackgroundImage().texture);
    }
    public void SetFrame(int projectID, int frameID, Texture2D backgroundImage, List<GameObject> characters, AudioClip recording)
    {
        this.frame.GetComponent<Frame>().SetFrame(projectID, frameID, backgroundImage, characters, recording);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
