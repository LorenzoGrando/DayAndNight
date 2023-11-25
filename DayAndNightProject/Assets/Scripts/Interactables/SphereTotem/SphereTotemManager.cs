using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTotemManager : MonoBehaviour, IInteractable
{
    private GlowEffectManager _totemGlowEffect;
    private PlayerInteractor _lastPlayerInteractor;
    private IInteractable interactable;
    public CrystalSphere currentSphere {get; private set;}
    private SpriteRenderer _spriteObjectRenderer;
    [SerializeField]
    private Sprite[] stateSprites;
    [SerializeField]
    private AudioSource _placeSound;
    [SerializeField]
    private GameObject textTooltipoObject;

    void OnEnable() {
        _totemGlowEffect = _totemGlowEffect != null ? _totemGlowEffect : GetComponentInChildren<GlowEffectManager>();
        _spriteObjectRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        interactable = this;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorEnter(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerInteractor")) {
            interactable.OnInteractorExit(other.gameObject);
        }
    }

    void IInteractable.OnInteractorEnter(GameObject interactorObject) {
        _lastPlayerInteractor = interactorObject.GetComponent<PlayerInteractor>();
        _lastPlayerInteractor.UpdateLastInteractable(interactable);
        if((_lastPlayerInteractor.playerDataManager.currentPlayerData.currentHeldSphere != null && currentSphere == null) 
            || _lastPlayerInteractor.playerDataManager.currentPlayerData.currentHeldSphere == null && currentSphere != null)
            textTooltipoObject.SetActive(true);
    }

    void IInteractable.OnInteractorExit(GameObject oldInteractorObject) {
        if(_lastPlayerInteractor != null) {
            if(Vector3.Distance(transform.position, _lastPlayerInteractor.transform.position) > 0.5f) {
                if(_lastPlayerInteractor.GetLastInteractable() == interactable) {
                    _lastPlayerInteractor.UpdateLastInteractable(null);
                    _lastPlayerInteractor = null;
                }
            }
            textTooltipoObject.SetActive(false);
        }
    }

    void IInteractable.OnInteractorActivate() {
        if(currentSphere == null) {
            if(_lastPlayerInteractor.playerDataManager.currentPlayerData.currentHeldSphere != null) {
                CrystalSphere sphere = _lastPlayerInteractor.playerDataManager.currentPlayerData.currentHeldSphere;
                _lastPlayerInteractor.LeaveSphere();
                GetSphere(sphere);
                _spriteObjectRenderer.sprite = stateSprites[1];
                _totemGlowEffect.thisGlowType = GlowEffectManager.GlowType.Null;
            }
        }
        else {
            if(_lastPlayerInteractor.playerDataManager.currentPlayerData.currentHeldSphere == null) {
                _totemGlowEffect.ForceFadeOutGlow();
                _lastPlayerInteractor.GetSphere(currentSphere);
                currentSphere = null;
                _spriteObjectRenderer.sprite = stateSprites[0];
            }
        }
    }

    void IInteractable.OnInteractorGlowActivate(GlowEffectManager.GlowType glowType) {
        if(_totemGlowEffect.GetGlowType() != glowType) {
            _totemGlowEffect.EndGlowEffect();
            _totemGlowEffect.StartGlow(glowType, false);
        }
    }

    void GetSphere(CrystalSphere sphere) {
        currentSphere = sphere;
        currentSphere.UpdateSphereStatus(CrystalSphere.SphereStatus.Hidden);
        _placeSound.Play();
        currentSphere.transform.parent = this.transform.parent;
        currentSphere.transform.localPosition = Vector3.zero;
    }
}
