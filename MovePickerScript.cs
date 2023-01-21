using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePickerScript : MonoBehaviour
{
    private List<GameObject> moveButtons;
    public GameObject moveManager;
    public GameObject templateMoveButton;
    public GameObject projectCreator;
    public GameObject touchControlsObject;
    // Start is called before the first frame update
    void Start()
    {
        this.moveButtons = new List<GameObject>();
    }

    public void LoadMoves()
    {
        this.moveButtons = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        List<GameObject> moves = moveManager.GetComponent<MoveManagerScript>().GetAllMoves();
        foreach (GameObject move in moves)
        {
            GameObject moveButton = Instantiate(templateMoveButton);
            moveButton.name = move.name + "Button";
            moveButton.GetComponent<Image>().sprite = move.GetComponent<Image>().sprite;
            moveButton.transform.SetParent(this.gameObject.transform, false);
            moveButton.GetComponent<MovementPickerMoveScript>().SetMove(move);
            this.moveButtons.Add(moveButton);
        }
    }
    public void SelectMove(GameObject move)
    {
        GameObject item = touchControlsObject.GetComponent<TouchControls>().GetItemToMove();
        if (item.GetComponent<CharacterScript>())
        {
            item.GetComponent<CharacterScript>().AttachMove(move);
            GameObject.Find("MovePicker").SetActive(false);
        }
        else if (item.GetComponent<PropScript>())
        {
            item.GetComponent<PropScript>().AttachMove(move);
            GameObject.Find("MovePicker").SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
