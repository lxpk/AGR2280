// AGR2280 2012 - 2015
// Created by Vonsnake


using UnityEngine;
using System.Collections;

/// <summary>
/// Stores and updates in-game settings.
/// </summary>
public class GameSettings : MonoBehaviour {

    // GRAPHICS | Lighting
    public bool pixelLightCount;
    public bool useRealtimeReflections;
    
    // GRAPHICS | Shadows
    public bool bShadowsEnabled;

    // GRAPHICS | PP
    public bool bBloomEnabled;
    public bool bBoostEffectsEnabled;

    // AUDIO
    public float volumeMaster;
    public float volumeShips;
    public float volumeEnvironment;
    public float volumeAnnouncer;
    public float volumeMusic;

    public bool bMusicEffectsEnabled;
    public bool bMusicEnabled;
    public bool bAnnouncerEnabled;

    // Profile
    public string profileName;
    public string profileTag;
    

    /// <summary>
    /// Save the current game settings to the registry
    /// </summary>
    public void WritePlayerPreferences()
    {

    }

    /// <summary>
    /// Read the current game settings from the registry
    /// </summary>
    public void ReadPlayerReferences()
    {
        
    }

    /// <summary>
    /// Update the player name
    /// </summary>
    public void UpdateProfileName(string name)
    {
        profileName = name;
        PlayerPrefs.SetString("Pname", profileName);
    }
}
