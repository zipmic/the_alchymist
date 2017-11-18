using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField]
    private Transform cannonPosition;
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private int poolSize = 20;
    private GameObject[] projectilePool;

    private int currentProjectile;

	void Start ()
    {
        projectilePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            projectilePool[i] = Instantiate(projectilePrefab);
            projectilePool[i].SetActive(false);
        }
	}

    public void Shoot()
    {
        var projectile = projectilePool[currentProjectile++];
        projectile.transform.position = cannonPosition.position;
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().ShootInDirection(transform.up);

        if (currentProjectile == poolSize)
        {
            currentProjectile = 0;
        }
    }
}
