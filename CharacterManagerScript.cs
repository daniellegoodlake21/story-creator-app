using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerScript : MonoBehaviour
{
    public List<GameObject> characters;
    // Start is called before the first frame update
    void Start()
    {
       for (int i = 0; i < characters.Count; i++)
        {
            characters[i].GetComponent<CharacterScript>().SetCharacterID(i);
        }
        this.gameObject.transform.SetParent(GameObject.Find("HUDCanvas").transform, true);
        this.gameObject.transform.SetSiblingIndex(2);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<GameObject> GetAllCharacters()
    {
        List<GameObject> characters = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.name != "CharacterInstanceManager")
            {
                GameObject character = child.gameObject;
                characters.Add(character);
            }
        }
        return characters;
    }
    public string DuplicateCharacter(int frameID, int characterID, int spriteIndex, float x, float y, float scale, float rotation)
    {
        GameObject character = null;
        GameObject found = GameObject.Find(this.characters[characterID].name + "Instance" + frameID.ToString());
        if (found)
        {
            character = found.gameObject;
            character.name = this.characters[characterID].name + "Instance"+frameID.ToString();
        }
        if (character)
        {
        }
        else
        {
            character = Instantiate(this.characters[characterID]);
            character.GetComponent<CharacterScript>().SetCharacter(characterID, spriteIndex, x, y, scale, rotation);
            character.name = this.characters[characterID].name + "Instance"+frameID.ToString();
            character.transform.SetParent(GameObject.Find("CharacterInstanceManager").transform, false);
        }
        character.tag = "Character";
        return character.name;
    }
    public void UpdateCharacterSprite(GameObject character, int spriteIndex)
    {
        character.GetComponent<CharacterScript>().SetSprite(spriteIndex);
    }
}
