using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Frame : MonoBehaviour
{
    private int frameID;
    private int projectID;
    private int backgroundImageIndex;
    private Texture2D backgroundImage;
    public List<GameObject> characters;
    private AudioClip recording;
    private AudioSource audioSource;
    private bool microphoneConnected;
    private VoiceActingAudioClip voiceActingAudioClip;
    public GameObject voiceActingObject;
    private string directory;
    private int minFreq;
    private int maxFreq;
    public void SetBasicFrame(int projectID, int frameID)
    {
        this.projectID = projectID;
        this.frameID = frameID;
        this.directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\StoryCreator\\" + this.projectID.ToString() + "\\" + this.frameID.ToString() + "\\";
    }
    public void SetFrame(int projectID, int frameID, Texture2D backgroundImage, List<GameObject> characters, AudioClip recording)
    {
        this.directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\StoryCreator\\" + this.projectID.ToString() + "\\" + this.frameID.ToString() + "\\";
        this.projectID = projectID;
        this.frameID = frameID;
        this.backgroundImage = backgroundImage;
        this.backgroundImage.Resize(1920, 1080);
        this.characters = characters;
        this.recording = recording;
    }
    public Sprite GetBackgroundImage()
    {
        Sprite backgroundSprite = Sprite.Create(this.backgroundImage, new Rect(0, 0, this.backgroundImage.width, this.backgroundImage.height), new Vector2(0.5f, 0.5f), 100.0f);
        return backgroundSprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.backgroundImage = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void RecordVoiceActingAudioClip()
    {
        if (microphoneConnected)
        {
            AudioClip audioClip = Microphone.Start(null, true, 20, maxFreq);
        }
        else
        {
            Debug.Log("Cannot record audio as no microphone is connected");
        }
    }
    public void EndVoiceActingAudioClipRecording()
    {
        Microphone.End(null);
        audioSource.Play();
    }
    public RenderTexture MergeSprites()
    {
        RenderTexture mergedTexture = new RenderTexture(1920, 1080, 0);
        Graphics.Blit(this.backgroundImage, mergedTexture);
        foreach (GameObject character in this.characters)
        {
            Graphics.Blit(character.GetComponent<Character>().GetCurrentSprite().texture, mergedTexture);
        }
        return mergedTexture;
    }
    public void SaveBackground()
    {
        byte[] backgroundImageData = backgroundImage.EncodeToPNG();
        File.WriteAllBytes(this.directory + "Background.png", backgroundImageData);
    }
    public void SaveCharacters()
    {
        string positions = "";
        for (int i = 0; i < characters.Count; i++)
        {
            Character character = characters[i].GetComponent<Character>();
            byte[] characterSpriteData = character.GetCurrentSprite().texture.EncodeToPNG();
            File.WriteAllBytes(this.directory + character.GetCharacterID() + ".png", characterSpriteData);
            positions = positions + character.GetCharacterID() + ":" + character.GetXAxisPosition() + ":" + character.GetYAxisPosition();
            if (i < characters.Count-1)
            {
                positions = positions + "\n";
            }
        }
        File.WriteAllText(this.directory + "CharacterPositions.txt", positions);
    }
    public void SaveFrameData()
    {
        try
        {
            if (!Directory.Exists(this.directory))
            {
                DirectoryInfo folder = Directory.CreateDirectory(this.directory);
                voiceActingAudioClip.SaveVoiceActingAudioClip(audioSource.clip);
                SaveBackground();
                SaveCharacters();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
