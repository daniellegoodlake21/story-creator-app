using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePickerFrameScript : MonoBehaviour
{
    public int frameIndex;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisFrame);
    }
    public void SelectThisFrame()
    {
        GameObject.Find("FramePickerContent").GetComponent<FramePickerScript>().SetFrame(this.frameIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
