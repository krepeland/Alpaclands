using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager singleton;

    private float T;
    private int clipNowID;
    private float TargetTime = 1;

    public List<AudioClip> MusicClips;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        singleton = this;
        TryUpdateVolume();
    }

    public void SetVolume(float value) {
        audioSource.volume = value;
    }

    public static void TryUpdateVolume()
    {
        if (singleton == null)
            return;

        singleton.SetVolume(KeyManager.GetKey("Music") * 0.1f);
    }

    void Update()
    {
        T += Time.deltaTime;
        if (T >= TargetTime) {
            var newClipID = Random.Range(0, MusicClips.Count);
            if (clipNowID == newClipID) {
                newClipID = (newClipID + 1) % MusicClips.Count;
            }
            T = 0;
            clipNowID = newClipID;
            var clip = MusicClips[clipNowID];
            TargetTime = clip.length + 4;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
