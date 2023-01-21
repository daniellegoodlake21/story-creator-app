using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject btnContinueStory;
    public GameObject btnCreateNewStory;
    public GameObject projectPicker;
    private string appDirectory;
    private FileHandler fileHandler;
    private void SetupApp()
    {
        if (!Directory.Exists(this.appDirectory))
        {
            Directory.CreateDirectory(this.appDirectory);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        fileHandler = GameObject.Find("Scripts").GetComponent<FileHandler>();
        this.appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DanielleApps");
        this.SetupApp();
        this.projectPicker.SetActive(false);
        if (new DirectoryInfo(this.appDirectory).GetDirectories().Length == 0)
        {
            this.btnContinueStory.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ContinueStory()
    {
        this.projectPicker.SetActive(true);
    }
    public void SelectStory(int projectIndex)
    {
        fileHandler.Write(this.appDirectory + "\\StoryProjectIndexData.txt", projectIndex.ToString());
        SceneManager.LoadScene("MainScene");
    }
    public void CreateNewStory()
    {
        fileHandler.Write(this.appDirectory + "\\StoryProjectIndexData.txt", "-1");
        SceneManager.LoadScene("MainScene");
    }
}
