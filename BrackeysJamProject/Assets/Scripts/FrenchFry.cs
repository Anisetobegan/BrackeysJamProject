using DG.Tweening;
using UnityEngine;

public class FrenchFry : Potatoe
{
    public override void OnInteract()
    {
        Oven oven = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Oven>();

        if (oven != null )
        {

        }
    }
}
