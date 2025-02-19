using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;

    public Player PlayerGet { get { return _player; } }
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
