using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundPickerScript : MonoBehaviour
{
    private List<GameObject> backgroundImages;
    public GameObject backgroundImageManager;
    public GameObject backgroundImageTemplate;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        this.backgroundImages = backgroundImageManager.GetComponent<BackgroundImageManagerScript>().backgroundImages;
    }
    public void LoadBackgroundImages()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        List<Sprite> sprites = new List<Sprite>();
        for (int i = 0; i < this.backgroundImages.Count; i++)
        {
            Sprite currentSprite = this.backgroundImages[i].GetComponent<Image>().sprite;
            sprites.Add(currentSprite);
            GameObject backgroundObject = Instantiate(this.backgroundImageTemplate);
            backgroundObject.GetComponent<Image>().sprite = currentSprite;
            backgroundObject.name = i.ToString();
            backgroundObject.transform.SetParent(this.gameObject.transform, false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
