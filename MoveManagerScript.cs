using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManagerScript : MonoBehaviour
{
    public List<GameObject> moves;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("HUDCanvas").transform, true);
        this.gameObject.transform.SetSiblingIndex(2);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    public List<GameObject> GetAllMoves()
    {
        List<GameObject> moves = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform)
        {
            GameObject move = child.gameObject;
            moves.Add(move);
        }
        return moves;
    }
    public string GetMoveByID(int moveID)
    {
        foreach (GameObject move in moves)
        {
            if (move.GetComponent<MoveScript>().moveID == moveID)
            {
                return move.name;
            }
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
