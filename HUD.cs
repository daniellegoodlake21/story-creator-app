using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button backToMainMenu;
    public Button addNewFrame;
    public Button addCharacter;
    public FrameManager frameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveAndReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
