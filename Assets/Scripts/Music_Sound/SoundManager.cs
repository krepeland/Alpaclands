using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;

    [SerializeField] private AudioSource soundSource;


    [SerializeField] private AudioClip clickSound;

    private bool debugSound;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        TryUpdateVolume();
        debugSound = true;
    }

    public static void TryPlaySound(AudioClip clip)
    {
        if (singleton == null || singleton.soundSource == null || singleton.debugSound == false)
            return;

        singleton.soundSource.clip = clip; 
        singleton.soundSource.Play();
    }

    public static void TryPlayClickSound() {
        if (singleton == null || singleton.soundSource == null || singleton.debugSound == false)
            return;
        TryPlaySound(singleton.clickSound);
    }

    public static void TryUpdateVolume()
    {
        if (singleton == null)
            return;

        singleton.SetVolume(KeyManager.GetKey("Sound") * 0.1f);
    }

    public void SetVolume(float value) {
        soundSource.volume = value;
    }
}
