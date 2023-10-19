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
}
