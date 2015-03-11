using UnityEngine;
using System.Collections;

public class AudioShot : MonoBehaviour {

    AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        if (!sound.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
