using System;
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
    private float rotationSpeed = 20.0f;

    [SerializeField]
    private float currentSpeed;

    [SerializeField]
    private float speedStep = 1.0f;

    [SerializeField]
    private float maxVelocity = 20.0f;

    private Rigidbody2D body;
        
    [SerializeField] public static bool planningState = true;

    [SerializeField]
    private bool MANUAL_INPUT = false;

    private InputRecorder inputRecorder;

    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        inputRecorder = new InputRecorder();
    }

    void Update()
    {
        if (planningState && !MANUAL_INPUT)
        {
            var inputString = Input.inputString;
            if (string.IsNullOrEmpty(inputString))
                return;
            bool maxKeysRecorded = inputRecorder.RecordKey(inputString);
            if (maxKeysRecorded)
                planningState = false;
        }
        else if (MANUAL_INPUT)
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
                currentSpeed += speedStep;
            }
            else if (Input.GetKeyDown(backwardKey))
            {
                currentSpeed -= speedStep;
            }
        }
        // not planning state
        else
        {
            var nextKey = this.inputRecorder.GetNextKey();
            if (!string.IsNullOrEmpty(nextKey))
            {
                print("nextkey: " + nextKey);
                print("forwardKey: " + forwardKey.ToString());
                if (nextKey.Equals(forwardKey.ToString().ToLowerInvariant()))
                {
                    print(forwardKey.ToString());
                    currentSpeed = 1;
                }
                else if (nextKey.Equals(backwardKey.ToString().ToLowerInvariant()))
                {
                    print(forwardKey.ToString());
                    currentSpeed = -1;
                }
                else if (nextKey.Equals(rotateLeftKey.ToString().ToLowerInvariant()))
                {
                    currentSpeed = 0;
                    transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                }
                else if (nextKey.Equals(rotateRightKey.ToString().ToLowerInvariant()))
                {
                    currentSpeed = 0;
                    transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                }
            }   
        }
    }

    void FixedUpdate ()
    {
        body.velocity = transform.up * currentSpeed;
        body.velocity = new Vector2(Mathf.Min(maxVelocity, body.velocity.x), Mathf.Min(maxVelocity, body.velocity.y));
    }
}

public class InputRecorder : MonoBehaviour
{
    private int maxNumberOfKeys;
    private Queue<String> inputStrings;

    public InputRecorder(int maxNumberOfKeys = 5)
    {
        this.maxNumberOfKeys = maxNumberOfKeys;
        inputStrings = new Queue<String>();
    }

    // Returns true if maxNumberOfKeys is at target
    public bool RecordKey(string inputString)
    {
        this.inputStrings.Enqueue(inputString);
        print(this.inputStrings.Count);
        print(this.maxNumberOfKeys);
        return maxNumberOfKeys <= this.inputStrings.Count;
    }

    public String GetNextKey()
    {
        try
        {
            return this.inputStrings.Dequeue();
        }
        catch (InvalidOperationException ex)
        {
            PlayerController.planningState = true;
            return "";
        }
    }
}
