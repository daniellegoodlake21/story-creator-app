using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteScript : MonoBehaviour
{
    public int spriteIndex;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectThisSprite);
    }
    private void SelectThisSprite()
    {
        GameObject.Find("SpritePickerContent").GetComponent<SpritePickerScript>().SelectSprite(this.spriteIndex);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
