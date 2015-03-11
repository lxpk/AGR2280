using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {

    public List<AudioClip> MusicClips = new List<AudioClip>();
    public List<string> MusicNames = new List<string>();
    public List<string> MusicArists = new List<string>();

    private AudioSource audioSource;
    private AudioMixer audioMixer;

    private float highPassMax = 2178.0f;
    private float highPassMin = 10.0f;
    public float highPass = 10.0f;

    public ShipSimulator shipToCheck;
    
    // UI
    public Text songText;

    void Start()
    {
        GetAvailableMusic();
        SetupPlayer();
    }

    // Run on update just to save on performance
    void LateUpdate()
    {
        // If no song is playing then get random song index and play it
        if (!audioSource.isPlaying)
        {
            int rand = Random.Range(0, MusicClips.Count);
            AudioClip ChosenClip = MusicClips[rand];

            // Play clip
            audioSource.clip = ChosenClip;
            audioSource.Play();

            // Update song text
            songText.text = "NOW PLAYING: " + ChosenClip.name;

            // Play Scroll Animation
            songText.gameObject.GetComponent<Animation>().Play("NowPlayingScroll");
        }

        // High Pass
        if (shipToCheck != null && !shipToCheck.bShipIsGrounded)
        {
            highPass = Mathf.Lerp(highPass, highPassMax, Time.deltaTime);
        } else
        {
            highPass = Mathf.Lerp(highPass, highPassMin, Time.deltaTime * 5);
        }

        // Apply high pass
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("musicHP", highPass);
    }

    private void GetAvailableMusic()
    {
        
    }

    private void SetupPlayer()
    {
        // Create Audio Source
        audioSource = gameObject.AddComponent<AudioSource>();

        // Get audio mixer
        audioMixer = Resources.Load("Audio/Master") as AudioMixer;

        // Find music mixer groups
        AudioMixerGroup[] MusicGroups = audioMixer.FindMatchingGroups("Music");

        // Set audio source mixer group to first index
        audioSource.outputAudioMixerGroup = MusicGroups[0];
    }
}
