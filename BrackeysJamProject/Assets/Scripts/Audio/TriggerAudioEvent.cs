using UnityEngine;

public class TriggerAudioEvent : MonoBehaviour
{
    public void TriggerSound(string soundEvent)
    {
        AudioSystem.Instance.TriggerEvent(soundEvent);
    }
}
