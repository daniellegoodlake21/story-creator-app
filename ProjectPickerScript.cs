using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class ProjectPickerScript : MonoBehaviour
{
    private List<GameObject> projectButtons;
    public GameObject projectButtonTemplate;
    private string appDirectory;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnEnable()
    {
        this.appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DanielleApps");
        this.LoadProjects();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadProjects()
    {
        this.projectButtons = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        List<int> projectIDs = new List<int>();
        List<string> projectNames = new List<string>();
        foreach (DirectoryInfo projectDirectoryInfo in new DirectoryInfo(this.appDirectory).GetDirectories())
        {
            projectIDs.Add(int.Parse(projectDirectoryInfo.Name));
            string name = File.ReadAllText(projectDirectoryInfo.FullName+"\\StoryNameData.txt");
            projectNames.Add(name);
        }
        int count = 1;
        foreach (int i in projectIDs)
        {
            GameObject projectButton = Instantiate(projectButtonTemplate);
            projectButton.name = i.ToString();
            projectButton.transform.SetParent(this.gameObject.transform, false);
            projectButton.GetComponent<ProjectScript>().projectName = projectNames[i];
            projectButton.GetComponent<ProjectScript>().SetProjectID(int.Parse(projectButton.gameObject.name));
            projectButton.transform.GetChild(0).GetComponent<Text>().text = projectButton.GetComponent<ProjectScript>().projectName + " " + count.ToString();
            this.projectButtons.Add(projectButton);
            count++;
        }
    }
    public void SelectProject(int projectIndex)
    {
        GameObject.Find("Scripts").GetComponent<MainMenuScript>().SelectStory(projectIndex);
    }
}
