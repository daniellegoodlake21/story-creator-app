using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManagerScript : MonoBehaviour
{
    public List<GameObject> props;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].GetComponent<PropScript>().SetPropID(i);
        }
        this.gameObject.transform.SetParent(GameObject.Find("HUDCanvas").transform, true);
        this.gameObject.transform.SetSiblingIndex(2);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<GameObject> GetAllProps()
    {
        List<GameObject> props = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.name != "PropInstanceManager")
            {
                GameObject prop = child.gameObject;
                props.Add(prop);
            }
        }
        return props;
    }

    public string DuplicateProp(int frameID, int propID, float x, float y, float scale, float rotation)
    {
        GameObject prop = null;
        GameObject found = GameObject.Find(this.props[propID].name + "Instance" + frameID.ToString());
        if (found)
        {
            Debug.Log("FOUND: " + found.gameObject.name);
            prop = found.gameObject;
            prop.name = this.props[propID].name + "Instance" + frameID.ToString();
        }
        if (prop)
        {
        }
        else
        {
            prop = Instantiate(this.props[propID]);
            prop.GetComponent<PropScript>().SetProp(propID, x, y, scale, rotation);
            prop.name = this.props[propID].name + "Instance" + frameID.ToString();
            prop.transform.SetParent(GameObject.Find("PropInstanceManager").transform, false);
        }
        prop.tag = "Prop";
        return prop.name;
    }
}
