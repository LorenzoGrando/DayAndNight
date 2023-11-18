using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _selectionSource, _movementSource, _musicSource, _bgAtmosphereSource;

    private Tween _musicTween;
    private Tween _atmosphereTween;
    private float _maxMusicVolume = 0;
    private float _maxAtmosphereVolume = 0;
    public void PlayMovementSound() {
        _movementSource.Play();
    }

    public void PlaySelectionSound() {
        _selectionSource.Play();
    }

    public void FadeInMenuSong() {
        if(_maxMusicVolume == 0) {
            _maxMusicVolume = _musicSource.volume;
            _maxAtmosphereVolume = _bgAtmosphereSource.volume;
        }
        ResetTweens();
        _musicSource.Play();
        _bgAtmosphereSource.Play();
        _musicSource.volume = 0;
        _bgAtmosphereSource.volume = 0;
        LerpVolumeValue(_musicSource, _musicTween, _maxMusicVolume, 7.5f, false);
        LerpVolumeValue(_bgAtmosphereSource, _atmosphereTween, _maxAtmosphereVolume, 2f, false);
    }

    public void FadeOutMenuSong() {
        LerpVolumeValue(_musicSource, _musicTween, 0, 2, true);
        LerpVolumeValue(_bgAtmosphereSource, _atmosphereTween, 0, 2, true);
    }
    
    private void LerpVolumeValue(AudioSource source, Tween tween, float valueToGoTowards, float time, bool stopMusic) {
        tween = source.DOFade(valueToGoTowards, time);
        tween.OnComplete(() => OnFadeComplete(stopMusic, source));
    }

    private void ResetTweens() {
        if(_musicTween != null) {
            _musicTween.Kill();
            _musicTween = null;
        }
        if(_atmosphereTween != null) {
            _atmosphereTween.Kill();
            _atmosphereTween = null;
        }
    }

    private void OnFadeComplete(bool stopMusic, AudioSource source) {
        ResetTweens();
        if(stopMusic) {
            source.Stop();
        }
    }
    
}
