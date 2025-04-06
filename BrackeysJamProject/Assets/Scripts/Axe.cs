using System.Collections;
using UnityEngine;

public class Axe : PickableObject
{
    int durability;
    float damage = 40f;
    float timeOfActiveHitbox = 0.5f;

    [SerializeField] private SkinnedMeshRenderer rend;

    [SerializeField] Hitbox attackHitbox;

    void Awake()
    {
        stackable = false;
        pickedUp = false;
        isPrepped = false;
        prepAmount = 0;

        trigger = GetComponent<Collider>();

        type = ObjectType.Object;
    }

    private void OnEnable()
    {
        attackHitbox.InitializeHitbox(damage);
    }

    void LateUpdate()
    {
        if (durability == 0)
        {

        }

        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.PickablePos.position;
            transform.rotation = GameManager.Instance.PlayerGet.PickablePos.rotation * Quaternion.Euler(90, 0, 0);

            rend.SetBlendShapeWeight(0, GameManager.Instance.PlayerGet.SmearDriver);
        }
    }

    public override void OnInteract()
    {
        if (!pickedUp)
        {
            GameManager.Instance.PlayerGet.AddToStack(this);
            //GameManager.Instance.PlayerGet.CanInteract();
            ObjectAnimation(GameManager.Instance.PlayerGet.PickablePos.position, pickedUp);
        }
        else
        {
            //Player Attacks
            Debug.Log("Attacked");
            StartCoroutine(ActivateAttackHitbox());
        }
    }

    public void BreakAxe()
    {
        Drop();
        GameManager.Instance.PlayerGet.RemoveFromStack();
        Destroy(gameObject);
        //Now you can buy an Axe again
    }

    IEnumerator ActivateAttackHitbox()
    {
        attackHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeOfActiveHitbox);
        attackHitbox.gameObject.SetActive(false);
    }
}
