using DG.Tweening;
using UnityEngine;

public class FrenchFry : PickableObject
{
    void Awake()
    {
        stackable = true;
        pickedUp = false;
        isPrepped = true;

        type = ObjectType.Ingredient;

        /*Vector3 playerPos = GameManager.Instance.PlayerGet.transform.position;

        transform.DOMoveX(playerPos.x, 0.2f).SetEase(Ease.Linear);
        transform.DOMoveY(playerPos.y + 1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOMoveZ(playerPos.z, 0.2f).SetEase(Ease.Linear).OnComplete(() => PickUp());*/
    }


    void Update()
    {
        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.transform.position;
        }
    }
}
