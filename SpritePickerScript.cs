using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePickerScript : MonoBehaviour
{
    private List<GameObject> spriteButtons;
    private GameObject character;
    public GameObject characterManager;
    public GameObject templateSpriteButton;
    // Start is called before the first frame update
    void Start()
    {
    }
    void OnEnable()
    {

        this.spriteButtons = new List<GameObject>();
    }
    public void LoadSprites(GameObject character)
    {
        this.spriteButtons = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        this.character = character;
        if (this.character != null)
        {
            this.character = character;
            int index = 0;
            List<Sprite> sprites = character.GetComponent<CharacterScript>().sprites;
            foreach (Sprite sprite in sprites)
            {
                GameObject spriteButton = Instantiate(templateSpriteButton);
                spriteButton.name = character.name + "_" + index.ToString();
                spriteButton.transform.SetParent(this.gameObject.transform, false);
                spriteButton.GetComponent<SpriteScript>().sprite = sprites[index];
                spriteButton.GetComponent<Image>().sprite = sprites[index];
                spriteButton.GetComponent<SpriteScript>().spriteIndex = index;
                this.spriteButtons.Add(spriteButton);
                index++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectSprite(int spriteIndex)
    {
        characterManager.GetComponent<CharacterManagerScript>().UpdateCharacterSprite(character, spriteIndex);
        GameObject.Find("SpritePicker").SetActive(false);
    }
}
