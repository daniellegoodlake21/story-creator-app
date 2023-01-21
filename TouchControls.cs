using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TouchControls : MonoBehaviour
{
    //private Dictionary<int, GameObject> trails = new Dictionary<int, GameObject>();
    //private Touch pinchFinger1;
    //private Touch pinchFinger2;
    //private ParticleSystem pinchExplosion;
    private string mode;
    private GameObject itemToMove;
    private GameObject characterToEdit;
    private string characterToEditName;
    public Button btnDone;
    public GameObject movePicker;
    public GameObject movePickerContainer;
    public GameObject spritePicker;
    public GameObject backgroundPicker;
    public GameObject characterPicker;
    public GameObject propPicker;
    public GameObject framePicker;
    public GameObject videoExporter;
    public GameObject projectCreator;
    private UnityEngine.Touch touch;
    private Vector3 lastMousePosition;
    public GameObject spritePickerContainer;
    public GameObject framePickerContainer;
    public GameObject characterPickerContainer;
    public GameObject backgroundPickerContainer;
    public GameObject propPickerContainer;
    public Button btnResizeRotate;
    public Button btnChangeSprite;
    public GameObject camera;
    private bool moving;
    // Start is called before the first frame update
    public bool ItemToMoveIsCharacter()
    {
        return this.itemToMove.GetComponent<CharacterScript>() != null;
    }
    public GameObject GetItemToMove()
    {
        return this.itemToMove;
    }
    void Start()
    {
        moving = false;
        this.mode = "DEFAULT"; // can be DEFAULT (do nothing), CHARACTER_PICKER_FOR_SPRITE_CHANGE, CHARACTER_PICKER_FOR_MOVE_CHARACTER, CHANGE_SPRITE, ADD_CHARACTER, CHANGE_BACKGROUND, MOVE_CHARACTER or DONE
        this.characterToEdit = null;
        this.characterToEditName = "";
        this.btnDone.gameObject.SetActive(false);
        this.btnResizeRotate.gameObject.SetActive(false);
        this.btnChangeSprite.gameObject.SetActive(false);
        this.lastMousePosition = new Vector3(0, 0, 0);
        this.spritePickerContainer.SetActive(false);
        this.framePickerContainer.SetActive(false);
        this.movePickerContainer.SetActive(false);
        this.characterPickerContainer.SetActive(false);
        this.backgroundPickerContainer.SetActive(false);
        this.propPickerContainer.SetActive(false);
    }
    public void SetMode(string newMode)
    {
        this.mode = newMode;
        if (this.mode == "CHANGE_BACKGROUND")
        {
            this.SetBackgroundMode();
        }
    }
    public void SetBackgroundMode()
    {
        backgroundPickerContainer.SetActive(true);
        backgroundPicker.GetComponent<BackgroundPickerScript>().LoadBackgroundImages();
    }
    private void MoveCharacter()
    {
        if (Input.GetKeyDown("d"))
        {
            BeDoneEditing(characterToEdit);
        }
        moving = true;
        // touch controls
        // Controls for the Swipe Gesture
        if (Input.GetMouseButton(0))
        {
            Vector3 position = this.characterToEdit.transform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = (mousePosition - lastMousePosition);
            //  Ensures character is visible
            mousePosition.z = -camera.transform.position.z;
            // Sets position of the character to position of the finger
            characterToEdit.transform.position = Camera.main.WorldToScreenPoint(mousePosition);
            this.lastMousePosition = mousePosition;
        // touch controls end
        }
        float x = 0;
        float y = 0;
        if (GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>())
        {
            x = GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>().GetXAxisPosition();
            y = GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>().GetYAxisPosition();
        }
        else if (GameObject.Find(this.characterToEditName).GetComponent<PropScript>())
        {
            // prop
            x = GameObject.Find(this.characterToEditName).GetComponent<PropScript>().GetXAxisPosition();
            y = GameObject.Find(this.characterToEditName).GetComponent<PropScript>().GetYAxisPosition();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x = x - 5.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            x = x + 5.0f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            y = y + 5.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            y = y - 5.0f;
        }
    }
    public void BeDoneEditing(GameObject character)
    {
        if (this.mode == "MOVE_CHARACTER")
        {
            if (character.GetComponent<CharacterScript>())
            {
                character.GetComponent<CharacterScript>().SetCharacterPosition(character.transform.position.x, character.transform.position.y);
            }
            else if (character.GetComponent<PropScript>())
            {
                character.GetComponent<PropScript>().SetPropPosition(character.transform.position.x, character.transform.position.y);
            }
        }
        this.projectCreator.GetComponent<StoryProjectScript>().SaveProject();
        this.SetMode("DEFAULT");
        btnDone.gameObject.SetActive(false);
    }
    private void ResizeOrRotate()
    {
        if (Input.GetKeyDown("d"))
        {
            BeDoneEditing(characterToEdit);
        }
        if (this.characterToEdit.GetComponent<CharacterScript>())
        {
            // scale
            if (Input.GetKey(KeyCode.RightBracket))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ModifyScale(true);
            }
            else if (Input.GetKey(KeyCode.LeftBracket))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ModifyScale(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ResetScale();
            }
            // rotation
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ModifyRotation(true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ModifyRotation(false);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ResetRotation();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.characterToEdit.GetComponent<CharacterScript>().ResetScale();
                this.characterToEdit.GetComponent<CharacterScript>().ResetRotation();
                this.characterToEdit.GetComponent<CharacterScript>().SetCharacterPosition(200, 200);
            }
        }
        else if (this.characterToEdit.GetComponent<PropScript>())
        {
            // scale
            if (Input.GetKey(KeyCode.RightBracket))
            {
                this.characterToEdit.GetComponent<PropScript>().ModifyScale(true);
            }
            else if (Input.GetKey(KeyCode.LeftBracket))
            {
                this.characterToEdit.GetComponent<PropScript>().ModifyScale(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                this.characterToEdit.GetComponent<PropScript>().ResetScale();
            }
            // rotation
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.characterToEdit.GetComponent<PropScript>().ModifyRotation(true);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.characterToEdit.GetComponent<PropScript>().ModifyRotation(false);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this.characterToEdit.GetComponent<PropScript>().ResetRotation();
            }
            // position in case of going off-screen
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.characterToEdit.GetComponent<PropScript>().ResetScale();
                this.characterToEdit.GetComponent<PropScript>().ResetRotation();
                this.characterToEdit.GetComponent<PropScript>().SetPropPosition(300, 300);
            }
        }
    }
    private void ChangeSprite()
    {
        int spriteIndex = GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>().GetCurrentSpriteIndex();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (spriteIndex - 1 < 0)
            {
                spriteIndex = GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>().GetSpriteCount() - 1;
            }
            else
            {
                spriteIndex--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (spriteIndex + 1 > GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>().GetSpriteCount() - 1)
            {
                spriteIndex = 0;
            }
            else
            {
                spriteIndex++;
            }
        }
        GameObject.Find(this.characterToEditName).GetComponent<CharacterScript>().SetCharacterSprite(spriteIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            var touch = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Input.GetTouch(0).position
            RaycastHit2D raycastHit = Physics2D.Raycast(Input.mousePosition, new Vector2(touch.x, touch.y), Mathf.Infinity);
            if (raycastHit.collider)
            {
                if (this.mode == "CHANGE_LIVE_MOVEMENT_PICK_ITEM")
                {
                    if (raycastHit.collider.tag == "Character" || raycastHit.collider.tag == "Prop")
                    {
                        this.itemToMove = raycastHit.collider.gameObject;
                        this.SetMode("CHANGE_LIVE_MOVEMENT");
                    }
                }
                else if (this.mode == "CHARACTER_PICKER_FOR_SPRITE_CHANGE")
                {
                    if (raycastHit.collider.tag == "Character")
                    {
                        this.characterToEdit = raycastHit.collider.gameObject;
                        this.characterToEditName = this.characterToEdit.name;
                        spritePicker.SetActive(true);
                        spritePicker.GetComponent<SpritePickerScript>().LoadSprites(this.characterToEdit);
                        this.SetMode("DEFAULT");
                        this.spritePickerContainer.SetActive(true);
                    }
                }
                if (this.mode == "CHARACTER_PICKER_FOR_MOVE_CHARACTER")
                {
                    if (raycastHit.collider.tag == "Character" || raycastHit.collider.tag == "Prop")
                    {
                        this.characterToEdit = raycastHit.collider.gameObject;
                        this.characterToEditName = this.characterToEdit.name;
                        this.SetMode("MOVE_CHARACTER");
                    }
                }
                else if (this.mode == "REMOVE_CHARACTER")
                {
                    if (raycastHit.collider.tag == "Character" || raycastHit.collider.tag == "Prop")
                    {
                        this.characterToEdit = raycastHit.collider.gameObject;
                        this.characterToEditName = this.characterToEdit.name;
                        this.projectCreator.GetComponent<StoryProjectScript>().RemoveCharacter(this.characterToEdit);
                        this.SetMode("DEFAULT");
                    }
                }
                else if (this.mode == "RESIZE_OR_ROTATE_PICKER")
                {
                    if (raycastHit.collider.tag == "Character" || raycastHit.collider.tag == "Prop")
                    {
                        this.characterToEdit = raycastHit.collider.gameObject;
                        this.characterToEditName = this.characterToEdit.name;
                        this.SetMode("RESIZE_OR_ROTATE");
                    }
                }
            }
        }
        if (this.mode == "ADD_CHARACTER")
        {
            characterPickerContainer.SetActive(true);
            characterPicker.GetComponent<CharacterPickerScript>().LoadCharacters();
            this.SetMode("DEFAULT");
        }
        if (this.mode == "ADD_PROP")
        {
            propPickerContainer.SetActive(true);
            propPicker.GetComponent<PropPickerScript>().LoadProps();
            this.SetMode("DEFAULT");
        }
        if (this.mode == "CHANGE_SPRITE")
        {
            this.ChangeSprite();
        }
        else if (this.mode == "MOVE_CHARACTER")
        {
            this.btnDone.gameObject.SetActive(true);
            this.MoveCharacter();
        }
        else if (this.mode == "RESIZE_OR_ROTATE")
        {
            this.btnDone.gameObject.SetActive(true);
            this.ResizeOrRotate();
        }
        else if (this.mode == "PICK_FRAME")
        {
            framePickerContainer.SetActive(true);
            framePicker.GetComponent<FramePickerScript>().LoadFrames();
            this.SetMode("DEFAULT");
        }
        else if (this.mode == "ADD_FRAME")
        {
            this.projectCreator.GetComponent<StoryProjectScript>().SaveNewFrameToProject();
            this.SetMode("DEFAULT");
        }
        else if (this.mode == "REMOVE_FRAME")
        {
            this.projectCreator.GetComponent<StoryProjectScript>().RemoveFrame();
            this.SetMode("DEFAULT");
            this.projectCreator.GetComponent<StoryProjectScript>().StopRemovingFrame();
        }
        else if (this.mode == "DUPLICATE_FRAME")
        {
            this.projectCreator.GetComponent<StoryProjectScript>().DuplicateFrame();
            this.SetMode("DEFAULT");
            this.projectCreator.GetComponent<StoryProjectScript>().StopDuplicatingFrame();
        }
        else if (this.mode == "START_RECORDING")
        {
            this.projectCreator.GetComponent<StoryProjectScript>().RecordVoice();
            this.SetMode("RECORDING");
        }
        else if (this.mode == "STOP_RECORDING")
        {
            this.projectCreator.GetComponent<StoryProjectScript>().StopRecording();
            this.SetMode("DEFAULT");
        }
        else if (this.mode == "EXPORT_VIDEO")
        {
            this.videoExporter.GetComponent<VideoPlayerScript>().ExportVideo();
            this.SetMode("DEFAULT");
        }
        else if (this.mode == "PLAY_AUDIO")
        {
            this.projectCreator.GetComponent<StoryProjectScript>().PlayFrameAudio();
            this.SetMode("DEFAULT");
        }
        else if (this.mode == "DONE")
        {
            this.SetMode("DEFAULT");
            this.btnDone.gameObject.SetActive(false);
        }
        else if (this.mode == "BACK_TO_MENU")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (this.mode == "CHANGE_LIVE_MOVEMENT")
        {
            projectCreator.GetComponent<StoryProjectScript>().SaveProject();
            movePicker.SetActive(true);
            movePicker.GetComponent<MovePickerScript>().LoadMoves();
            this.SetMode("DEFAULT");
            movePickerContainer.SetActive(true);
        }
    }
}
