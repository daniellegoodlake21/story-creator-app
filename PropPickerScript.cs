using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PropPickerScript : MonoBehaviour
{
    private List<GameObject> propButtons;
    public GameObject propManager;
    public GameObject templatePropButton;
    public GameObject projectCreator;

    void OnEnable()
    {

        this.propButtons = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SelectProp(GameObject prop)
    {
        GameObject newProp = Instantiate(prop);
        newProp.GetComponent<PropScript>().SetPropID(prop.GetComponent<PropScript>().GetPropID());
        newProp.name = prop.name + "Instance" + projectCreator.GetComponent<StoryProjectScript>().GetCurrentFrameIndex().ToString();
        newProp.transform.SetParent(GameObject.Find("PropInstanceManager").transform, false);
        newProp.GetComponent<PropScript>().SetPropPosition(300, 300);
        projectCreator.GetComponent<StoryProjectScript>().SavePropToFrame(newProp);
        GameObject.Find("PropPicker").SetActive(false);
    }
    public void LoadProps()
    {
        this.propButtons = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        List<GameObject> props = propManager.GetComponent<PropManagerScript>().GetAllProps();
        foreach (GameObject prop in props)
        {
            if (!projectCreator.GetComponent<StoryProjectScript>().PropExists(prop))
            {
                GameObject propButton = Instantiate(templatePropButton);
                propButton.name = prop.name + "Button";
                propButton.GetComponent<Image>().sprite = prop.GetComponent<Image>().sprite;
                propButton.transform.SetParent(this.gameObject.transform, false);
                propButton.GetComponent<PropPickerPropScript>().SetProp(prop);
                this.propButtons.Add(propButton);
            }

        }
    }
}
