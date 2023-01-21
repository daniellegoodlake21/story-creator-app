using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StoryProjectScript : MonoBehaviour
{
    [Serializable]
    public class ItemMoveData
    {
        public int id;
        public float x;
        public float y;
        public bool delayed;
    }
    [Serializable]
    public class CharacterFileData
    {
        public int id;
        public int sprite;
        public float x;
        public float y;
        public float scale;
        public float rotation;
        public ItemMoveData itemMoveData; 
    }
    [Serializable]
    public class PropFileData
    {
        public int id;
        public float x;
        public float y;
        public float scale;
        public float rotation;
        public ItemMoveData itemMoveData;
    }
    [Serializable]
    public class ItemCollectionFileData
    {
        public List<CharacterFileData> characterFileDatas;
        public List<PropFileData> propFileDatas;
    }
    private int projectID;
    private string projectDirectory;
    private string appDirectory;
    public List<GameObject> frames;
    public GameObject frameTemplate;
    public GameObject characterManager;
    public void UpdateCharacterMoveData(GameObject character)
    {
        string filepath = this.projectDirectory + "\\" + this.frames[currentFrameIndex].name + "\\ItemCollectionData.json";
        fileHandler.Read(filepath);
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
        for (int i = 0; i < itemCollectionFileData.characterFileDatas.Count; i++)
        {
            CharacterFileData characterFileData = itemCollectionFileData.characterFileDatas[i];
            if (characterFileData.id == character.GetComponent<CharacterScript>().GetCharacterID())
            {
                ItemMoveData itemMoveData = new ItemMoveData();
                MoveScript moveScript = character.GetComponent<CharacterScript>().GetMove().GetComponent<MoveScript>();
                itemMoveData.id = moveScript.GetMoveID();
                itemMoveData.x = moveScript.GetX();
                itemMoveData.y = moveScript.GetY();
                itemMoveData.delayed = moveScript.IsDelayed();
                itemCollectionFileData.characterFileDatas[i].itemMoveData = itemMoveData;
            }
        }
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(filepath, json);
    }
    public void UpdatePropMoveData(GameObject prop)
    {
        string filepath = this.projectDirectory + "\\" + this.frames[currentFrameIndex].name + "\\ItemCollectionData.json";
        fileHandler.Read(filepath);
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
        for (int i = 0; i < itemCollectionFileData.propFileDatas.Count; i++)
        {
            PropFileData propFileData = itemCollectionFileData.propFileDatas[i];
            if (propFileData.id == prop.GetComponent<PropScript>().GetPropID())
            {
                ItemMoveData itemMoveData = new ItemMoveData();
                MoveScript moveScript = prop.GetComponent<PropScript>().GetMove().GetComponent<MoveScript>();
                itemMoveData.id = moveScript.GetMoveID();
                itemMoveData.x = moveScript.GetX();
                itemMoveData.y = moveScript.GetY();
                itemMoveData.delayed = moveScript.IsDelayed();
                itemCollectionFileData.propFileDatas[i].itemMoveData = itemMoveData;
            }
        }
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(filepath, json);
    }

    public GameObject propManager;
    public GameObject moveManager;
    public int currentFrameIndex;
    public FileHandler fileHandler;
    private List<GameObject> characters;
    private List<GameObject> moves;
    private List<GameObject> props;
    private int backgroundImageIndex;
    public GameObject btnRemoveFrame;
    public GameObject btnStopRecording;
    public GameObject btnStartRecording;
    public GameObject btnMoveCharacter;
    public GameObject btnRemoveCharacter;
    public GameObject btnResizeRotate;
    public GameObject btnLiveMovement;
    public GameObject btnChangeSprite;
    private bool removing;
    private bool copyingFrame;
    private void SetupApp()
    {
        if (!Directory.Exists(this.appDirectory))
        {
            Directory.CreateDirectory(this.appDirectory);
        }
    }
    public IEnumerator WaitForMinimumAudioLengthToPass()
    {
        this.btnStopRecording.SetActive(false);
        this.btnStartRecording.SetActive(false);
        yield return new WaitForSeconds(1);
        this.btnStopRecording.SetActive(true);
    }
    public void EndAudioRecording()
    {
        this.btnStartRecording.SetActive(true);
    }
    private void SetProjectIndex()
    {
        string projectIndexFilepath = this.appDirectory + "\\StoryProjectIndexData.txt";
        if (!File.Exists(projectIndexFilepath))
        {
            fileHandler.Write(projectIndexFilepath, "-1");
        }
        fileHandler.Read(projectIndexFilepath);
        this.projectID = int.Parse(fileHandler.contents);
    }
    private void CreateNewProject()
    {
        // first, get all current project IDs and find the maximum (the previous project's projectID)
        List<int> projectIDs = new List<int>();
        foreach (DirectoryInfo projectDirectoryInfo in new DirectoryInfo(this.appDirectory).GetDirectories())
        {
            projectIDs.Add(int.Parse(projectDirectoryInfo.Name));
        }
        int previousProjectID;
        if (projectIDs.Count > 0)
        {
            previousProjectID = projectIDs.Max();
            this.projectID = previousProjectID + 1;
        }
        else
        {
            this.projectID = 0;
        }
        this.projectDirectory = this.appDirectory + "\\" + this.projectID.ToString() + "\\"; // needed as projectID has changed from -1 to the projectID of the new project
        fileHandler.Write(this.appDirectory + "\\StoryProjectIndexData.txt", this.projectID.ToString());
        // now, create the initial project directory
        Directory.CreateDirectory(this.projectDirectory);
        string firstFrameDirectory = this.projectDirectory + "\\0";
        Directory.CreateDirectory(firstFrameDirectory);
        ItemCollectionFileData emptyItemCollectionFileData = new ItemCollectionFileData();
        emptyItemCollectionFileData.characterFileDatas = new List<CharacterFileData>();
        emptyItemCollectionFileData.propFileDatas = new List<PropFileData>();
        string json = JsonUtility.ToJson(emptyItemCollectionFileData);
        string filepath = firstFrameDirectory + "\\ItemCollectionData.json";
        fileHandler.Write(filepath, json);
        AudioClip newAudioClip = AudioClip.Create("NewAudioClip", 44100, 2, 44100, false);
        SavWav.Save(firstFrameDirectory + "\\VoiceActingAudioClip.wav", newAudioClip);
        fileHandler.Write(firstFrameDirectory + "\\BackgroundImageData.txt", "0"); // sets default background image
        fileHandler.Write(this.projectDirectory+"StoryNameData.txt", "My Project ");
    }
    public void Load()
    {
        this.SetProjectIndex();
        this.projectDirectory = this.appDirectory + "\\" + this.projectID.ToString() + "\\";
        if (this.projectID == -1)
        {
            this.CreateNewProject(); // also sets projectID to the next integer for a project e.g. if 4 existing projects, this will be the 5th project
        }
        GameObject.Find("FrameIdentifierText").GetComponent<Text>().text = "Frame 1";
        this.LoadAllFrames();
        this.ToggleRemoveFrameButton();
        this.ToggleMoveCharacterButton();
    }
    private void ToggleRemoveFrameButton()
    {
        if (this.frames.Count > 1)
        {
            this.btnRemoveFrame.SetActive(true);
        }
        else
        {
            this.btnRemoveFrame.SetActive(false);
        }
    }
    private void ToggleMoveCharacterButton()
    {
        if (this.frames[this.currentFrameIndex].GetComponent<FrameScript>().GetAllCharacters().Count > 0 || this.frames[this.currentFrameIndex].GetComponent<FrameScript>().GetAllProps().Count > 0)
        {
            this.btnMoveCharacter.SetActive(true);
            this.btnRemoveCharacter.SetActive(true);
            this.btnResizeRotate.SetActive(true);
            this.btnLiveMovement.SetActive(true);
            this.btnChangeSprite.SetActive(true);
        }
        else
        {
            this.btnRemoveCharacter.SetActive(false);
            this.btnMoveCharacter.SetActive(false);
            this.btnResizeRotate.SetActive(false);
            this.btnLiveMovement.SetActive(false);
            this.btnChangeSprite.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        fileHandler = GameObject.Find("Scripts").GetComponent<FileHandler>();
        this.currentFrameIndex = 0;
        removing = false;
        moves = new List<GameObject>();
        this.backgroundImageIndex = -1;
        this.appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DanielleApps");
        this.SetupApp();
        this.btnStopRecording.SetActive(false);
        Load();
    }
    private GameObject GetProp(string propID, int frameID, DirectoryInfo propDirectory)
    {
        // if I ever reference this I need to change to JSON file handling
        int propIntegerID = int.Parse(propID.Replace("P", ""));
        fileHandler.ReadLines(propDirectory.FullName + "\\PropData.txt");
        string[] propData = fileHandler.lines;
        float x = float.Parse(propData[0].Split('\n')[0]);
        float y = float.Parse(propData[1].Split('\n')[0]);
        float scale = float.Parse(propData[2].Split('\n')[0]);
        float rotation = float.Parse(propData[3]);
        string propName = GameObject.Find("PropManager").GetComponent<PropManagerScript>().DuplicateProp(frameID, propIntegerID, x, y, scale, rotation);
        return GameObject.Find(propName);
        // will probably need to make the program refresh the props to clear unneeded props when switching frames
    }
    private void GetFrameCharacters(string frameDirectory, int frameID)
    {
        List<GameObject> currentCharacters = new List<GameObject>();
        List<GameObject> currentProps = new List<GameObject>();
        fileHandler.Read(frameDirectory+"\\ItemCollectionData.json");
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
        foreach (CharacterFileData characterFileData in itemCollectionFileData.characterFileDatas)
        {
            String characterName = GameObject.Find("CharacterManager").GetComponent<CharacterManagerScript>().DuplicateCharacter(frameID, characterFileData.id, characterFileData.sprite, characterFileData.x, characterFileData.y, characterFileData.scale, characterFileData.rotation);
            currentCharacters.Add(GameObject.Find(characterName));
            ItemMoveData itemMoveData = characterFileData.itemMoveData;
            string moveName = moveManager.GetComponent<MoveManagerScript>().GetMoveByID(itemMoveData.id);
            moves.Add(GameObject.Find(moveName));
        }
        foreach (PropFileData propFileData in itemCollectionFileData.propFileDatas)
        {
            String propName = GameObject.Find("PropManager").GetComponent<PropManagerScript>().DuplicateProp(frameID, propFileData.id, propFileData.x, propFileData.y, propFileData.scale, propFileData.rotation);
            currentProps.Add(GameObject.Find(propName));
            ItemMoveData itemMoveData = propFileData.itemMoveData;
            string moveName = moveManager.GetComponent<MoveManagerScript>().GetMoveByID(itemMoveData.id);
            moves.Add(GameObject.Find(moveName));
        }
        this.characters = currentCharacters;
        this.props = currentProps;
    }
    private void GetBackgroundImageIndex(string frameDirectory)
    {
        fileHandler.Read(frameDirectory + "\\BackgroundImageData.txt");
        this.backgroundImageIndex = int.Parse(fileHandler.contents);
    }
    public int GetBackgroundImageIndex2(string frameID)
    {
        fileHandler.Read(this.projectDirectory + "\\" + frameID + "\\BackgroundImageData.txt");
        return int.Parse(fileHandler.contents); 
    }
    public void StopRemovingFrame()
    {
        removing = false;
    }
    public void StopDuplicatingFrame()
    {
        copyingFrame = false;
    }
    public bool CharacterExists(GameObject character)
    {
        string filepath = this.projectDirectory + "\\" + this.currentFrameIndex.ToString() + "\\ItemCollectionData.json";
        fileHandler.Read(filepath);
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
        foreach (CharacterFileData characterFileData in itemCollectionFileData.characterFileDatas)
        {
            if (characterFileData.id == character.GetComponent<CharacterScript>().GetCharacterID())
            {
                return true;
            }
        }
        return false;
    }
    public bool PropExists(GameObject prop)
    {
        string filepath = this.projectDirectory + "\\" + this.currentFrameIndex.ToString() + "\\ItemCollectionData.json";
        fileHandler.Read(filepath);
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
        foreach (PropFileData propFileData in itemCollectionFileData.propFileDatas)
        {
            if (propFileData.id == prop.GetComponent<PropScript>().GetPropID())
            {
                return true;
            }
        }
        return false;
    }
    private string GetCharacterIDFromName(string name)
    {
        return new string(name.Where(c => char.IsDigit(c)).ToArray());
    }
    private string RenameCharacterInstance(string name, int frameID)
    {
        name = name.Replace("(Clone)", "");
        char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        foreach (char digit in digits)
        {
            name = name.Replace(digit.ToString(), "");
        }
        name = name + frameID.ToString();
        return name;
    }
    private IEnumerator PlayItemMovement(GameObject item)
    {
        float elapsedTime = 0;
        float seconds = 1;
        Vector3 startPosition = item.transform.position;
        Vector3 endPosition = startPosition;
        if (item.GetComponent<CharacterScript>())
        {
            GameObject move = item.GetComponent<CharacterScript>().GetMove();
            if (move == null)
            {
                yield return null;
            }
            if (move.GetComponent<MoveScript>().IsDelayed())
            {
                yield return new WaitForSeconds(1);
            }
            endPosition = startPosition + new Vector3(move.GetComponent<MoveScript>().x, move.GetComponent<MoveScript>().y, 0);
        }
        else if (item.GetComponent<PropScript>())
        {
            GameObject move = item.GetComponent<PropScript>().GetMove();
            if (move.GetComponent<MoveScript>().IsDelayed())
            {
                yield return new WaitForSeconds(1);
            }
            endPosition = startPosition + new Vector3(move.GetComponent<MoveScript>().x, move.GetComponent<MoveScript>().y, 0);
        }
        while (elapsedTime < seconds)
        {
            item.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        item.transform.position = endPosition;
    }
    private void PlayAllItemMovements()
    {
        foreach (GameObject character in this.characters)
        {
            StartCoroutine(PlayItemMovement(character));
        }
        foreach (GameObject prop in this.props)
        {
            StartCoroutine(PlayItemMovement(prop));
        }
    }
    private IEnumerator WaitForAudioToFinish(int i)
    {
        this.currentFrameIndex = i;
        this.DisplayFrame(i, false, false);
        this.PlayFrameAudio();
        GetFrameCharacters(this.projectDirectory + "\\" + i.ToString(), i);
        PlayAllItemMovements();
        yield return new WaitUntil(() => this.frames[this.currentFrameIndex].GetComponent<FrameScript>().length != 0);
        yield return new WaitForSeconds(this.frames[this.currentFrameIndex].GetComponent<FrameScript>().length);
        if (i < this.frames.Count-1)
        {
            yield return StartCoroutine(WaitForAudioToFinish(i + 1));
        }
        else
        {
            this.currentFrameIndex = 0;
            this.ResetItemPositions();
            this.DisplayFrame(0, false, false);
            this.gameObject.GetComponent<VideoPlayerScript>().ToggleUI(true);
        }
    }
    public void WalkthroughFrames()
    {
        StartCoroutine(WaitForAudioToFinish(0));
    }
    public void PlayFrameAudio()
    {
        this.frames[this.currentFrameIndex].GetComponent<FrameScript>().PlayAudio();
    }
    public void StopRecording()
    {
        this.frames[this.currentFrameIndex].GetComponent<FrameScript>().EndVoiceActingAudioClipRecording();
        this.btnStopRecording.SetActive(false);
    }

    public void RecordVoice()
    {
        this.frames[this.currentFrameIndex].GetComponent<FrameScript>().RecordVoiceActingAudioClip();
    }

    public void SaveNewFrameToProject(bool isDuplicate=false)
    {
        if (copyingFrame)
        {
            return;
        }
        copyingFrame = true;
        GameObject frame = Instantiate(this.frameTemplate);
        int newFrameID = this.frames.Count;
        int frameIndex = this.currentFrameIndex;
        int previousImageIndex = frames[this.currentFrameIndex].GetComponent<FrameScript>().GetBackgroundImageIndex();
        frame.name = newFrameID.ToString();
        ItemCollectionFileData itemCollectionFileData = new ItemCollectionFileData();
        itemCollectionFileData.characterFileDatas = new List<CharacterFileData>();
        itemCollectionFileData.propFileDatas = new List<PropFileData>();
        if (isDuplicate)
        {
            List<GameObject> characters = this.frames[frameIndex].GetComponent<FrameScript>().GetAllCharacters();
            List<GameObject> props = this.frames[frameIndex].GetComponent<FrameScript>().GetAllProps();
            DirectoryInfo frameDirectory = new DirectoryInfo(this.projectDirectory + "\\" + newFrameID);
            List<GameObject> newCharacters = new List<GameObject>();
            List<GameObject> newProps = new List<GameObject>();
            foreach (GameObject characterToCopy in characters)
            {
                GameObject character = Instantiate(characterToCopy);
                character.GetComponent<CharacterScript>().SetCharacter(characterToCopy.GetComponent<CharacterScript>().GetCharacterID(), characterToCopy.GetComponent<CharacterScript>().GetCurrentSpriteIndex(), characterToCopy.GetComponent<CharacterScript>().GetXAxisPosition(), characterToCopy.GetComponent<CharacterScript>().GetYAxisPosition(), characterToCopy.GetComponent<CharacterScript>().GetScale(), characterToCopy.GetComponent<CharacterScript>().GetRotation(), characterToCopy.GetComponent<CharacterScript>().GetMove());
                character.name = this.RenameCharacterInstance(character.name, newFrameID);
                character.transform.SetParent(GameObject.Find("CharacterInstanceManager").transform, false);
                int characterID = character.GetComponent<CharacterScript>().GetCharacterID();
                int spriteIndex = character.GetComponent<CharacterScript>().GetCurrentSpriteIndex();
                float x = character.GetComponent<CharacterScript>().GetXAxisPosition();
                float y = character.GetComponent<CharacterScript>().GetYAxisPosition();
                float scale = character.GetComponent<CharacterScript>().GetScale();
                float rotation = character.GetComponent<CharacterScript>().GetRotation();
                CharacterFileData characterFileData = new CharacterFileData();
                characterFileData.id = characterID;
                characterFileData.sprite = spriteIndex;
                characterFileData.x = x;
                characterFileData.y = y;
                characterFileData.scale = scale;
                characterFileData.rotation = rotation;
                characterFileData.itemMoveData = new ItemMoveData();
                characterFileData.itemMoveData.id = character.GetComponent<CharacterScript>().GetMove().GetComponent<MoveScript>().GetMoveID();
                characterFileData.itemMoveData.x = character.GetComponent<CharacterScript>().GetMove().GetComponent<MoveScript>().GetX();
                characterFileData.itemMoveData.y = character.GetComponent<CharacterScript>().GetMove().GetComponent<MoveScript>().GetY();
                characterFileData.itemMoveData.delayed = character.GetComponent<CharacterScript>().GetMove().GetComponent<MoveScript>().IsDelayed();
                itemCollectionFileData.characterFileDatas.Add(characterFileData);
                //Directory.CreateDirectory(frameDirectory.FullName + "\\" + characterID.ToString());
                //string characterDataPath = frameDirectory.FullName + "\\" + characterID.ToString() + "\\CharacterData.txt";
                //string contents = spriteIndex.ToString() + "\n" + x.ToString() + "\n" + y.ToString() + "\n" + scale + "\n" + rotation;
                //fileHandler.Write(characterDataPath, contents);
                newCharacters.Add(character);
            }
            foreach (GameObject propToCopy in props)
            {
                GameObject prop = Instantiate(propToCopy);
                prop.GetComponent<PropScript>().SetProp(propToCopy.GetComponent<PropScript>().GetPropID(), propToCopy.GetComponent<PropScript>().GetXAxisPosition(), propToCopy.GetComponent<PropScript>().GetYAxisPosition(), propToCopy.GetComponent<PropScript>().GetScale(), propToCopy.GetComponent<PropScript>().GetRotation(), propToCopy.GetComponent<PropScript>().GetMove());
                prop.name = this.RenameCharacterInstance(prop.name, newFrameID);
                prop.transform.SetParent(GameObject.Find("PropInstanceManager").transform, false);
                int propID = prop.GetComponent<PropScript>().GetPropID();
                float x = prop.GetComponent<PropScript>().GetXAxisPosition();
                float y = prop.GetComponent<PropScript>().GetYAxisPosition();
                float scale = prop.GetComponent<PropScript>().GetScale();
                float rotation = prop.GetComponent<PropScript>().GetRotation();
                PropFileData propFileData = new PropFileData();
                propFileData.id = propID;
                propFileData.x = x;
                propFileData.y = y;
                propFileData.scale = scale;
                propFileData.rotation = rotation;
                propFileData.itemMoveData = new ItemMoveData();
                propFileData.itemMoveData.id = prop.GetComponent<PropScript>().GetMove().GetComponent<MoveScript>().GetMoveID();
                propFileData.itemMoveData.x = prop.GetComponent<PropScript>().GetMove().GetComponent<MoveScript>().GetX();
                propFileData.itemMoveData.y = prop.GetComponent<PropScript>().GetMove().GetComponent<MoveScript>().GetY();
                propFileData.itemMoveData.delayed = prop.GetComponent<PropScript>().GetMove().GetComponent<MoveScript>().IsDelayed();
                itemCollectionFileData.propFileDatas.Add(propFileData);
                //Directory.CreateDirectory(frameDirectory.FullName + "\\P" + propID.ToString());
                //string propDataPath = frameDirectory.FullName + "\\P" + propID.ToString() + "\\PropData.txt";
                //string contents = x.ToString() + "\n" + y.ToString() + "\n" + scale + rotation;
                //fileHandler.Write(propDataPath, contents);
                newProps.Add(prop);
            }
            frame.GetComponent<FrameScript>().SetFrameData(this.projectID, newFrameID, previousImageIndex, newCharacters, newProps);
        }
        else
        {
            frame.GetComponent<FrameScript>().SetFrameData(this.projectID, newFrameID, previousImageIndex, new List<GameObject>(), new List<GameObject>());
        }
        this.frames.Add(frame);
        frame.transform.SetParent(GameObject.Find("FrameContainer").transform, false);
        this.currentFrameIndex++;
        Directory.CreateDirectory(this.projectDirectory + "\\" + newFrameID.ToString());
        string filepath = this.projectDirectory + "\\" + newFrameID.ToString() + "\\BackgroundImageData.txt";
        fileHandler.Write(filepath, previousImageIndex.ToString());
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(projectDirectory + newFrameID.ToString() + "\\ItemCollectionData.json", json);
        this.DisplayFrame(this.frames.Count-1);
        this.ToggleRemoveFrameButton();
        BubbleSortFrames();
    }
    public void DuplicateFrame()
    {
        SaveNewFrameToProject(true);
    }
    public void SaveCharacterToFrame(GameObject character)
    {
        this.frames[this.currentFrameIndex].GetComponent<FrameScript>().AddCharacter(character);
        DirectoryInfo frameDirectory = new DirectoryInfo(this.projectDirectory + frames[this.currentFrameIndex].GetComponent<FrameScript>().GetFrameID().ToString());
        string filepath = frameDirectory + "\\ItemCollectionData.json";
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(File.ReadAllText(filepath));
        CharacterFileData characterFileData = new CharacterFileData();
        characterFileData.id = character.GetComponent<CharacterScript>().GetCharacterID();
        characterFileData.sprite = character.GetComponent<CharacterScript>().GetCurrentSpriteIndex();
        characterFileData.x = character.GetComponent<CharacterScript>().GetXAxisPosition();
        characterFileData.y = character.GetComponent<CharacterScript>().GetYAxisPosition();
        characterFileData.scale = character.GetComponent<CharacterScript>().GetScale();
        characterFileData.rotation = character.GetComponent<CharacterScript>().GetRotation();
        itemCollectionFileData.characterFileDatas.Add(characterFileData);
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(filepath, json);
        this.ToggleMoveCharacterButton();
    }
    public void SavePropToFrame(GameObject prop)
    {
        this.frames[this.currentFrameIndex].GetComponent<FrameScript>().AddProp(prop);
        DirectoryInfo frameDirectory = new DirectoryInfo(this.projectDirectory + frames[this.currentFrameIndex].GetComponent<FrameScript>().GetFrameID().ToString());
        //string propDirectory = frameDirectory + "\\P" + prop.GetComponent<PropScript>().GetPropID().ToString();
        //Directory.CreateDirectory(propDirectory);
        string filepath = frameDirectory + "\\ItemCollectionData.json";
        fileHandler.Read(filepath);
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
        PropFileData propFileData = new PropFileData();
        propFileData.id = prop.GetComponent<PropScript>().GetPropID();
        propFileData.x = 300;
        propFileData.y = 300;
        propFileData.scale = 1;
        propFileData.rotation = 0;
        itemCollectionFileData.propFileDatas.Add(propFileData);
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(filepath, json);
        this.ToggleMoveCharacterButton();
    }
    private void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            fi.MoveTo(Path.Combine(target.FullName, fi.Name));
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = source.CreateSubdirectory(diSourceSubDir.Name+"_v");
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }
    private void DeleteTemporaryFrameFolders(int frameID)
    {
        DirectoryInfo source = new DirectoryInfo(this.projectDirectory+"\\"+frameID.ToString());
        // Copy each file into the new directory.

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            if (diSourceSubDir.Name.Contains("_v"))
            {
                foreach (FileInfo fi in source.GetFiles())
                {
                    fi.Delete();
                }
                foreach (FileInfo fi2 in diSourceSubDir.GetFiles())
                {
                    fi2.Delete();
                }
                diSourceSubDir.Delete();
            }
        }
    }
    //private void CopyContentsOfFrameDirectories(DirectoryInfo frameDirectoryInfo, DirectoryInfo newFrameDirectoryInfo)
    //{
    //    if (Directory.Exists(newFrameDirectoryInfo.FullName))
    //    {
    //        int newFrameDirectoryIndex = int.Parse(newFrameDirectoryInfo.Name) - 1;
    //        if (newFrameDirectoryIndex == -1)
    //        {
    //            newFrameDirectoryIndex = 0;
    //        }
    //        CopyContentsOfFrameDirectories(newFrameDirectoryInfo, new DirectoryInfo(newFrameDirectoryIndex.ToString()));
    //        foreach (FileInfo fileInfo in frameDirectoryInfo.GetFiles())
    //        {
    //            fileInfo.CopyTo(Path.Combine(newFrameDirectoryInfo.FullName, fileInfo.Name), true);
    //        }
    //        foreach (DirectoryInfo directoryInfo in frameDirectoryInfo.GetDirectories())
    //        {
    //            DirectoryInfo newDirectoryInfo = newFrameDirectoryInfo.CreateSubdirectory(directoryInfo.Name);
    //            CopyAll(directoryInfo, newDirectoryInfo);
    //        }
    //    }
    //}
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the source directory does not exist, throw an exception.
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        // If the destination directory does not exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Debug.Log("Directory " + destDirName + " does not exist so creating it");
            Directory.CreateDirectory(destDirName);
        }


        // Get the file contents of the directory to copy.
        FileInfo[] files = dir.GetFiles();

        foreach (FileInfo file in files)
        {
            // Create the path to the new copy of the file.
            string temppath = Path.Combine(destDirName, file.Name);
            // Copy the file.
            file.CopyTo(temppath, true);
            file.Delete();
        }
        // If copySubDirs is true, copy the subdirectories.
        if (copySubDirs)
        {

            foreach (DirectoryInfo subdir in dirs)
            {
                // Create the subdirectory.
                string temppath = Path.Combine(destDirName, subdir.Name);

                // Copy the subdirectories.
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
        Directory.Delete(sourceDirName, true);
    }
    private void RenumberFramesInFiles(int frameID, int newFrameID)
    {
        string frameIDString = frameID.ToString();
        string newFrameIDString = newFrameID.ToString();
        Directory.Move(this.projectDirectory + "\\" + frameIDString, this.projectDirectory + "\\" + newFrameIDString);
    }
    private void RenumberFrames(int removedFrameIndex)
    {
        foreach (GameObject frame in this.frames)
        {
            DirectoryInfo frameDirectory = new DirectoryInfo(this.projectDirectory+"\\"+frame.name);
            int frameNameAsInt = int.Parse(frameDirectory.Name);
            if (frameNameAsInt == removedFrameIndex)
            {
                frameDirectory.Delete(true);
            }
            else if (frameNameAsInt > removedFrameIndex)
            {
                int newFrameID = frameNameAsInt - 1;
                frameDirectory.MoveTo(this.projectDirectory + "\\" + newFrameID.ToString());
            }
        }
    }
    public void RemoveFrame()
    {
        if (removing)
        {
            return;
        }
        removing = true;
        RenumberFrames(this.currentFrameIndex);
        SceneManager.LoadScene("MainScene");
        //}
        if (currentFrameIndex >= this.frames.Count)
        {
            currentFrameIndex = 0;
        }
        else if (currentFrameIndex > 0)
        {
            currentFrameIndex--;
        }
        // otherwise do nothing as frame index is 0 so should stay as 0
        this.ToggleRemoveFrameButton();
        BubbleSortFrames();

    }
    public void FileOnlyRemoveCharacter(GameObject character)
    {
        this.SaveProject();
        string filepath = this.projectDirectory + "\\" + this.currentFrameIndex.ToString() + "\\ItemCollectionData.json";
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(File.ReadAllText(filepath));
        if (character.GetComponent<CharacterScript>())
        {
            //this.frames[this.currentFrameIndex].GetComponent<FrameScript>().RemoveCharacterByID(character.GetComponent<CharacterScript>().GetCharacterID());
            for (int i = 0; i < itemCollectionFileData.characterFileDatas.Count; i++)
            {
                CharacterFileData characterFileData = itemCollectionFileData.characterFileDatas[i];
                if (characterFileData.id == character.GetComponent<CharacterScript>().GetCharacterID())
                {
                    itemCollectionFileData.characterFileDatas.RemoveAt(i);
                }
            }
            //File.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\" + character.GetComponent<CharacterScript>().GetCharacterID() + "\\CharacterData.txt");
            //Directory.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\" + character.GetComponent<CharacterScript>().GetCharacterID());
        }
        else if (character.GetComponent<PropScript>())
        {
            //this.frames[this.currentFrameIndex].GetComponent<FrameScript>().RemovePropByID(character.GetComponent<PropScript>().GetPropID());
            for (int i = 0; i < itemCollectionFileData.propFileDatas.Count; i++)
            {
                PropFileData propFileData = itemCollectionFileData.propFileDatas[i];
                if (propFileData.id == character.GetComponent<PropScript>().GetPropID())
                {
                    itemCollectionFileData.propFileDatas.RemoveAt(i);
                }
            }
            //File.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\P" + character.GetComponent<PropScript>().GetPropID() + "\\PropData.txt");
            //Directory.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\P" + character.GetComponent<PropScript>().GetPropID());
        }
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(filepath, json);
        this.ToggleMoveCharacterButton();
    }
    public void RemoveCharacter(GameObject character)
    {
        this.SaveProject();
        string filepath = this.projectDirectory + "\\" + this.currentFrameIndex.ToString() + "\\ItemCollectionData.json";
        ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(File.ReadAllText(filepath));
        if (character.GetComponent<CharacterScript>())
        {
            this.frames[this.currentFrameIndex].GetComponent<FrameScript>().RemoveCharacterByID(character.GetComponent<CharacterScript>().GetCharacterID());
            for (int i = 0; i < itemCollectionFileData.characterFileDatas.Count; i++)
            {
                CharacterFileData characterFileData = itemCollectionFileData.characterFileDatas[i];
                if (characterFileData.id == character.GetComponent<CharacterScript>().GetCharacterID())
                {
                    itemCollectionFileData.characterFileDatas.RemoveAt(i);
                }
            }
            //File.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\" + character.GetComponent<CharacterScript>().GetCharacterID() + "\\CharacterData.txt");
            //Directory.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\" + character.GetComponent<CharacterScript>().GetCharacterID());
        }
        else if (character.GetComponent<PropScript>())
        {
            this.frames[this.currentFrameIndex].GetComponent<FrameScript>().RemovePropByID(character.GetComponent<PropScript>().GetPropID());
            for (int i = 0; i < itemCollectionFileData.propFileDatas.Count; i++)
            {
                PropFileData propFileData = itemCollectionFileData.propFileDatas[i];
                if (propFileData.id == character.GetComponent<PropScript>().GetPropID())
                {
                    itemCollectionFileData.propFileDatas.RemoveAt(i);
                }
            }
            //File.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\P" + character.GetComponent<PropScript>().GetPropID() + "\\PropData.txt");
            //Directory.Delete(this.projectDirectory + "\\" + this.currentFrameIndex + "\\P" + character.GetComponent<PropScript>().GetPropID());
        }
        string json = JsonUtility.ToJson(itemCollectionFileData);
        fileHandler.Write(filepath, json);
        this.ToggleMoveCharacterButton();
    }
    private GameObject GetMatchingRuntimeCharacter(int frameID, int characterID)
    {
        foreach (GameObject character in this.frames[frameID].GetComponent<FrameScript>().GetAllCharacters())
        {
            if (character.GetComponent<CharacterScript>().GetCharacterID() == characterID)
            {
                return character;
            }
        }
        return null;
    }
    private GameObject GetMatchingRuntimeProp(int frameID, int propID)
    {
        foreach (GameObject prop in this.frames[frameID].GetComponent<FrameScript>().GetAllProps())
        {
            if (prop.GetComponent<PropScript>().GetPropID() == propID)
            {
                return prop;
            }
        }
        return null;
    }
    private void ResetItemPositions()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            string filepath = this.projectDirectory + "\\" + i.ToString() + "\\ItemCollectionData.json";
            fileHandler.Read(filepath);
            ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
            foreach (CharacterFileData characterFileData in itemCollectionFileData.characterFileDatas)
            {
                GameObject character = GetMatchingRuntimeCharacter(i, characterFileData.id);
                if (character != null)
                {
                    character.GetComponent<CharacterScript>().SetCharacterPosition(characterFileData.x, characterFileData.y);
                }
            }
            foreach (PropFileData propFileData in itemCollectionFileData.propFileDatas)
            {
                GameObject prop = GetMatchingRuntimeProp(i, propFileData.id);
                if (prop != null)
                {
                    prop.GetComponent<PropScript>().SetPropPosition(propFileData.x, propFileData.y);
                }
            }
        }
    }
    public void LoadAllFrames(bool refreshOnly=false)
    {
        if (!refreshOnly)
        {
            this.frames = new List<GameObject>();
        }
        int index = 0;
        foreach (DirectoryInfo frameDirectory in new DirectoryInfo(this.projectDirectory).GetDirectories())
        {
            List<GameObject> characters = new List<GameObject>();
            List<GameObject> props = new List<GameObject>();
            int frameID = int.Parse(frameDirectory.Name);
            GetBackgroundImageIndex(frameDirectory.FullName);
            // add the frame data
            frameTemplate.name = frameID.ToString();
            GameObject frame = Instantiate(frameTemplate);
            frame.name = frameID.ToString();
            frame.transform.SetParent(GameObject.Find("FrameContainer").transform, false);
            frame.transform.localPosition = new Vector3(0, 0, 0);
            this.frames.Add(frame);
            this.frames[index].GetComponent<FrameScript>().SetBasicFrameData(projectID, frameID);
            string filepath = frameDirectory + "\\ItemCollectionData.json";
            fileHandler.Read(filepath);
            ItemCollectionFileData itemCollectionFileData = JsonUtility.FromJson<ItemCollectionFileData>(fileHandler.contents);
            foreach (CharacterFileData characterFileData in itemCollectionFileData.characterFileDatas)
            {
                string characterName = characterManager.GetComponent<CharacterManagerScript>().DuplicateCharacter(frameID, characterFileData.id, characterFileData.sprite, characterFileData.x, characterFileData.y, characterFileData.scale, characterFileData.rotation);
                characters.Add(GameObject.Find(characterName));
                ItemMoveData itemMoveData = characterFileData.itemMoveData;
                string moveName = moveManager.GetComponent<MoveManagerScript>().GetMoveByID(itemMoveData.id);
                GameObject.Find(characterName).GetComponent<CharacterScript>().AttachMove(GameObject.Find(moveName));
                moves.Add(GameObject.Find(moveName));
            }
            foreach (PropFileData propFileData in itemCollectionFileData.propFileDatas)
            {
                string propName = propManager.GetComponent<PropManagerScript>().DuplicateProp(frameID, propFileData.id, propFileData.x, propFileData.y, propFileData.scale, propFileData.rotation);
                props.Add(GameObject.Find(propName));
                ItemMoveData itemMoveData = propFileData.itemMoveData;
                string moveName = moveManager.GetComponent<MoveManagerScript>().GetMoveByID(itemMoveData.id);
                GameObject.Find(propName).GetComponent<PropScript>().AttachMove(GameObject.Find(moveName));
                moves.Add(GameObject.Find(moveName));
            }
            this.frames[index].GetComponent<FrameScript>().SetFrameData(this.projectID, frameID, backgroundImageIndex, characters, props);
            index++;
            if (!File.Exists(frameDirectory+ "\\VoiceActingAudioClip.wav"))
            {
                AudioClip newAudioClip = AudioClip.Create("NewAudioClip", 44100, 2, 44100, false);
                SavWav.Save(frameDirectory + "\\VoiceActingAudioClip.wav", newAudioClip);
            }

        }
        this.DisplayFrame(0, false, false);
    }
    public int GetCurrentFrameIndex()
    {
        return this.currentFrameIndex;
    }
    public void UpdateCurrentFrameBackgroundImage(int backgroundImageIndex)
    {
        this.frames[currentFrameIndex].GetComponent<FrameScript>().SetBackgroundImage(backgroundImageIndex);
        this.SaveProject();
    }
    public void DisplayFrame(int frameID, bool save=true, bool refresh=false)
    {  
        if (save)
        {
            SaveProject();
        }
        this.currentFrameIndex = frameID;
        GameObject.Find("SelectedFrame").GetComponent<Image>().sprite = null;
        foreach (GameObject frame in this.frames)
        {
            if (frame.name == frameID.ToString())
            {
                frame.SetActive(true);
                frame.GetComponent<FrameScript>().DisplayBackgroundImage();
                foreach (GameObject character in frame.GetComponent<FrameScript>().GetAllCharacters())
                {
                    character.SetActive(true);
                    character.GetComponent<CharacterScript>().DisplayCharacter();
                }
                foreach (GameObject prop in frame.GetComponent<FrameScript>().GetAllProps())
                {
                    prop.SetActive(true);
                    prop.GetComponent<PropScript>().DisplayProp();
                }
            }
            else
            {
                foreach (GameObject character in frame.GetComponent<FrameScript>().GetAllCharacters())
                {
                    character.SetActive(false);
                }
                foreach (GameObject prop in frame.GetComponent<FrameScript>().GetAllProps())
                {
                    prop.SetActive(false);
                }
                frame.SetActive(false);
            }
        }
        this.ToggleMoveCharacterButton();
        BubbleSortFrames();
        if (GameObject.Find("FrameIdentifierText") != null)
        {
            GameObject.Find("FrameIdentifierText").GetComponent<Text>().text = "Frame " + (this.currentFrameIndex + 1).ToString();
        }
    }
    public void SaveProject()
    {
        int savedFrameIndex = this.currentFrameIndex;
        foreach (GameObject frame in this.frames)
        {
            int frameID = frame.GetComponent<FrameScript>().GetFrameID();
            DirectoryInfo frameDirectory = new DirectoryInfo(this.projectDirectory + frame.gameObject.name.ToString());
            int backgroundImageIndex = frame.GetComponent<FrameScript>().GetBackgroundImageIndex();
            fileHandler.Write(frameDirectory.FullName + "\\BackgroundImageData.txt", backgroundImageIndex.ToString());
            this.DisplayFrame(frameID, false, false);
            GetFrameCharacters(frameDirectory.FullName, frameID);
            //foreach (DirectoryInfo directoryInfo in frameDirectory.GetDirectories())
            //{
            //    foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            //    {
            //        fileInfo.Delete();
            //    }
            //    directoryInfo.Delete(); // deletes previously stored character data in case, for example, a character has been replaced
            //}
            string filepath = frameDirectory + "\\ItemCollectionData.json";
            ItemCollectionFileData itemCollectionFileData = new ItemCollectionFileData();
            itemCollectionFileData.characterFileDatas = new List<CharacterFileData>();
            itemCollectionFileData.propFileDatas = new List<PropFileData>();
            foreach (GameObject character in characters)
            {
                character.transform.SetParent(GameObject.Find("CharacterInstanceManager").transform, true);
                CharacterFileData characterFileData = new CharacterFileData();
                characterFileData.id = character.GetComponent<CharacterScript>().GetCharacterID();
                characterFileData.sprite = character.GetComponent<CharacterScript>().GetCurrentSpriteIndex();
                characterFileData.x = character.GetComponent<CharacterScript>().GetXAxisPosition();
                characterFileData.y = character.GetComponent<CharacterScript>().GetYAxisPosition();
                characterFileData.scale = character.GetComponent<CharacterScript>().GetScale();
                characterFileData.rotation = character.GetComponent<CharacterScript>().GetRotation();
                characterFileData.itemMoveData = new ItemMoveData();
                MoveScript moveScript = character.GetComponent<CharacterScript>().GetMove().GetComponent<MoveScript>();
                characterFileData.itemMoveData.id = moveScript.moveID;
                characterFileData.itemMoveData.x = moveScript.x;
                characterFileData.itemMoveData.y = moveScript.y;
                characterFileData.itemMoveData.delayed = moveScript.delayed;
                itemCollectionFileData.characterFileDatas.Add(characterFileData);
            }
            foreach (GameObject prop in props)
            {
                PropFileData propFileData = new PropFileData();
                prop.transform.SetParent(GameObject.Find("PropInstanceManager").transform, true);
                propFileData.id = prop.GetComponent<PropScript>().GetPropID();
                propFileData.x = prop.GetComponent<PropScript>().GetXAxisPosition();
                propFileData.y = prop.GetComponent<PropScript>().GetYAxisPosition();
                propFileData.scale = prop.GetComponent<PropScript>().GetScale();
                propFileData.rotation = prop.GetComponent<PropScript>().GetRotation();
                propFileData.itemMoveData = new ItemMoveData();
                MoveScript moveScript = prop.GetComponent<PropScript>().GetMove().GetComponent<MoveScript>();
                propFileData.itemMoveData.id = moveScript.moveID;
                propFileData.itemMoveData.x = moveScript.x;
                propFileData.itemMoveData.y = moveScript.y;
                propFileData.itemMoveData.delayed = moveScript.delayed;
                itemCollectionFileData.propFileDatas.Add(propFileData);
            }
            string json = JsonUtility.ToJson(itemCollectionFileData);
            fileHandler.Write(filepath, json);
            DisplayFrame(savedFrameIndex, false, false);
        }
    }
    // Update is called once per frame
    private void SwapFrames(int x, int y)
    {
        GameObject temp = frames[x];
        frames[x] = frames[y];
        frames[y] = temp;
    }
    private void BubbleSortFrames()
    {
        // keep frames in order in memory using a bubble sort
        int i;
        int j;
        for (i = 0; i < frames.Count - 1; i++)
        {
            for (j = 0; j < frames.Count - i - 1; j++)
            {
                if (int.Parse(frames[j].name) > int.Parse(frames[j + 1].name))
                {
                    SwapFrames(j, j + 1);
                }
            }
        }
    }
    void Update()
    {
        
    }
}
