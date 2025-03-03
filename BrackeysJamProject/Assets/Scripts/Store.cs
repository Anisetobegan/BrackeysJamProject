using UnityEngine;

public class Store : InteractiveObject
{

    void Start()
    {

    }

    void Update()
    {

    }

    public override void OnInteract()
    {
        UIManager.Instance.OpenStoreScreen();
    }
}
