using UnityEngine;

public class Hitbox : MonoBehaviour
{
    float _damage;
    IDamagable _damagable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _damagable = other.GetComponent<IDamagable>();
            _damagable.Damage(_damage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _damagable = other.GetComponent<IDamagable>();
            _damagable.Damage(_damage);
        }
    }

    public void InitializeHitbox(float damage)
    {
        this._damage = damage;
    }
}
