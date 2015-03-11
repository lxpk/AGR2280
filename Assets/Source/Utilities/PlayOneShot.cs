using UnityEngine;
using System.Collections;

public class PlayOneShot : MonoBehaviour {

    public static void PlayShot(string SoundName, float volume, float pitch, float minRadius, float maxRadius, Transform parent, Vector3 position, AudioClip sound)
    {
        GameObject newSound = new GameObject(SoundName);
        newSound.AddComponent<AudioSource>();
        newSound.GetComponent<AudioSource>().volume = volume;
        newSound.GetComponent<AudioSource>().pitch = pitch;
        newSound.GetComponent<AudioSource>().dopplerLevel = 0;
        newSound.GetComponent<AudioSource>().spatialBlend = 1;
        newSound.GetComponent<AudioSource>().minDistance = minRadius;
        newSound.GetComponent<AudioSource>().maxDistance = maxRadius;
        newSound.GetComponent<AudioSource>().clip = sound;
        newSound.GetComponent<AudioSource>().loop = false;

        newSound.transform.parent = parent;
        newSound.transform.position = position;

        newSound.GetComponent<AudioSource>().Play();
        newSound.AddComponent<AudioShot>();

    }
}
