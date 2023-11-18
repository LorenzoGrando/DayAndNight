using UnityEngine;
using DG.Tweening;

public class InGameSoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _atmosphereSource, _musicSource, _shrineSound, _shrineAtmosphereSound, _collectableSound;

    private Tween _atmosphereTween, _musicTween, _shrineTween, _shrineAtmosphereTween;
    
    [SerializeField]
    private float _maxAtmosphereValue = 0;
    [SerializeField]
    private float _maxMusicValue = 0;
    [SerializeField]
    private float _maxShrineValue = 0;
    [SerializeField]
    private float _maxShrineAtmosphereValue = 0;

    public void FadeInMusic() {
        ResetTweens();
        if(_atmosphereSource.volume == 0) {
            FadeInAtmosphereMusic(0);
            FadeInRegularMusic(0);
        }
        else {
            FadeInAtmosphereMusic(_maxAtmosphereValue/2);
            FadeInRegularMusic(_maxMusicValue/2);
        }
    }

    public void PlayCollectable() {
        _collectableSound.Play();
    }

    public void StopMusic() {
        ResetTweens();
        FadeOutAtmosphereMusic();
        FadeOutRegularMusic();
    }

    public void HalfFadeOutMusic() {
        ResetTweens();
        HalfFadeOutAtmosphereMusic();
        HalfFadeOutRegularMusic();
    }

    public void PlayShrine() {
        _shrineSound.volume = 0;
        _shrineSound.Play();
        LerpVolumeValue(_shrineSound, _shrineTween, _maxShrineValue, 7f, true);
        _shrineAtmosphereSound.volume = 0;
        _shrineAtmosphereSound.Play();
        LerpVolumeValue(_shrineAtmosphereSound, _shrineAtmosphereTween, _maxShrineAtmosphereValue, 4.5f, false);
    }

    public void EndShrine() {
        LerpVolumeValue(_shrineAtmosphereSound, _shrineAtmosphereTween, 0, 6.5f, true);
    }

    private void FadeInAtmosphereMusic(float initialValue) {
        if(_maxAtmosphereValue == 0) {
            _maxAtmosphereValue = _atmosphereSource.volume;
        }
        if(!_atmosphereSource.isPlaying)
            _atmosphereSource.Play();

         _atmosphereSource.volume = initialValue; 
        LerpVolumeValue(_atmosphereSource, _atmosphereTween, _maxAtmosphereValue, 2.5f, false);
    }

    private void FadeInRegularMusic(float initialValue) {
        if(_maxMusicValue == 0) {
            _maxMusicValue = _musicSource.volume;
        }
        if(!_musicSource.isPlaying)
            _musicSource.Play();

         _musicSource.volume = initialValue; 
        LerpVolumeValue(_musicSource, _musicTween, _maxMusicValue, 2.5f, false);
    }

    private void FadeOutAtmosphereMusic() {
        LerpVolumeValue(_atmosphereSource, _atmosphereTween, 0, 2f, true);
    }

    private void FadeOutRegularMusic() {
        LerpVolumeValue(_musicSource, _musicTween, 0, 2f, true);
    }

    private void HalfFadeOutAtmosphereMusic() {
        LerpVolumeValue(_atmosphereSource, _atmosphereTween, 0, .5f, false);
    }

    private void HalfFadeOutRegularMusic() {
        LerpVolumeValue(_musicSource, _musicTween, 0, .5f, false);
    }

    private void LerpVolumeValue(AudioSource source, Tween tween, float valueToGoTowards, float time, bool stopMusic) {
        tween = source.DOFade(valueToGoTowards, time);
        tween.OnComplete(() => OnFadeComplete(stopMusic, source));
    }

    private void OnFadeComplete(bool stopMusic, AudioSource source) {
        ResetTweens();
        if(stopMusic) {
            source.Stop();
        }
    }

    private void ResetTweens() {
        if(_atmosphereTween != null) {
            _atmosphereTween.Kill();
            _atmosphereTween = null;
        }
        if(_musicTween != null) {
            _musicTween.Kill();
            _musicTween = null;
        }
        if(_shrineTween != null) {
            _shrineTween.Kill();
            _shrineTween = null;
        }
    }
}