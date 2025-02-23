using DG.Tweening;
using UnityEngine;

public class PotatoesCrate : InteractiveObject
{
    [SerializeField] Potatoe _potatoePrefab;

    public Potatoe PotatoePrefab {  get { return _potatoePrefab; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        Potatoe newPotatoe = Instantiate(_potatoePrefab, transform.position, transform.rotation);
        GameManager.Instance.PlayerGet.AddToStack(newPotatoe);

        /*Vector3 playerPos = GameManager.Instance.PlayerGet.transform.position;

        newPotatoe.transform.DOMoveX(playerPos.x, 0.2f).SetEase(Ease.Linear);
        newPotatoe.transform.DOMoveY(playerPos.y + 1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        newPotatoe.transform.DOMoveZ(playerPos.z, 0.2f).SetEase(Ease.Linear).OnComplete(() => newPotatoe.PickUp());*/
    }
}
