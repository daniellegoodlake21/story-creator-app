using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PropScript : MonoBehaviour
{
    private int propID;
    private float scale;
    private float rotation;
    private GameObject move;
    // Start is called before the first frame update
    void Start()
    {
        if (this.scale <= 0)
        {
            this.scale = 1;
        }
    }
    public int GetPropID()
    {
        return this.propID;
    }
    public GameObject GetMove()
    {
        if (move == null)
        {
            move = GameObject.Find("NoMovement");
        }
        return this.move;
    }
    public void SetPropID(int id)
    {
        this.propID = id;
        this.scale = 1;
        this.rotation = 0;
    }
    public void SetProp(int id, float x, float y, float scale, float rotation, GameObject copiedMove=null)
    {
        this.propID = id;
        this.gameObject.transform.position = new Vector2(x, y);
        this.scale = scale;
        this.rotation = rotation;
        if (copiedMove)
        {
            move = copiedMove;
        }
        this.DisplayProp();
    }
    public float GetScale()
    {
        return this.scale;
    }
    public float GetRotation()
    {
        return this.rotation;
    }
    public float GetXAxisPosition()
    {
        return this.gameObject.transform.position.x;
    }
    public float GetYAxisPosition()
    {
        return this.gameObject.transform.position.y;
    }

    public void AttachMove(GameObject move)
    {
        this.move = move;
        GameObject.Find("Scripts").GetComponent<StoryProjectScript>().UpdatePropMoveData(this.gameObject);
    }

    public void ResetScale()
    {
        this.scale = 1;
        DisplayProp();
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
        DisplayProp();
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
        DisplayProp();
    }
    public void ResetRotation()
    {
        this.rotation = 0;
        DisplayProp();
    }
    public void DisplayProp()
    {
        this.gameObject.SetActive(true);
        float x = this.gameObject.GetComponent<PropScript>().GetXAxisPosition();
        float y = this.gameObject.GetComponent<PropScript>().GetYAxisPosition();
        this.gameObject.transform.position = new Vector2(x, y);
        var color = this.gameObject.GetComponent<Image>().color;
        color.a = 1;
        Sprite sprite = this.gameObject.GetComponent<Image>().sprite;
        this.gameObject.GetComponent<Image>().color = color;
        this.gameObject.GetComponent<RectTransform>().sizeDelta = sprite.bounds.size * 25 * scale;
        this.gameObject.GetComponent<BoxCollider2D>().size = sprite.bounds.size * 25 * scale;
        sprite.bounds.Expand(new Vector2(25, 25) * scale);
        transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
    }

    public void SetPropPosition(float x, float y)
    {
        this.gameObject.transform.position = new Vector2(x, y);
        this.DisplayProp();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
