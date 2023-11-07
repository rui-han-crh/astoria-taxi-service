using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This controller causes routes audio events to be played when the taxi is boarded or
/// a passenger is dropped off.
/// 
/// The use of this case mains the single responsibility principle, as the Taxi class
/// should not be responsible for playing audio.
/// 
/// At the same time, it maintains the downward dependency from Taxi to HorseCarriageAudioController,
/// the child prefab does not depend on the parent component directly.
/// </summary>
public class TaxiAudioController : MonoBehaviour
{
    [SerializeField]
    private Taxi taxi;

    [SerializeField]
    private HorseCarriageAudioController horseCarriageAudioController;

    private void Awake()
    {
        taxi.OnBoard += PlayCarDoorAudio;
        taxi.OnDropOff += PlayCarDoorAudio;
    }

    private void PlayCarDoorAudio()
    {
        horseCarriageAudioController.PlayCarDoorAudio();
    }

}
