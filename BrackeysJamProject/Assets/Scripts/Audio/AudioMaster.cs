using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioMaster", menuName = "AudioSystem/AudioMaster", order = 1)]
public class AudioMaster : ScriptableObject
{
    public List<AudioGroup> audioGroups;
    public List<AudioSystemEvents.OnPlay> audioEvents;
    public List<string> audioEventNames;
#if UNITY_EDITOR
    public bool eventsCollapsed = false;
#endif
}
