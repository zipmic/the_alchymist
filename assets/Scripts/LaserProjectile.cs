using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour {

    private Rigidbody2D _rb;

    public float Speed = 100;

	// Use this for initialization
	void Start () 
    {
        _rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        _rb.velocity = transform.up * Speed * Time.deltaTime;
		
	}
}
