using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProjectScript : MonoBehaviour
{
    public int projectID;
    public string projectName;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisProject);
    }
    public void SetProjectID(int id)
    {
        projectID = id;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void SelectThisProject()
    {
        GameObject.Find("ProjectPicker").GetComponent<ProjectPickerScript>().SelectProject(int.Parse(gameObject.name));
    }
}
