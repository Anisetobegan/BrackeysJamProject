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
        //For Debug purposes
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            OrderManager.Instance.PickUpOrder();
        }*/
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
