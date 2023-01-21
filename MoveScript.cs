using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// visually, there is an arrow on-screen when a character/prop is selected to show their movement-to-be, but not in Play Story mode.
// this game object is always associated with a character/prop and can make the character/prop move
public class MoveScript : MonoBehaviour
{
    private bool isCharacter;
    public int moveID;
    private GameObject item;
    public float x;
    public float y;
    public bool delayed;
    // Start is called before the first frame update
    void Start()
    {
        isCharacter = true;
    }
    public int GetMoveID()
    {
        return moveID;
    }
    public float GetX()
    {
        return x;
    }
    public float GetY()
    {
        return y;
    }
    public bool IsDelayed()
    {
        return delayed;
    }
    public void SetMoveID(int id)
    {
        this.moveID = id;
    }
    public void AttachItem(GameObject itemToAttach)
    {
        // attaches both ways
        item = itemToAttach;
        if (isCharacter)
        {
            item.GetComponent<CharacterScript>().AttachMove(this.gameObject); // in addition to assigning the move as an attribute it also updates it in the files with the move (json class data - if no movement then x=0 and y=0)
        }
        else
        {
            item.GetComponent<PropScript>().AttachMove(this.gameObject); // in addition to assigning the move as an attribute it also updates it in the files with the move (json class data - if no movement then x=0 and y=0)
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
