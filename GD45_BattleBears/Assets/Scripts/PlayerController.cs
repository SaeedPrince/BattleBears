using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Unit {

    public float speed;
    public float jumpVelocity;
    public Transform camPivot;
    public RedParticle particlePrefab;
    public AngrySphere angryPrefab;

    private Rigidbody rb;

    private float respawnTime = 3f;
    private int startHealth;

	//We override the Start method in the Unit class.
	protected override void Start () {
        startHealth = health;
        //This calls the Start method in the Unit class.
        base.Start();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0)
            return;

        //Getting input from the keyboard
        /*
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontalInput, 0, verticalInput) * speed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            input.y = jumpVelocity;
            anim.SetTrigger("Jump");
        }
        else
        {
            //If we are not jumping, we make sure the velocity does not get reset to 0
            //Instead, we keep the velocity as is, and the Physics system will pull it down with gravity
            input.y = rb.velocity.y;
        }

        //transform.TransformVector will rotate the input vector, based on this transforms rotation.
        rb.velocity = transform.TransformVector(input);

        //Getting input from the Mouse - this is the mouse MOVEMENT in the two axes. (not POSITION)
        float mouseXInput = Input.GetAxis("Mouse X");
        float mouseYInput = Input.GetAxis("Mouse Y");

        transform.Rotate(0, mouseXInput, 0);

        camPivot.Rotate(-mouseYInput, 0, 0);

        anim.SetFloat("HorizontalInput", horizontalInput);
        anim.SetFloat("VerticalInput", verticalInput); 
        */

        /*
        if (Input.GetMouseButtonDown(0))
        {
            //We start our first ray at the camera pos and go forward (this is what's in front of the crosshair)
            Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            //hitInfo is null when we pass it as a parameter, BUT because we use the 'out' keyword,
            //it can be assigned from the Physics.Raycast method and that modifies the hitInfo object here
            if (Physics.Raycast(camRay, out hitInfo)) {
                //If teddies eyes can see the same object that I hit (and he's not blocked)
                if (CanSee(hitInfo.point, hitInfo.transform))
                {
                    //Display lasers and do damage
                    ShootLasers(hitInfo.point, hitInfo.transform);
                }
            }
        }
        */
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);
            if (Physics.Raycast(ray))
            {
                RedParticle theParticle = Instantiate( particlePrefab , hit.point, transform.rotation);
                redParticleLocation = hit.point;
                bThereIsARedParticle = true;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Casts the ray and get the first game object hit
            Physics.Raycast(ray, out hit);
            if (Physics.Raycast(ray))
            {
                Vector3 theDepart = Camera.main.transform.position;
                Vector3 theDestin = hit.point;
                Vector3 targetDir = theDestin - theDepart;
                Quaternion theRotation = Quaternion.FromToRotation(Vector3.up, theDestin - theDepart);
                AngrySphere theSphere = Instantiate(angryPrefab, theDepart, theRotation);
            }
        }


    }

    protected override void Die()
    {
        base.Die();
        Invoke("Respawn", respawnTime);
    }

    void Respawn ()
    {
        health = startHealth;
        anim.SetBool("Death", false);
    }
}
