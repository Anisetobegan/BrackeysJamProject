using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Transform _enemyStartingPos;
    [SerializeField] Enemy _enemyPrefab;
    Enemy _currentEnemy = null;

    public enum GameState
    {
        Normal,
        EnemyIncoming,
        EnemyChasing,
        GameOver
    }
    public GameState gameState;

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
        /*switch (gameState)
        {
            case GameState.Normal:

                break;

            case GameState.EnemyIncoming:
                //Start turning the lighting red gradually and playing door knocking sound
                break;

            case GameState.EnemyChasing:
                //Stops all Orders timers, turns lights red
                if (_currentEnemy != null)
                {
                        
                }

                break;

            case GameState.GameOver:

                break;
        }*/
    }

    void Update()
    {
        //For Debug purposes
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            OrderManager.Instance.PickUpOrder();
        }*/
    }

    public void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemyPrefab, _enemyStartingPos.position, Quaternion.identity);
    }

    public void ChangeGameState(GameState newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameState.Normal:
                //Play normal music and normal lighting
                break;

            case GameState.EnemyIncoming:
                //Start turning the lighting red gradually and playing door knocking sound
                break;

            case GameState.EnemyChasing:
                //Stops all Orders timers, turns lights red and spawns enemy
                SpawnEnemy();
                break;

            case GameState.GameOver:

                break;
        }
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
