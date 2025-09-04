using Project.Core.Common;
using Project.Core.Features.Units;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IUnit
{
    [SerializeField] protected Rigidbody2D Rigidbody;

    public Transform Transform => transform;
    public Vector2 Position => Rigidbody.position;
    public float Rotation => Rigidbody.rotation;


    public abstract void ApplyDamage(DamageInfo damageInfo);

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
