using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioMaster))]
public class AudioMasterEditor : Editor
{
    private AudioMaster audioMaster;
    private Color orange = new Color(0.9921569f, 0.7529413f, 0.03137255f, 1f);

    List<bool> editingEvents;

    private bool save = false;

    //Dictionary<AudioImporter, AudioClipLoadType> clipTypes;

    private AudioClip playingClip = null;

    private void OnEnable()
    {
        audioMaster = target as AudioMaster;
        editingEvents = new List<bool>();
        if(audioMaster.audioEventNames == null) audioMaster.audioEventNames = new List<string>();
        for (int i = 0; i < audioMaster.audioEvents.Count; i++)
        {
            editingEvents.Add(false);
        }

        //clipTypes = new Dictionary<AudioImporter, AudioClipLoadType>();
        //for (int i = 0; i < audioMaster.audioGroups.Count; i++)
        //{
        //    //SwitchClipType(audioMaster.audioGroups[i]);
        //}
    }

    private void SwitchClipType(AudioGroup group) 
    {
        for (int i = 0; i < group.audioGroups.Count; i++)
        {
            SwitchClipType(group.audioGroups[i]);
        }

        //for (int i = 0; i < group.audioContainers.Count; i++)
        //{
        //    SwitchClipType(group.audioContainers[i]);
        //}
    }

    //private void SwitchClipType(AudioContainer container) 
    //{
    //    for (int i = 0; i < container.audioSfxs.Count; i++)
    //    {
    //        AudioSfx sfx = container.audioSfxs[i];
    //
    //        AudioImporter audiosettings = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sfx.clip)) as AudioImporter;
    //        AudioImporterSampleSettings settings = audiosettings.GetOverrideSampleSettings(EditorUserBuildSettings.activeBuildTarget.ToString());
    //        if (settings.loadType != AudioClipLoadType.DecompressOnLoad)
    //        {
    //            clipTypes.Add(audiosettings, settings.loadType);
    //            settings.loadType = AudioClipLoadType.DecompressOnLoad;
    //
    //            audiosettings.SetOverrideSampleSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), settings);
    //            audiosettings.SaveAndReimport();
    //        }
    //    }
    //}

    //private void OnDisable()
    //{
    //    foreach (AudioImporter audioSettings in clipTypes.Keys)
    //    {
    //        AudioImporterSampleSettings settings = audioSettings.GetOverrideSampleSettings(EditorUserBuildSettings.activeBuildTarget.ToString());
    //        settings.loadType = clipTypes[audioSettings];
    //
    //        audioSettings.SetOverrideSampleSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), settings);
    //        audioSettings.SaveAndReimport();
    //    }
    //}

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        DrawEventsInspector();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        DrawGroupsInspector();
        save = EditorGUI.EndChangeCheck();
        serializedObject.ApplyModifiedProperties();

