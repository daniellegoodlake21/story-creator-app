using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class FileHandler : MonoBehaviour
{
    public string contents;
    public string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        contents = "";
        lines = new string[5]; // may need to change to 6 after adding live movement options
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReadLines(string filename)
    {
        this.lines = File.ReadAllLines(filename);
        Debug.Log("The sprite is currently " + lines[0]);
    }
    public void Read(string filename)
    {
        this.contents = File.ReadAllText(filename);
    }
    public void Write(string filename, string contents)
    {
        File.WriteAllText(filename, contents);
    }
}
