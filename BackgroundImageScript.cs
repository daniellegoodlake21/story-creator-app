using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisBackground);
    }

    private int GetThisBackgroundImageIndex()
    {
        return int.Parse(this.gameObject.name);
    }
    private void SelectThisBackground()
    {
        GameObject.Find("Scripts").GetComponent<StoryProjectScript>().UpdateCurrentFrameBackgroundImage(this.GetThisBackgroundImageIndex());
        GameObject.Find("BackgroundPicker").SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
