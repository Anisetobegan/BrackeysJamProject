using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSfx
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 0.5f;
    [Range(0f, 1f)] public float pitchMax = 1f;
    [Range(0f, 1f)] public float pitchMin = 1f;
    [Range(0f, 1f)] public float spatialSound = 0f;
    public bool loop = false;

#if UNITY_EDITOR
    public bool collapsed = false;
#endif
}

public class AudioSfxMono : MonoBehaviour 
{
    private AudioSource source = null;
    public AudioSource Source
    {
        get => this.source;
        set => source = value; 
    }

    private AudioSfx sfxData;
    public AudioSfx SfxData { set => sfxData = value; }
    public void Play() 
    {
        if (!source.isPlaying)
        {
            source.pitch = Random.Range(sfxData.pitchMin, sfxData.pitchMax);
            source.Play();
        }
        else 
        {
            GameObject newInstance = Instantiate(this.gameObject, this.transform.parent);
            AudioSource newSource = newInstance.GetComponent<AudioSource>();
            newSource.pitch = Random.Range(sfxData.pitchMin, sfxData.pitchMax);
            newSource.volume = source.volume;
            newSource.Play();
            Destroy(newInstance, newSource.clip.length);
        }
    }

    public void Stop() 
    {
        source.Stop();
    }

    private void Update()
    {
        if(source != null)
        {
            source.volume = sfxData.volume;
        }
    }
}
