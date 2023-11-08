using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spritePartParents;
    [SerializeField]
    private float faceRightOffsetValue;
    private Player _player;
    public int activePlayerSprite = 0;

    void OnEnable()
    {
        _player = GetComponentInParent<Player>();
        _player.OnPlayerDirectionalInput += UpdateDirections;
    }
    private void UpdateDirections(float xDirection) {
        bool isFaceLeft = xDirection == -1;
        bool isMaintainSide = xDirection == 0;
        
        foreach(GameObject spriteParent in spritePartParents) {
            for(int i = 0; i < spriteParent.transform.childCount; i++) {
                GameObject childObject = spriteParent.transform.GetChild(i).gameObject;
                SpriteRenderer sp = childObject.GetComponent<SpriteRenderer>();
                Vector3 transform = childObject.transform.localPosition;

                if(!isMaintainSide) {
                    sp.flipX = !isFaceLeft;
                }

                if(sp.flipX) {
                    transform.x = faceRightOffsetValue;
                }
                else {
                    transform.x = 0;
                }
                childObject.transform.localPosition = transform;
            }
         }
    }

    public void UpdateActiveSprite(SaveData saveData) {
        activePlayerSprite = (int)saveData.thisGameSpriteChoice;

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
}