        if(save)
            EditorUtility.SetDirty(audioMaster);
    }

    private void DrawEventsInspector() 
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent(audioMaster.eventsCollapsed ? "IN Foldout" : "d_icon dropdown"), "AC ComponentButton", GUILayout.Width(15)))
            audioMaster.eventsCollapsed = !audioMaster.eventsCollapsed;
        EditorGUILayout.LabelField("Events", GUILayout.Width(6 * 8));

        if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.MaxWidth(40))) 
        {
            AddEvent();
        }

        EditorGUILayout.EndHorizontal();

        if (!audioMaster.eventsCollapsed)
        {
            EditorGUILayout.BeginVertical("Badge");

            for (int i = 0; i < audioMaster.audioEventNames.Count; i++)
            {
                DrawEventInspector(i);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawEventInspector(int eventIndex) 
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("d_AudioSpatializerMicrosoft Icon"), GUILayout.MaxWidth(20), GUILayout.Height(20));

        float size = GUI.skin.label.CalcSize(new GUIContent(audioMaster.audioEventNames[eventIndex])).x;
        if (!editingEvents[eventIndex]) EditorGUILayout.LabelField(audioMaster.audioEventNames[eventIndex], GUILayout.Width(size));
        GUI.SetNextControlName("eventName");
        if (editingEvents[eventIndex]) audioMaster.audioEventNames[eventIndex] = EditorGUILayout.TextField(audioMaster.audioEventNames[eventIndex]);
        if (Event.current.Equals(Event.KeyboardEvent("return")) && GUI.GetNameOfFocusedControl() == "eventName") 
        {
            editingEvents[eventIndex] = false;
        }
        editingEvents[eventIndex] = GUILayout.Toggle(editingEvents[eventIndex], EditorGUIUtility.IconContent("d_editicon.sml"), "Button", GUILayout.Height(20), GUILayout.MaxWidth(40));
        if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus@2x"), GUILayout.MaxWidth(40), GUILayout.Height(20)))
        {
            audioMaster.audioEvents.RemoveAt(eventIndex);
            audioMaster.audioEventNames.RemoveAt(eventIndex);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AddEvent() 
    {
        audioMaster.audioEventNames.Add("Event");
        audioMaster.audioEvents.Add(new AudioSystemEvents.OnPlay());
        editingEvents.Add(true);
    }

    private void DrawSubscribedEventsInspector(ref AudioGroup group) 
    {
        if (group.subscribedEvents.Count > 0) EditorGUILayout.BeginVertical("Box");
        for (int i = 0; i < group.subscribedEvents.Count; i++)
        {
            DrawSubscribedEvent(i, ref group);
        }
        if (group.subscribedEvents.Count > 0) EditorGUILayout.EndVertical();
    }

    private void DrawSubscribedEventsInspector(ref AudioContainer container)
    {
        if(container.subscribedEvents.Count > 0) EditorGUILayout.BeginVertical("Box");
        for (int i = 0; i < container.subscribedEvents.Count; i++)
        {
            DrawSubscribedEvent(i, ref container);
        }
        if (container.subscribedEvents.Count > 0) EditorGUILayout.EndVertical();
    }

    private void DrawSubscribedEvent(int eventName, ref AudioContainer parent) 
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("d_AudioSpatializerMicrosoft Icon"), GUILayout.MaxWidth(20), GUILayout.Height(20));

        float size = GUI.skin.label.CalcSize(new GUIContent(audioMaster.audioEventNames[parent.subscribedEvents[eventName]])).x + 20f;
        parent.subscribedEvents[eventName] = EditorGUILayout.Popup(parent.subscribedEvents[eventName], audioMaster.audioEventNames.ToArray(), GUILayout.Width(size));
        if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus@2x"), GUILayout.MaxWidth(40), GUILayout.Height(20)))
        {
            parent.subscribedEvents.Remove(eventName);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSubscribedEvent(int eventName, ref AudioGroup parent)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("d_AudioSpatializerMicrosoft Icon"), GUILayout.MaxWidth(20), GUILayout.Height(20));

        float size = GUI.skin.label.CalcSize(new GUIContent(audioMaster.audioEventNames[parent.subscribedEvents[eventName]])).x + 20f;
        parent.subscribedEvents[eventName] = EditorGUILayout.Popup(parent.subscribedEvents[eventName], audioMaster.audioEventNames.ToArray(), GUILayout.Width(size));
        if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus@2x"), GUILayout.MaxWidth(40), GUILayout.Height(20)))
        {
            parent.subscribedEvents.Remove(eventName);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawGroupsInspector() 
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Group")) 
        {
            AudioGroup newGroup = new AudioGroup();
            audioMaster.audioGroups.Add(newGroup);
        }
        if (GUILayout.Button("Delete All")) 
        {
            audioMaster.audioGroups.Clear();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical("Box");
        for (int i = 0; i < audioMaster.audioGroups.Count; i++)
        {
            AudioGroup group = audioMaster.audioGroups[i];
            audioMaster.audioGroups[i] = DrawGroupInspector(ref group, ref audioMaster.audioGroups);
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndVertical();
    }

    private AudioGroup DrawGroupInspector(ref AudioGroup group, ref List<AudioGroup> parent) 
    {
        EditorGUILayout.BeginVertical("helpBox");

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button(EditorGUIUtility.IconContent(group.collapsed? "IN Foldout": "d_icon dropdown"), "AC ComponentButton", GUILayout.Width(15)))
            group.collapsed = !group.collapsed;
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("d_AudioMixerGroup Icon"), GUILayout.Width(20));
        EditorGUILayout.LabelField(group.name, EditorStyles.boldLabel, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(group.name)).x + 3f));
        if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus@2x"), GUILayout.Width(40f), GUILayout.Height(20f))) 
        {
            parent.Remove(group);
        }
        EditorGUILayout.EndHorizontal();

        if (group.collapsed) 
        {
            EditorGUILayout.EndVertical();
            return group; 
        }

        group.name = EditorGUILayout.TextField("Name", group.name);
        group.volume = EditorGUILayout.Slider("Volume", group.volume, 0f, 1f);

        if (audioMaster.audioEventNames.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Subscribed events", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Subscribed events")).x + 3f));
            if (group.subscribedEvents == null) group.subscribedEvents = new List<int>();
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.MaxWidth(40), GUILayout.Height(20)))
            {
                group.subscribedEvents.Add(0);
            }

            EditorGUILayout.EndHorizontal();

            DrawSubscribedEventsInspector(ref group);
        }

        if (group.audioGroups == null) group.audioGroups = new List<AudioGroup>();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Group"))
        {
            AudioGroup newGroup = new AudioGroup();
            
            group.audioGroups.Add(newGroup);
        }
        if (group.audioContainers == null) group.audioContainers = new List<AudioContainer>();
        if (GUILayout.Button("Add Container"))
        {
            AudioContainer newContainer = new AudioContainer();
            
            group.audioContainers.Add(newContainer);
        }
        EditorGUILayout.EndHorizontal();

        if(group.audioGroups.Count > 0) GUILayout.BeginVertical("Groups", "window");
        for (int i = 0; i < group.audioGroups.Count; i++)
        {
            AudioGroup subGroup = group.audioGroups[i];
            DrawGroupInspector(ref subGroup, ref group.audioGroups);
            EditorGUILayout.Space();
        }
        if (group.audioGroups.Count > 0) GUILayout.EndVertical();

        DrawContainersInspector(ref group);

        EditorGUILayout.EndVertical();
        return group;
    }

    private void DrawContainersInspector(ref AudioGroup group) 
    {
        if (group.audioContainers.Count > 0) GUILayout.BeginVertical("Containers", "window");
        for (int i = 0; i < group.audioContainers.Count; i++)
        {
            AudioContainer container = group.audioContainers[i];
            DrawContainerInspector(ref container, ref group);
            EditorGUILayout.Space();
        }
        if (group.audioContainers.Count > 0) GUILayout.EndVertical();
    }

    private AudioContainer DrawContainerInspector(ref AudioContainer container, ref AudioGroup parent) 
    {
        EditorGUILayout.BeginVertical("Badge");

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent(container.collapsed ? "IN Foldout" : "d_icon dropdown"), "AC ComponentButton", GUILayout.Width(15)))
            container.collapsed = !container.collapsed;
        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("d_AudioMixerController Icon"), GUILayout.Width(20));
        EditorGUILayout.LabelField(container.eventName, EditorStyles.boldLabel, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(container.eventName)).x + 3f));
        if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus@2x"), GUILayout.Width(40f), GUILayout.Height(20f)))
        {
            parent.audioContainers.Remove(container);
        }
        EditorGUILayout.EndHorizontal();

        if (container.collapsed) 
        {
            EditorGUILayout.EndVertical();
            return container;
        }
        container.eventName = EditorGUILayout.TextField("Event Name", container.eventName);
        container.volume = EditorGUILayout.Slider("Volume", container.volume, 0f, 1f);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Play on awake", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Play on awake")).x + 3f));
        container.playOnAwake = EditorGUILayout.Toggle(container.playOnAwake, GUILayout.Width(20));
        if (container.playOnAwake) 
        {
            List<string> scenes = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if(EditorBuildSettings.scenes[i].enabled)
                    scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
            }
            EditorGUILayout.LabelField("Scene spawn", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Scene spawn")).x + 3f));
            container.sceneIndex = EditorGUILayout.Popup(container.sceneIndex, scenes.ToArray());
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Play in sequence", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Play in sequence")).x + 3f));
        container.playInSequence = EditorGUILayout.Toggle(container.playInSequence, GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();

        if (audioMaster.audioEventNames.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Subscribed events", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Subscribed events")).x + 3f));
            if (container.subscribedEvents == null) container.subscribedEvents = new List<int>();
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.MaxWidth(40), GUILayout.Height(20)))
            {
                container.subscribedEvents.Add(0);
            }

            EditorGUILayout.EndHorizontal();

            DrawSubscribedEventsInspector(ref container);
        }

        if (container.audioSfxs == null) container.audioSfxs = new List<AudioSfx>();
        if (GUILayout.Button("Add Sfx"))
        {
            AudioSfx newSfx = new AudioSfx();
            container.audioSfxs.Add(newSfx);
        }
        
        for (int i = 0; i < container.audioSfxs.Count; i++)
        {
            EditorGUILayout.BeginVertical("HelpBox");
            AudioSfx sfx = container.audioSfxs[i];
            DrawSfxInspector(ref sfx, ref container);
            EditorGUILayout.EndVertical();
        }
        

        EditorGUILayout.EndVertical();

        return container;
    }

    private AudioSfx DrawSfxInspector(ref AudioSfx sfx, ref AudioContainer parent) 
    {
        EditorGUILayout.BeginHorizontal();
        if (sfx.clip != null)
        {
            if (GUILayout.Button(EditorGUIUtility.IconContent(sfx.collapsed ? "IN Foldout" : "d_icon dropdown"), "AC ComponentButton", GUILayout.Width(15)))
                sfx.collapsed = !sfx.collapsed;
            EditorGUILayout.LabelField(EditorGUIUtility.IconContent("d_AudioImporter Icon"), GUILayout.MaxWidth(20));
            EditorGUILayout.LabelField(sfx.clip.name, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(sfx.clip.name)).x));
            sfx.loop = GUILayout.Toggle(sfx.loop, EditorGUIUtility.IconContent("d_preAudioLoopOff"), "Button", GUILayout.Height(20), GUILayout.MaxWidth(40));
            if (GUILayout.Button(EditorGUIUtility.IconContent((AudioUtility.IsClipPlaying(sfx.clip) && sfx.clip == playingClip)? "d_PauseButton" : "PlayButton On@2x"), GUILayout.Height(20), GUILayout.MaxWidth(40)))
            {
                if (!AudioUtility.IsClipPlaying(sfx.clip))
                    TestAudio(sfx);
                else
                    StopAllAudio();
            }

            if (parent.audioSfxs.Count > 1)
            {
                int index = parent.audioSfxs.IndexOf(sfx);
                if (index != parent.audioSfxs.Count - 1 && GUILayout.Button(EditorGUIUtility.IconContent("ProfilerTimelineDigDownArrow"), GUILayout.Height(20), GUILayout.MaxWidth(40)))
                {
                    MoveDown(ref sfx, ref parent);
                }
                if (index != 0 && GUILayout.Button(EditorGUIUtility.IconContent("ProfilerTimelineRollUpArrow"), GUILayout.Height(20), GUILayout.MaxWidth(40)))
                {
                    MoveUp(ref sfx, ref parent);
                }
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus@2x"), GUILayout.MaxWidth(40f), GUILayout.Height(20f)))
            {
                parent.audioSfxs.Remove(sfx);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        if(sfx.clip == null)
            sfx.clip = EditorGUILayout.ObjectField("Audio Clip", sfx.clip, typeof(AudioClip), false) as AudioClip;
        if (EditorGUI.EndChangeCheck()) 
        {
            AudioImporter audiosettings = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sfx.clip)) as AudioImporter;
            AudioImporterSampleSettings settings = audiosettings.GetOverrideSampleSettings(EditorUserBuildSettings.activeBuildTarget.ToString());
            if (settings.loadType != AudioClipLoadType.DecompressOnLoad)
            {
                //clipTypes.Add(audiosettings, settings.loadType);
                settings.loadType = AudioClipLoadType.DecompressOnLoad;

                audiosettings.SetOverrideSampleSettings(EditorUserBuildSettings.activeBuildTarget.ToString(), settings);
                audiosettings.SaveAndReimport();
            }
        }

        if (sfx.clip != null && !sfx.collapsed) 
        {
            sfx.volume = EditorGUILayout.Slider("Volume", sfx.volume, 0f, 1f);
            
            EditorGUILayout.BeginHorizontal();
            string minLbl = "Min: " + sfx.pitchMin.ToString("0.00");
            string maxLbl = "Max: " + sfx.pitchMax.ToString("0.00");
            EditorGUILayout.MinMaxSlider("Pitch(" + minLbl + "; " + maxLbl + ")", ref sfx.pitchMin, ref sfx.pitchMax, -3f, 3f);
            EditorGUILayout.EndHorizontal();

            sfx.spatialSound = EditorGUILayout.Slider("Spatial Sound", sfx.spatialSound, 0f, 1f);

            GUILayout.Label(PaintWaveformSpectrum(sfx.clip, 1, Mathf.RoundToInt(GUILayoutUtility.GetLastRect().width), 40, orange));
        }

        return sfx;
    }

    private AudioContainer MoveUp(ref AudioSfx sfx, ref AudioContainer parent) 
    {
        int index = parent.audioSfxs.IndexOf(sfx);
        AudioSfx swappedSfx = parent.audioSfxs[index - 1];
        parent.audioSfxs[index - 1] = sfx;
        parent.audioSfxs[index] = swappedSfx;
        return parent;
    }

    private AudioContainer MoveDown(ref AudioSfx sfx, ref AudioContainer parent) 
    {
        int index = parent.audioSfxs.IndexOf(sfx);
        AudioSfx swappedSfx = parent.audioSfxs[index + 1];
        parent.audioSfxs[index + 1] = sfx;
        parent.audioSfxs[index] = swappedSfx;
        return parent;
    }

    private void TestAudio(AudioSfx sfx) 
    {
        AudioUtility.PlayClip(sfx.clip, 0, sfx.loop);
        playingClip = sfx.clip;
    }

    private void TestAudio(AudioContainer container)
    {

    }

    private void TestAudio(AudioGroup group)
    {

    }

    private void StopAllAudio() 
    {
        AudioUtility.StopAllClips();
    }

    private Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        Color transparent = new Color(0, 0, 0, 0);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, transparent);
            }
        }

        for (int x = 0; x < waveform.Length; x++)
        {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();

        return tex;
    }
}
