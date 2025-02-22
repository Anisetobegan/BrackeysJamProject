using UnityEngine;

public class FirstAidKit : InteractiveObject
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
