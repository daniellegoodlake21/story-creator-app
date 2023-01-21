using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FrameScript : MonoBehaviour
{
    public int projectID;
    public int frameID;
    public int backgroundImageIndex;
    public List<GameObject> characters;
    public List<GameObject> props;
    // voice recording varibles
    private int minFreq;
    private int maxFreq;
    public AudioSource audioSource;
    private bool microphoneConnected;
    private VoiceActingAudioClip voiceActingAudioClip;
    private int audioTime;
    public bool loaded;
    public float length;
    // end of voice recording variables
    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }
    void OnEnable()
    {
        loaded = false;
        audioTime = 0;
        Transform frameParent = this.gameObject.transform;
        voiceActingAudioClip = this.GetComponent<VoiceActingAudioClip>();
        if (Microphone.devices.Length <= 0)
        {
            Debug.Log("Microphone not connected!");
            microphoneConnected = false;
        }
        else
        {
            microphoneConnected = true;
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
            }

            //Get the attached AudioSource component  
            audioSource = this.gameObject.GetComponent<AudioSource>();
            voiceActingAudioClip.SetVoiceActingAudioClip(this.projectID, this.frameID, audioSource.clip);
            voiceActingAudioClip.LoadAudioClip();


        }
    }
    // voice recording methods
    public void RecordVoiceActingAudioClip()
    {
        if (microphoneConnected)
        {
            audioSource.clip = Microphone.Start(null, true, 20, maxFreq);
            StartCoroutine(GameObject.Find("Scripts").GetComponent<StoryProjectScript>().WaitForMinimumAudioLengthToPass());
        }
    }
    public float GetAudioLength()
    {
        this.length = this.audioSource.clip.length;
        return this.length;
    }
    public void PlayAudio()
    {
        StartCoroutine(voiceActingAudioClip.LoadAndPlaySound());
    }
    public void EndVoiceActingAudioClipRecording()
    {
        this.audioTime = Microphone.GetPosition(null);
        if (this.audioTime / 44100 != 0)
        {
            this.audioTime = (int)Math.Ceiling((double)this.audioTime / 44100) * 44100;
        }
        Microphone.End(null);
        audioSource.Stop();
        GameObject.Find("Scripts").GetComponent<StoryProjectScript>().EndAudioRecording();
        AudioClip ac = audioSource.clip;
        float lengthL = ac.length;
        float samplesL = ac.samples;
        float samplesPerSec = (float)samplesL / lengthL;
        float[] samples = new float[(int)(samplesPerSec * (audioTime/44100))];
        ac.GetData(samples, 0);
        audioSource.clip = AudioClip.Create("LoadedAudio", (int)((audioTime / 44100) * samplesPerSec), 1, (int)samplesPerSec, false);
        audioSource.clip.SetData(samples, 0);
        audioSource.Play();
        this.length = audioSource.clip.length;
        voiceActingAudioClip.SaveVoiceActingAudioClip(this.audioSource.clip);
    }
    // end of voice recording methods
    public void AddCharacter(GameObject character)
    {
        this.characters.Add(character);
    }
    public void AddProp(GameObject prop)
    {
        this.props.Add(prop);
    }
    public int GetCharacterID(int index)
    {
        return characters[index].GetComponent<CharacterScript>().GetCharacterID();
    }
    public void RemoveCharacterByID(int characterID)
    {
        for (int i = 0; i < this.characters.Count; i++)
        {
            if (this.characters[i].GetComponent<CharacterScript>().GetCharacterID() == characterID)
            {
                this.characters[i].SetActive(false);
                Destroy(this.characters[i]);
                this.characters.RemoveAt(i);
            }
        }
    }
    public void RemovePropByID(int propID)
    {
        for (int i = 0; i < this.props.Count; i++)
        {
            if (this.props[i].GetComponent<PropScript>().GetPropID() == propID)
            {
                this.props[i].SetActive(false);
                Destroy(this.props[i]);
                this.props.RemoveAt(i);
            }
        }
    }
    public int GetFrameID()
    {
        return this.frameID;
    }
    public void SetBasicFrameData(int projectID, int frameID)
    {
        this.projectID = projectID;
        this.frameID = frameID;
    }
    public void SetFrameID(int frameID)
    {
        this.frameID = frameID;
    }
    public void SetBackgroundImage(int backgroundImageIndex)
    {
        this.backgroundImageIndex = backgroundImageIndex;
        this.DisplayBackgroundImage();

    }
    public bool AudioClipIsPlaying()
    {
        return this.audioSource.isPlaying;
    }
    // Update is called once per frame
    void Update()
    {
        if (!loaded && voiceActingAudioClip.GetAudioClip() != null)
        {
            loaded = true;
            audioSource.clip = voiceActingAudioClip.GetAudioClip();
            audioSource.clip.name = "LoadedVoiceActingClip";
        }
    }
    public List<GameObject> GetAllCharacters()
    {
        return this.characters;
    }
    public List<GameObject> GetAllProps()
    {
        return this.props;
    }
    public Texture2D GetBackgroundImage()
    {
        return GameObject.Find("BackgroundImageManager").GetComponent<BackgroundImageManagerScript>().GetBackgroundImage(this.backgroundImageIndex);
    }
    public int GetBackgroundImageIndex()
    {
        return this.backgroundImageIndex;
    }
    public void SetFrameData(int projectID, int frameID, int backgroundImageIndex, List<GameObject> characters, List<GameObject> props)
    {
        this.projectID = projectID;
        this.frameID = frameID;
        this.backgroundImageIndex = backgroundImageIndex;
        this.characters = characters;
        this.props = props;
    }
    public void DisplayBackgroundImage()
    {
        Texture2D backgroundImage = this.GetBackgroundImage();
        Sprite sprite = Sprite.Create(backgroundImage, new Rect(0, 0, backgroundImage.width, backgroundImage.height), new Vector2(0.5f, 0.5f), 1.0f);
        GameObject.Find("SelectedFrame").GetComponent<Image>().sprite = sprite;
    }
    public void HideFrame()
    {
        this.gameObject.SetActive(false);
    }
}
