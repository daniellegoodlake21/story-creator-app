using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropPickerPropScript : MonoBehaviour
{
    public GameObject prop;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisProp);
    }
    public void SetProp(GameObject prop)
    {
        this.prop = prop;
    }
    public void SelectThisProp()
    {
        GameObject.Find("PropPickerContent").GetComponent<PropPickerScript>().SelectProp(this.prop);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
