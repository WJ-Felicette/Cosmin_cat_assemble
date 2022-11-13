using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private AudioSource audiosource;
    private float musicVolume = 1f;

    // Use this for initialization
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audiosource.volume = musicVolume;
    }

    public void musicCheck()
    {
        audiosource.mute = !audiosource.mute;
    }

    public void setVolume(float vol)
    {
        musicVolume = vol;
    }
}