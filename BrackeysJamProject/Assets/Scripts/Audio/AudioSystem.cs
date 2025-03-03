using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSystem : MonoBehaviour
{
    private static AudioSystem instance;
    public static AudioSystem Instance { get => instance; }

    public bool IsAudioMuted
    {
        get;
        private set;
    }

    private struct ContainerInstanceMeta 
    {
        public AudioContainer container;
        public AudioGroupMono parent;
    }

    [SerializeField] private AudioMaster audioMaster = null;
    private AudioMaster audioMasterInstance = null;

    private Dictionary<string, GameObject> groups;
    private Dictionary<string, GameObject> containers;
    private Dictionary<string, AudioSystemEvents.OnPlay> events;
    private Dictionary<string, List<ContainerInstanceMeta>> eventSubscriptors;

    private AudioGroupMono root = null;

    public static string IS_MUTE_KEY = "IsAudioMuted";

    private GameObject audioSourcesGO;
    public List<AudioSource> audioSources = new List<AudioSource>();
    public List<AudioSource> sourceInstances = new List<AudioSource>();
    private int currentAudioSourceIndex;

    public static int index;
    public int id;
    private void Awake()
    {
        id = index;
        index++;
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this) 
        {
            Destroy(this.gameObject);
            return;
        }

        name += "-" + (id);
        
        InitializeAudioSourcesPool();
        audioMasterInstance = ScriptableObject.Instantiate(audioMaster);

        //GameManager.Instance.sceneTrantitionManager.OnSceneLoadedEv += LoadSounds;
        GetEvents();

        LoadSounds();
        //GameManager.Instance.sceneTrantitionManager.OnSceneUnloadedEv += UnloadSounds;

        DontDestroyOnLoad(this.gameObject);

        //MuteAudio(GameManager.Instance.saveManager.LoadPref(IS_MUTE_KEY) == "1");
    }

    private void InitializeAudioSourcesPool()
    {
        GameObject audioSourcesGO = new GameObject("AudioSources");
        DontDestroyOnLoad(audioSourcesGO);
        for (int i = 0; i < 10; i++)
        {
            AudioSource audioSource = audioSourcesGO.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSources.Add(audioSource);
        }
    }

    public AudioSource Play(AudioClip audioClip, float volumen, float pitch, bool loop = false)
    {
        AudioSource source = audioSources[currentAudioSourceIndex];
        audioSources[currentAudioSourceIndex].clip = audioClip;
        audioSources[currentAudioSourceIndex].volume = volumen;
        audioSources[currentAudioSourceIndex].pitch = pitch;
        source.loop = loop;
        source.Play();
        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % audioSources.Count;
        return source;
    }

    public void MuteAudio(bool mute)
    {
        IsAudioMuted = mute;
        float volume = mute ? 0 : 1;
        SetVolume("Music", volume);
        SetVolume("SoundEffects", volume);
    }

    private void LoadSounds() 
    {
        if (root == null) 
        {
            root = GetComponent<AudioGroupMono>();
        }
        for (int i = 0; i < audioMasterInstance.audioGroups.Count; i++)
        {
            CreateHierarchy(audioMasterInstance.audioGroups[i], root);
        }
    }

    private void UnloadSounds() 
    {
        foreach (AudioSystemEvents.OnPlay audioEvent in events.Values)
        {
            audioEvent.RemoveAllListeners();
        }
        foreach (var subscriptors in eventSubscriptors.Values)
        {
            subscriptors.Clear();
        }

        for (int i = transform.childCount - 1; i > -1 ; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        root.Groups.Clear();
        root.Containers.Clear();
        sourceInstances.Clear();
    }

    private void GetEvents() 
    {
        events = new Dictionary<string, AudioSystemEvents.OnPlay>();
        eventSubscriptors = new Dictionary<string, List<ContainerInstanceMeta>>();
        for (int i = 0; i < audioMasterInstance.audioEventNames.Count; i++)
        {
            events.Add(audioMasterInstance.audioEventNames[i], audioMasterInstance.audioEvents[i]);
            eventSubscriptors.Add(audioMasterInstance.audioEventNames[i], new List<ContainerInstanceMeta>());
        }
    }

    private void CreateHierarchy(AudioGroup group, AudioGroupMono parent) 
    {
        GameObject groupObject = new GameObject(group.name);
        groupObject.transform.SetParent(parent.transform);
        AudioGroupMono groupMono = groupObject.AddComponent<AudioGroupMono>();

        parent.Groups.Add(groupMono);

        for (int i = 0; i < group.audioGroups.Count; i++)
        {
            CreateHierarchy(group.audioGroups[i], groupMono);
        }

        for (int i = 0; i < group.audioContainers.Count; i++)
        {
            CreateHierarchy(group.audioContainers[i], groupMono);
        }
    }

    private void CreateHierarchy(AudioContainer container, AudioGroupMono parentTransform) 
    {
        if (!container.playOnAwake)
        {
            for (int i = 0; i < container.subscribedEvents.Count; i++)
            {
                eventSubscriptors[audioMasterInstance.audioEventNames[container.subscribedEvents[i]]].Add(new ContainerInstanceMeta { parent = parentTransform, container = container });
            }
            
            return;
        }

        if (container.sceneIndex != SceneManager.GetActiveScene().buildIndex) return;

        GameObject containerObject = new GameObject(container.eventName);
        containerObject.transform.SetParent(parentTransform.transform);
        AudioContainerMono containerMono = containerObject.AddComponent<AudioContainerMono>();
        parentTransform.Containers.Add(containerMono);
        containerMono.PlayInSequence = container.playInSequence;

        for (int i = 0; i < container.audioSfxs.Count; i++)
        {
            GameObject sfxObject = new GameObject(container.audioSfxs[i].clip.name);
            sfxObject.transform.SetParent(containerObject.transform);
            AudioSfxMono sfxMono = sfxObject.AddComponent<AudioSfxMono>();

            AudioSource source = sfxObject.AddComponent<AudioSource>();
            source.clip = container.audioSfxs[i].clip;
            source.volume = container.audioSfxs[i].volume;
            source.pitch = Random.Range(container.audioSfxs[i].pitchMin, container.audioSfxs[i].pitchMax);
            source.spatialBlend = container.audioSfxs[i].spatialSound;
            source.loop = container.audioSfxs[i].loop;
            sfxMono.Source = source;
            sfxMono.SfxData = container.audioSfxs[i];

            sourceInstances.Add(source);
            containerMono.AudioSfxMonos.Add(sfxMono);
        }
        containerMono.Play();
    }

    public void TriggerEvent(string eventName) 
    {
        if (eventName == string.Empty || !eventSubscriptors.ContainsKey(eventName)) 
        {
            Debug.LogError("audio event " + eventName + " cannot be null");
            return;
        }

        if (eventSubscriptors[eventName].Count > 0) 
        {
            for (int i = 0; i < eventSubscriptors[eventName].Count; i++)
            {
                AudioContainer container = eventSubscriptors[eventName][i].container;
                GameObject containerObject = new GameObject(container.eventName);
                containerObject.transform.SetParent(eventSubscriptors[eventName][i].parent.transform);
                AudioContainerMono containerMono = containerObject.AddComponent<AudioContainerMono>();
                containerMono.PlayInSequence = container.playInSequence;
                for (int j = 0; j < container.audioSfxs.Count; j++)
                {
                    GameObject sfxObject = new GameObject(container.audioSfxs[j].clip.name);
                    sfxObject.transform.SetParent(containerObject.transform);
                    AudioSfxMono sfxMono = sfxObject.AddComponent<AudioSfxMono>();

                    AudioSource source = sfxObject.AddComponent<AudioSource>();
                    source.clip = container.audioSfxs[j].clip;
                    source.volume = container.audioSfxs[j].volume;
                    source.pitch = Random.Range(container.audioSfxs[i].pitchMin, container.audioSfxs[j].pitchMax);
                    source.spatialBlend = container.audioSfxs[j].spatialSound;
                    source.loop = container.audioSfxs[j].loop;

                    sfxMono.Source = source;
                    sfxMono.SfxData = container.audioSfxs[j];

                    sourceInstances.Add(source);

                    containerMono.AudioSfxMonos.Add(sfxMono);
                }
                events[eventName].AddListener(containerMono.Play);
            }

            eventSubscriptors[eventName].Clear();
        }
        events[eventName].Invoke();
    }

    public void TriggerButtonSFX()
    {
        TriggerEvent("OnButtonPressed");
    }

    private void TriggerGroup() 
    {

    }

    private void TriggerContainer() 
    {

    }

    public void Stop(string audioPath) 
    {
        Transform audioObject = transform.Find(audioPath);

        if (audioObject == null) return;

        AudioGroupMono audioGroup = audioObject.GetComponent<AudioGroupMono>();
        if (audioGroup != null) 
        {
            audioGroup.Stop();
            return;
        }

        AudioContainerMono audioContainer = audioObject.GetComponent<AudioContainerMono>();
        if (audioContainer != null) 
        {
            audioContainer.Stop();
        }
    }

    public List<AudioSource> GetAllAudioSources(string audioPath)
    {
        Transform audioObject = transform.Find(audioPath);
        AudioGroupMono audioGroup = audioObject.GetComponent<AudioGroupMono>();
        if (audioGroup != null)
        {
            return audioGroup.GetAllAudioSource();
        }

        AudioContainerMono audioContainer = audioObject.GetComponent<AudioContainerMono>();
        if (audioContainer != null)
        {
            audioContainer.Stop();
            return audioContainer.AudioSfxMonos.Select(x => x.Source).ToList();
        }

        return null;
    }

    public void SetVolume(string audioPath, float volume) 
    {
        //TODO MATI: change to a generic way to do this
        AudioGroup groupData = null;
        AudioGroup ogGroupData = null;
        for (int i = 0; i < audioMasterInstance.audioGroups.Count; i++)
        {
            if (audioMasterInstance.audioGroups[i].name == audioPath)
            {
                groupData = audioMasterInstance.audioGroups[i];
                ogGroupData = audioMaster.audioGroups[i];
                break;
            }
        }
        SetGroupsDataVolume(volume, groupData, ogGroupData);
    }

    private void SetGroupsDataVolume(float volume, AudioGroup groupData, AudioGroup ogGroupData) 
    {
        groupData.volume = Mathf.Lerp(0, ogGroupData.volume, volume);
        for (int i = 0; i < groupData.audioGroups.Count; i++)
        {
            SetGroupsDataVolume(groupData.volume, groupData.audioGroups[i], ogGroupData.audioGroups[i]);
        }

        for (int i = 0; i < groupData.audioContainers.Count; i++)
        {
            SetContainersDataVolume(groupData.volume, groupData.audioContainers[i], ogGroupData.audioContainers[i]);
        }
    }

    private void SetContainersDataVolume(float volume, AudioContainer containerData, AudioContainer ogContainerData) 
    {
        containerData.volume = Mathf.Lerp(0, ogContainerData.volume, volume);
        for (int i = 0; i < containerData.audioSfxs.Count; i++)
        {
            containerData.audioSfxs[i].volume = Mathf.Lerp(0, ogContainerData.audioSfxs[i].volume, containerData.volume);
        }
    }
}
