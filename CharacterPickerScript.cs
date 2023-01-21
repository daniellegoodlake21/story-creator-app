
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPickerScript : MonoBehaviour
{
    private List<GameObject> characterButtons;
    public GameObject characterManager;
    public GameObject templateCharacterButton;
    public GameObject projectCreator;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {

        this.characterButtons = new List<GameObject>();
    }
    public void LoadCharacters()
    {
        this.characterButtons = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        List<GameObject> characters = characterManager.GetComponent<CharacterManagerScript>().GetAllCharacters();
        foreach (GameObject character in characters)
        {
            GameObject characterButton = null;
            if (!projectCreator.GetComponent<StoryProjectScript>().CharacterExists(character))
            {
                characterButton = Instantiate(templateCharacterButton);
                characterButton.name = character.name + "Button";
                characterButton.transform.SetParent(this.gameObject.transform, false);
                characterButton.GetComponent<CharacterPickerCharacterScript>().SetCharacter(character);
                characterButton.GetComponent<Image>().sprite = character.GetComponent<CharacterScript>().sprites[0];
                this.characterButtons.Add(characterButton);
            }
                        if (character.name.EndsWith("Two"))
            {
                GameObject original = GameObject.Find(character.name.Substring(0, character.name.Length - 3));
                if (!projectCreator.GetComponent<StoryProjectScript>().CharacterExists(original))
                {
                    if (characterButton != null)
                    {
                        characterButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (characterButton != null)
                    {
                        characterButton.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void SelectCharacter(GameObject character)
    {
        GameObject newCharacter = Instantiate(character);
        newCharacter.GetComponent<CharacterScript>().SetCharacterID(character.GetComponent<CharacterScript>().GetCharacterID());
        newCharacter.name = character.name + "Instance" + projectCreator.GetComponent<StoryProjectScript>().GetCurrentFrameIndex().ToString();
        newCharacter.transform.SetParent(GameObject.Find("CharacterInstanceManager").transform, false);
        newCharacter.GetComponent<CharacterScript>().SetCharacterPosition(200, 200);
        projectCreator.GetComponent<StoryProjectScript>().SaveCharacterToFrame(newCharacter);
        GameObject.Find("CharacterPicker").SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
