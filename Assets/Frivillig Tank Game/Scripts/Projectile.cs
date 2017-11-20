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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Contains("Player") && !collider.tag.Equals(gameObject.tag))
        {
            Camera.main.GetComponent<ScreenShake>().ShakeIt();
            collider.GetComponent<PlayerController>().Damage(20);
            gameObject.SetActive(false);
        }
    }
}