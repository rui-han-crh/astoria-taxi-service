using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCarriageAudioController : MonoBehaviour
{
    [SerializeField]
    private DriverBehavior driverBehavior;

    // Audio sources
    [SerializeField]
    private AudioSource horseGallop;
    [SerializeField]
    private AudioSource cartWheelsRotate;
    [SerializeField]
    private AudioSource carDoor;

    private TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> horseGallopTween;
    private TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> cartWheelsRotateTween;

    private void Start()
    {
        driverBehavior.OnMovementStarted += PerformStartMovingAudio;
        driverBehavior.OnMovementStopped += PerformStopMovingAudio;
    }

    public void PlayCarDoorAudio()
    {
        carDoor.Play();
    }

    private void PerformStartMovingAudio(float movementSpeed)
    {
        horseGallopTween?.Kill();
        cartWheelsRotateTween?.Kill();

        horseGallopTween = horseGallop.DOFade(1f, 0.5f);
        cartWheelsRotateTween = cartWheelsRotate.DOFade(1f, 0.5f);

        UnpauseOrPlay(horseGallop);
        UnpauseOrPlay(cartWheelsRotate);
    }

    private void PerformStopMovingAudio()
    {
        horseGallopTween?.Kill();
        cartWheelsRotateTween?.Kill();

        horseGallopTween = horseGallop.DOFade(0f, 0.5f).OnComplete(() => horseGallop.Pause());
        cartWheelsRotateTween = cartWheelsRotate.DOFade(0f, 0.5f).OnComplete(() => cartWheelsRotate.Pause());
    }

    private void UnpauseOrPlay(AudioSource audioSource)
    {
        if (audioSource.enabled == false)
        {
            // Cannot play a disabled audio source
            return;
        }

        if (audioSource.time != 0)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Play();
        }
    }

    private void OnDestroy()
    {
        horseGallopTween?.Kill();
        cartWheelsRotateTween?.Kill();
    }
}
