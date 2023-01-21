using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public List<Sprite> sprites;
    public string characterName;
    private Vector3 position;
    private int spriteIndex;
    private int characterID;
    public void SetCharacterID(int characterID)
    {
        this.characterID = characterID;
        this.spriteIndex = 0;
        this.position.x = 1;
        this.position.y = 1;
    }
    public void SetCharacter(int characterID, int spriteIndex, float x, float y)
    {
        this.characterID = characterID;
        this.spriteIndex = spriteIndex;
        this.position.x = x;
        this.position.y = y;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public int GetCharacterID()
    {
        return this.characterID;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public Sprite GetCurrentSprite()
    {
        return sprites[spriteIndex];
    }
    public float GetXAxisPosition()
    {
        return position.x;
    }
    public float GetYAxisPosition()
    {
        return position.y;
    }
}
