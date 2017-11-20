using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStop : MonoBehaviour
{
    public void Shoot()
    {
        GetComponentInParent<CannonController>().Shoot();
        GetComponent<Animator>().SetBool("shooting", false);
    }
}
