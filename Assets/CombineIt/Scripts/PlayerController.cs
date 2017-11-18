using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackState
{
    Normal,
    InMud
}

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private TrackState trackState;
    private bool goingForward;
    [SerializeField]
    private Transform frontCheck;
    [SerializeField]
    private Transform backCheck;

    [SerializeField]
    private KeyCode forwardKey;
    [SerializeField]
    private KeyCode backwardKey;
    [SerializeField]
    private KeyCode rotateLeftKey;
    [SerializeField]
    private KeyCode rotateRightKey;
    [SerializeField]
    private KeyCode shootKey;

    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float rotationSpeed = 20.0f;

    [SerializeField]
    private float currentSpeed;

    [SerializeField]
    private float maxVelocity = 20.0f;

    [SerializeField]
    private float mudSpeedFactor = 0.7f;

    [SerializeField]
    private LayerMask collideWith;
    private bool canCheck;
    private CannonController cannonController;
    private Rigidbody2D body;

    void Start ()
    {
        canCheck = true;
        cannonController = GetComponent<CannonController>();
        body = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        if (Input.GetKey(rotateLeftKey))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(rotateRightKey))
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(forwardKey))
        {
            currentSpeed = maxVelocity;
        }
        else if (Input.GetKeyDown(backwardKey))
        {
            currentSpeed = -maxVelocity;
        }

        if (Input.GetKeyDown(shootKey))
        {
            cannonController.Shoot();
        }
    }

    void FixedUpdate ()
    {
        if (canCheck)
        {
            if (currentSpeed < 0)
            {
                if (Physics2D.OverlapCircle(backCheck.position, 0.2f, collideWith))
                {
                    currentSpeed = maxVelocity;
                    StartCoroutine(DirectionChangeCooldown());
                }
            }
            else if (currentSpeed > 0)
            {
                if (Physics2D.OverlapCircle(frontCheck.position, 0.2f, collideWith))
                {
                    currentSpeed = -maxVelocity;
                    StartCoroutine(DirectionChangeCooldown());
                }
            }
        }

        float speed = currentSpeed;
        if (trackState == TrackState.InMud)
        {
            speed *= mudSpeedFactor;
        }

        if (trackState == TrackState.InMud)
        {
            speed *= mudSpeedFactor;
        }

        body.velocity = transform.up * speed;
        body.velocity = new Vector2(Mathf.Min(maxVelocity, body.velocity.x), Mathf.Min(maxVelocity, body.velocity.y));
    }

    IEnumerator DirectionChangeCooldown()
    {
        canCheck = false;
        yield return new WaitForSeconds(0.3f);
        canCheck = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "mud")
        {
            trackState = TrackState.InMud;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "mud")
        {
            if (trackState == TrackState.InMud)
            {
                trackState = TrackState.Normal;
            }
        }
    }
}
