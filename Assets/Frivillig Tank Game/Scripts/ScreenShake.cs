using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private bool shouldShake;
    private float timeToShake = 0.5f;
    private float shakeAmount;
    private int currentIndex;
    private int indexCount;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void ShakeIt()
    {
        shouldShake = true;
        StartCoroutine(ShakeCooldown());
    }

    void Update()
    {
        if (shouldShake)
        {
            transform.position = new Vector3(initialPosition.x + Random.Range(-0.2f, 0.2f), initialPosition.y + Random.Range(-0.2f, 0.2f), transform.position.z);
        }
    }

    IEnumerator ShakeCooldown()
    {
        currentIndex = indexCount++;
        yield return new WaitForSeconds(timeToShake);
        if (currentIndex == indexCount - 1)
        {
            shouldShake = false;
            transform.position = initialPosition;
        }
    }
}
