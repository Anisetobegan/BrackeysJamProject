using UnityEngine;

public class Oven : InteractiveObject
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        Debug.Log($"Interacted with: {this}");
    }
}
