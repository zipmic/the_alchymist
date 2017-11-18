using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed;

    public void ShootInDirection(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}
