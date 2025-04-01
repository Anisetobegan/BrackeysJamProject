using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BangDoor(bool enemyIncoming) 
    {
        _animator.SetBool("EnemyIncoming", enemyIncoming);
    }

    public void PlaySound(string soundEvent) 
    {
        AudioSystem.Instance.TriggerEvent(soundEvent);
    }
}
