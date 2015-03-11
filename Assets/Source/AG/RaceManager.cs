using UnityEngine;
using System.Collections;

public class RaceManager : MonoBehaviour {

    // Race Settings
    public enum RaceState
    {
        Overview = 0,
        Countdown = 1,
        Started = 2,
        Complete = 3
    };
    public RaceState raceProgression;
}
