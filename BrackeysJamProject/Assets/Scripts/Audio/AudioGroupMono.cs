using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AudioGroup
{
    public string name = "";
    [Range(0f, 1f)] public float volume = 0.5f;

    public List<AudioContainer> audioContainers;
    public List<AudioGroup> audioGroups;
    public List<int> subscribedEvents;

#if UNITY_EDITOR
    public bool collapsed = false;
#endif
}

public class AudioGroupMono : MonoBehaviour 
{
    private List<AudioGroupMono> groups = new List<AudioGroupMono>();
    public List<AudioGroupMono> Groups { get => groups; }

    private List<AudioContainerMono> containers = new List<AudioContainerMono>();
    public List<AudioContainerMono> Containers { get => containers; }

    public void Play() 
    {
        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].Play();
        }

        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].Play();
        }
    }

    public void Stop() 
    {
        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].Stop();
        }

        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].Stop();
        }
    }

    public List<AudioSource> GetAllAudioSource()
    {
        List<AudioSource> toReturn = new List<AudioSource>();

        for (int i = 0; i < groups.Count; i++)
        {
            toReturn.AddRange(groups[i].Containers.SelectMany(x => x.AudioSfxMonos.Select(y=>y.Source)));
        }

        for (int i = 0; i < containers.Count; i++)
        {
            toReturn.AddRange(containers[i].AudioSfxMonos.Select(x => x.Source));
        }

        return toReturn;
    }
}
