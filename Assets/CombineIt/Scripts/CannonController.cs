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

    [SerializeField]
    private AudioSource[] shootingAudio;

    private int currentProjectile;

	void Start ()
    {
        projectilePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            projectilePool[i] = Instantiate(projectilePrefab);
            projectilePool[i].SetActive(false);
            projectilePool[i].tag = transform.tag;
        }
	}

    public void InitiateShot()
    {
        GetComponentInChildren<Animator>().SetBool("shooting", true);
    }

    public void Shoot()
    {
        Camera.main.GetComponent<ScreenShake>().ShakeIt();
        var projectile = projectilePool[currentProjectile++];
        projectile.transform.position = cannonPosition.position + transform.right * 0.5f;
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().ShootInDirection(transform.right);
        projectile.transform.rotation = transform.rotation;
        shootingAudio[Random.Range(0, 1)].Play();

        if (currentProjectile == poolSize)
        {
            currentProjectile = 0;
        }
    }
}
