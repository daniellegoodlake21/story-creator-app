using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Threading;
using UnityEngine.Networking;

public class VoiceActingAudioClip : MonoBehaviour
{

    private string filepath;
    int projectID;
    int frameID;
    public AudioSource source;
    public Button play;
    public bool finishedLoading;
    public void SetVoiceActingAudioClip(int projectID, int frameID, AudioClip clip)
    {
        this.projectID = projectID;
        this.frameID = frameID;
        this.source.clip = clip;
        filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DanielleApps", projectID.ToString() + "\\" + frameID.ToString(), "VoiceActingAudioClip.wav");

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public IEnumerator LoadAndPlaySound()
    {
        finishedLoading = false;
        string url = string.Format("file:///{0}", filepath);
        source = GetComponent<AudioSource>();
        WWW www = new WWW(url);
        yield return www;
        source.clip = www.GetAudioClip(false, false);
        finishedLoading = true;
        source.PlayOneShot(source.clip);
        if (source.clip.length > 2)
        {
            this.gameObject.GetComponent<FrameScript>().length = source.clip.length;
        }
        else
        {
            this.gameObject.GetComponent<FrameScript>().length = 2; // length by default if the voice audio file is empty
        }

    }
    public IEnumerator LoadSound()
    {
        finishedLoading = false;
        string url = string.Format("file:///{0}", filepath);
        source = GetComponent<AudioSource>();
        WWW www = new WWW(url);
        yield return www;
        finishedLoading = true;
        source.clip = www.GetAudioClip(false, false);
    }
    public void SaveVoiceActingAudioClip(AudioClip audioClip)
    {
        SavWav.Save(filepath, audioClip);
    }
    public void PlayVoiceActingAudioClip()
    {
        this.source.Play();
        while (this.source.isPlaying)
        {
        }
    }
    public AudioClip GetAudioClip()
    {
        return this.source.clip;
    }
    public void LoadAudioClip()
    {
        StartCoroutine(LoadSound());
    }
}
