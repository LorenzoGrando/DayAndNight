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

    void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
        _player.OnPlayerDirectionalInput += UpdateDirections;
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

        //Old System
        if(spritePartParents[0].transform.childCount > 0) {
            switch(activePlayerSprite) {
                case 0:
                    spritePartParents[0].transform.GetChild(0).gameObject.SetActive(true);
                    spritePartParents[0].transform.GetChild(1).gameObject.SetActive(false);
                break;
                case 1:
                    spritePartParents[0].transform.GetChild(0).gameObject.SetActive(false);
                    spritePartParents[0].transform.GetChild(1).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void UpdateCape(int capeIndex) {
        _capeLibrary.spriteLibraryAsset = _capeLibraries[capeIndex];
    }
}
