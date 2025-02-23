using DG.Tweening;
using UnityEngine;

public class FrenchFry : Potatoe
{

    protected override void Awake()
    {
        stackable = true;
        pickedUp = false;
        isPrepped = true;
        prepAmount = 0;

        type = ObjectType.Ingredient;
    }

    public override void OnInteract()
    {
        Oven oven = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Oven>();

        if (oven != null )
        {

        }
    }
}
