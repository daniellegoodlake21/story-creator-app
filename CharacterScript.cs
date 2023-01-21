using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

public class CharacterScript : MonoBehaviour
{
    public List<Sprite> sprites;
    public string characterName;
    private int spriteIndex;
    private int characterID;
    public Sprite sprite;
    private float scale;
    private float rotation;
    public GameObject move;
    public void SetCharacterID(int characterID)
    {
        this.characterID = characterID;
        this.scale = 1;
        this.spriteIndex = 0;
    }
    public GameObject GetMove()
    {
        if (move == null)
        {
            move = GameObject.Find("NoMovement");
        }
        return this.move;
       
    }
    public void SetSprite(int spriteIndex)
    {
        this.spriteIndex = spriteIndex;
        this.DisplayCharacter();
    }
    public void SetCharacter(int characterID, int spriteIndex, float x, float y, float scale, float rotation, GameObject copiedMove=null)
    {
        this.characterID = characterID;
        this.spriteIndex = spriteIndex;
        this.gameObject.transform.position = new Vector2(x, y);
        this.scale = scale;
        this.rotation = rotation;
        if (copiedMove)
        {
            move = copiedMove;
        }
        this.DisplayCharacter();
    }

    public void AttachMove(GameObject move)
    {
        this.move = move;
        GameObject.Find("Scripts").GetComponent<StoryProjectScript>().UpdateCharacterMoveData(this.gameObject);
    }

    public float GetScale()
    {
        return this.scale;
    }
    public float GetRotation()
    {
        return this.rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (this.scale <= 0)
        {
            this.scale = 1;
        }
    }
    public int GetCharacterID()
    {
        return this.characterID;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public Sprite GetDefaultSprite()
    {
        return sprites[0];
    }
    public Sprite GetCurrentSprite()
    {
        return sprites[spriteIndex];
    }
    public int GetCurrentSpriteIndex()
    {
        return spriteIndex;
    }
    public float GetXAxisPosition()
    {
        return this.gameObject.transform.position.x;
    }
    public float GetYAxisPosition()
    {
        return this.gameObject.transform.position.y;
    }
    public void HideCharacter()
    {
        this.gameObject.GetComponent<Image>().sprite = null;
    }
    public void DisplayCharacter()
    {
        this.gameObject.SetActive(true);
        this.sprite = this.gameObject.GetComponent<CharacterScript>().GetCurrentSprite();
        float x = this.gameObject.GetComponent<CharacterScript>().GetXAxisPosition();
        float y = this.gameObject.GetComponent<CharacterScript>().GetYAxisPosition();
        this.gameObject.transform.position = new Vector2(x, y);
        this.gameObject.GetComponent<Image>().sprite = sprite;
        var color = this.gameObject.GetComponent<Image>().color;
        color.a = 1;
        this.gameObject.GetComponent<Image>().color = color;
        this.gameObject.GetComponent<RectTransform>().sizeDelta = sprite.bounds.size * 50 * scale;
        this.gameObject.GetComponent<BoxCollider2D>().size = sprite.bounds.size * 50 * scale;
        sprite.bounds.Expand(new Vector2(50, 50) * scale);
        transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
    }
    public void ResetScale()
    {
        this.scale = 1;
        DisplayCharacter();
    }
    public void ModifyScale(bool increase)
    {
        if (increase)
        {
            this.scale += 0.01f;
        }
        else
        {
            this.scale -= 0.01f;
        }
        DisplayCharacter();
    }
    public void ModifyRotation(bool clockwise)
    {
        if (clockwise)
        {
            this.rotation += 0.2f;
        }
        else
        {
            this.rotation -= 0.2f;
        }
        DisplayCharacter();
    }
    public void ResetRotation()
    {
        this.rotation = 0;
        DisplayCharacter();
    }
    public int GetSpriteCount()
    {
        return this.sprites.Count;
    }
    public void SetCharacterSprite(int spriteIndex)
    {
        this.spriteIndex = spriteIndex;
        this.DisplayCharacter();
    }
    public void SetCharacterPosition(float x, float y)
    {
        this.gameObject.transform.position = new Vector2(x, y);
        this.DisplayCharacter();
    }
}