using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spritePartParents;
    [SerializeField]
    private SpriteLibraryAsset[] _capeLibraries, _femHairLibraries, _malehairLibraries;
    [SerializeField]
    private SpriteLibrary _capeLibrary, _hairLibrary;
    [SerializeField]
    private SpriteResolver _capeResolver, _hairResolver;
    [SerializeField]
    private float faceRightOffsetValue, faceLeftValue;
    private Player _player;
    public int activePlayerSprite = 0;
    private Animator _animator;

    private int tempCapeIndexAnim = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace)) {
            tempCapeIndexAnim++;
            UpdateCape(tempCapeIndexAnim);
        }
    }

    void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
        _player.OnPlayerDirectionalInput += UpdateDirections;
        SaveLoadSystem.OnSaveGame += UpdateHairMode;
    }

    void OnDisable()
    {
        _player.OnPlayerDirectionalInput -= UpdateDirections;
        SaveLoadSystem.OnSaveGame -= UpdateHairMode;
    }
    private void UpdateDirections(float xDirection) {
        bool isFaceLeft = xDirection == -1;
        bool isMaintainSide = xDirection == 0;
        
        foreach(GameObject spriteParent in spritePartParents) {
            for(int i = 0; i < spriteParent.transform.childCount; i++) {
                GameObject childObject = spriteParent.transform.GetChild(i).gameObject;
                SpriteRenderer sp = childObject.GetComponent<SpriteRenderer>();

                if(!isMaintainSide) {
                    sp.flipX = !isFaceLeft;
                }

                Vector3 transform = gameObject.transform.localPosition;
                if(sp.flipX) {
                    transform.x = faceRightOffsetValue;
                }
                else {
                    transform.x = faceLeftValue;
                }
                gameObject.transform.localPosition = transform;
            }
         }
    }

    public void UpdateAnimatorParameters(bool isWalking, int isGrounded) {
        _animator.SetBool("isWalking", isWalking);
        _animator.SetInteger("isGrounded", isGrounded);
    }

    public void TriggerJump() {
        _animator.SetTrigger("TriggerJump");
    }

    public void UpdateActiveSprite(SaveData saveData) {
        activePlayerSprite = (int)saveData.thisGameSpriteChoice;

        if(spritePartParents[0].transform.childCount > 0) {
            switch(activePlayerSprite) {
                case 0:
                    _hairLibrary.spriteLibraryAsset = _femHairLibraries[0];
                break;
                case 1:
                    _hairLibrary.spriteLibraryAsset = _malehairLibraries[0];
                break;
            }
        }

        UpdateHairMode(saveData);
    }

    public void UpdateHairMode(SaveData saveData)  {
        bool isMale = activePlayerSprite == 0 ? false : true;
        int hairIndex = 0;

        if(saveData.savedShrineData.sceneShrineData != null) {
            switch(saveData.savedShrineData.sceneShrineData[0].status) {
                case Shrine.ShrineTypeStatus.Uncomplete:
                    hairIndex = 0;
                break;
                case Shrine.ShrineTypeStatus.Sun:
                    hairIndex = 1;
                break;
                case Shrine.ShrineTypeStatus.Moon:
                    hairIndex = 2;
                break;
            }

            if(isMale) {
                _hairLibrary.spriteLibraryAsset = _malehairLibraries[hairIndex];
            }
            else {
                _hairLibrary.spriteLibraryAsset = _femHairLibraries[hairIndex];
            }
        }
        else {
            bool isMale2 = activePlayerSprite == 0 ? false : true;

            if(isMale2) {
                    _hairLibrary.spriteLibraryAsset = _malehairLibraries[0];
                }
                else {
                    _hairLibrary.spriteLibraryAsset = _femHairLibraries[0];
                }
        }
    }

    public void UpdateCape(int capeIndex) {
        _capeLibrary.spriteLibraryAsset = _capeLibraries[capeIndex];
    }
}
