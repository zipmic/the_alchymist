using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour 
{

    public GameObject BackThruster, LeftThruster,RightThruster, LaserSpawnPoint;

    public float BackThrusterSpeed, LeftRightThrusterSpeed;

    public ParticleSystem BackThrusterParticle1,BackThrusterParticle2, LeftThrusterParticle, RightThrusterParticle, BrakeParticle;

    public GameObject PrefabLaserProjectile;

    public enum Thruster {None, Left,Right,Forward};
    public Thruster CurrentThruster = Thruster.None;

    private bool _isBraking = false;
    private bool _isShootingLaser = false;

    // kun en thruster af gangen

    private Rigidbody2D _rb;


   

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();   
    }

	// Use this for initialization
	void Start () 
    {

        BackThrusterParticle1.Stop();
        BackThrusterParticle2.Stop();
        LeftThrusterParticle.Stop();
        RightThrusterParticle.Stop();
        BrakeParticle.Stop();
		
	}

    void Update()
    {

        if (_isBraking)
        {
            CurrentThruster = Thruster.None;
            _isShootingLaser = false;
        }

        if (CurrentThruster != Thruster.None)
        {
            _isBraking = false;
        }



    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		
        // left, right, stop, back 
        // i toggle

        if (Input.GetKey(KeyCode.W) || CurrentThruster == Thruster.Forward)
        {

            _rb.AddForce(transform.up * BackThrusterSpeed * Time.deltaTime);
            BackThrusterParticle1.Play();
            BackThrusterParticle2.Play();
            // _rb.AddForceAtPosition(transform.up*BackThrusterSpeed*Time.deltaTime, BackThruster.transform.position);

        }
        else if (Input.GetKeyUp(KeyCode.W) || !CurrentThruster == Thruster.Forward)
        {
            BackThrusterParticle1.Stop();
            BackThrusterParticle2.Stop();
        }

        if (Input.GetKey(KeyCode.A) || CurrentThruster == Thruster.Left)
        {
            LeftThrusterParticle.Play();
            _rb.AddForceAtPosition(transform.right * -1 * LeftRightThrusterSpeed * Time.deltaTime, LeftThruster.transform.position);
        }
        else if (Input.GetKeyUp(KeyCode.A) || !CurrentThruster == Thruster.Left)
        {
            LeftThrusterParticle.Stop();
        }

        if (Input.GetKey(KeyCode.D) || CurrentThruster == Thruster.Right)
        {
            RightThrusterParticle.Play();
            _rb.AddForceAtPosition(transform.right * LeftRightThrusterSpeed * Time.deltaTime, RightThruster.transform.position);
        }
        else if (Input.GetKeyUp(KeyCode.D) || !CurrentThruster == Thruster.Right)
        {
            RightThrusterParticle.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space) || _isShootingLaser )
        {
            GameObject tmp = Instantiate(PrefabLaserProjectile) as GameObject;
            tmp.transform.position = LaserSpawnPoint.transform.position;
            tmp.transform.rotation = LaserSpawnPoint.transform.rotation;


        }

        // brake

        if (Input.GetKeyDown(KeyCode.B) || _isBraking)
        {
            _rb.drag = 1.3f;
            _rb.angularDrag = 1.3f;
            BrakeParticle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.B) || !_isBraking)
        {
            _rb.drag = 0;
            _rb.angularDrag = 1.3f;
            BrakeParticle.Stop();
        }
           



	}
}
