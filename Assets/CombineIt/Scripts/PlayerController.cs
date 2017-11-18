using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode forwardKey;
    [SerializeField]
    private KeyCode backwardKey;
    [SerializeField]
    private KeyCode rotateLeftKey;
    [SerializeField]
    private KeyCode rotateRightKey;

    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float currentSpeed;

    [SerializeField]
    private float speedStep = 5.0f;

    [SerializeField]
    private float maxVelocity = 30.0f;

    private Rigidbody2D body;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        if (Input.GetKey(rotateLeftKey))
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(forwardKey))
        {
            currentSpeed += speedStep;
        }
        else if (Input.GetKeyDown(backwardKey))
        {
            currentSpeed -= speedStep;
        }
    }

    void FixedUpdate ()
    {
        body.velocity = transform.up * currentSpeed;
        body.velocity = new Vector2(Mathf.Min(maxVelocity, body.velocity.x), Mathf.Min(maxVelocity, body.velocity.y));
    }
}
