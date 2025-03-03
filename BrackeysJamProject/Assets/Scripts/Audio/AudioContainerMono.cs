using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioContainer
{
    public string eventName = "";
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool playOnAwake = false;
    public bool playInSequence = false;
    public int sceneIndex = -1;
    public List<AudioSfx> audioSfxs;
    public List<int> subscribedEvents;
#if UNITY_EDITOR
    public bool collapsed = false;
#endif
}

public class AudioContainerMono : MonoBehaviour
{
    private List<AudioSfxMono> audioSfxMonos = new List<AudioSfxMono>();
    public List<AudioSfxMono> AudioSfxMonos { get => audioSfxMonos; }

    public bool PlayInSequence
    {
        get;
        set;
    }

    private Coroutine sfxPlayingCoroutine;
    
    public void Play() 
    {
        if (this.PlayInSequence)
        {
            StopSFXPlayingCoroutine();
            sfxPlayingCoroutine = StartCoroutine(WaitForCurrentSFX(audioSfxMonos));
        }
        else
        {
            for (int i = 0; i < audioSfxMonos.Count; i++)
            {
                audioSfxMonos[i].Play();
            }
        }
    }

    private IEnumerator WaitForCurrentSFX(List<AudioSfxMono> audioSfxMonos)
    {
        foreach (AudioSfxMono audioMono in audioSfxMonos)
        {
            AudioSource audioSource = audioMono.Source;
            audioMono.Play();
            yield return new WaitUntil(()=> !audioSource.loop && !audioSource.isPlaying);
        }
    }
    
    public void Stop()
    {
        for (int i = 0; i < audioSfxMonos.Count; i++)
        {
            audioSfxMonos[i].Stop();
        }

        this.StopSFXPlayingCoroutine();
    }

    private void StopSFXPlayingCoroutine()
    {
        if (this.sfxPlayingCoroutine != null)
        {
            this.StopCoroutine(this.sfxPlayingCoroutine);
        }
    }
}
