using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageManagerScript : MonoBehaviour
{
    public List<GameObject> backgroundImages;

    // Start is called before the first frame update
    void Start()
    {
    }
    public Sprite GetBackgroundImageSprite(int index)
    {
        Sprite sprite = backgroundImages[index].GetComponent<Image>().sprite;
        sprite.rect.Set(0, 0, 200, 200);
        return sprite;
    }
    public Texture2D GetBackgroundImage(int index)
    {
        return backgroundImages[index].GetComponent<Image>().sprite.texture;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
