using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPickerCharacterScript : MonoBehaviour
{
    public GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisCharacter);
    }
    public void SetCharacter(GameObject character)
    {
        this.character = character;
    }
    public void SelectThisCharacter()
    {
        GameObject.Find("CharacterPickerContent").GetComponent<CharacterPickerScript>().SelectCharacter(this.character);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
