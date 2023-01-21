using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementPickerMoveScript : MonoBehaviour
{
    public GameObject move;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisMove);
    }
    public void SetMove(GameObject move)
    {
        this.move = move;
    }
    public void SelectThisMove()
    {
        GameObject.Find("MovePickerContent").GetComponent<MovePickerScript>().SelectMove(this.move);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
