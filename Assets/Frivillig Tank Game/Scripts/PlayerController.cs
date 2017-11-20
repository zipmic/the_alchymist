using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TrackState
{
    Normal,
    InMud
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameManager gameController;
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
    private int health = 100;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private AudioSource drivingSound;

    [SerializeField]
    private LayerMask collideWith;
    private bool canCheck;
    private CannonController cannonController;
    private Rigidbody2D body;

    private bool moveExecuted;
    private Queue<ActionType> actionsToExecute;
    private ActionType currentAction = ActionType.None;

    void Start ()
    {
        canCheck = true;
        cannonController = GetComponent<CannonController>();
        body = GetComponent<Rigidbody2D>();
        currentSpeed = 0;
	}

    public void ExecuteActions(Queue<ActionType> actions)
    {
        actionsToExecute = actions;
        StartCoroutine(ExecuteAction());
    }

    IEnumerator ExecuteAction()
    {
        currentAction = actionsToExecute.Dequeue();
        float timeUntilNextAction = 2.0f;

        if (drivingSound.isPlaying && currentAction != ActionType.Forward || currentAction != ActionType.Backwards)
        {
            drivingSound.Stop();
        }

        switch (currentAction)
        {
            case ActionType.RotateLeft45:
            case ActionType.RotateRight45:
                timeUntilNextAction = 0.5f;
                break;
            case ActionType.RotateLeft90:
            case ActionType.RotateRight90:
                timeUntilNextAction = 1.5f;
                break;
            case ActionType.Forward:
            case ActionType.Backwards:
                drivingSound.Play();
                break;
        }

        yield return new WaitForSeconds(timeUntilNextAction);

        if (actionsToExecute.Count > 0)
        {
            StartCoroutine(ExecuteAction());
        }
        else
        {
            currentAction = ActionType.None;
            gameController.PlayerDone(transform.tag);
        }
    }

    void Update()
    {
        switch (currentAction)
        {
            case ActionType.Forward:

                currentSpeed = maxVelocity;
                moveExecuted = false;
                break;
            case ActionType.Backwards:
                currentSpeed = -maxVelocity;
                moveExecuted = false;
                break;
            case ActionType.RotateLeft90:
            case ActionType.RotateLeft45:
                transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                break;
            case ActionType.RotateRight90:
            case ActionType.RotateRight45:
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                break;
            case ActionType.Stop:
                currentSpeed = 0.0f;
                break;
            case ActionType.Shoot:
                cannonController.InitiateShot();
                currentAction = ActionType.None;
                break;
        }
    }

    void FixedUpdate()
    {
        if (!moveExecuted)
        {
            if (currentSpeed < 0 && Physics2D.OverlapCircle(backCheck.position, 0.2f, collideWith) || currentSpeed > 0 && Physics2D.OverlapCircle(frontCheck.position, 0.2f, collideWith))
            {
                currentSpeed = 0.0f;
            }

            speed = currentSpeed;
            if (trackState == TrackState.InMud)
            {
                speed *= mudSpeedFactor;
            }

            if (trackState == TrackState.InMud)
            {
                speed *= mudSpeedFactor;
            }
            body.velocity = transform.right * speed;

            moveExecuted = true;
        }
        else
        {
            currentSpeed *= 0.9f;
            body.velocity = transform.right * currentSpeed;
        }

        body.velocity = new Vector2(Mathf.Min(maxVelocity, body.velocity.x), Mathf.Min(maxVelocity, body.velocity.y));
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        healthText.text = gameObject.tag + " : " + health;
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
