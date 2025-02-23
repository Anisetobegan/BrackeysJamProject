using DG.Tweening;
using UnityEngine;

public class FrenchFry : Potatoe
{
    protected override void Awake()
    {
        base.Awake();

        Transform viewContainer = transform.GetChild(0);
        viewContainer.GetChild((Random.Range(0, viewContainer.childCount))).gameObject.SetActive(true);
    }
    public override void OnInteract()
    {
        Oven oven = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Oven>();

        if (oven != null )
        {

        }
    }
}
