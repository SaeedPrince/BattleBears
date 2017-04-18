using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Laser laserPrefab;
    public int team;
    public int health;
    public int attackPower;
    public bool bThereIsARedParticle;
    public Vector3 redParticleLocation;
    protected Animator anim;

    private Eye[] eyes;

	//We make this method virtual, so we CAN override it, in another class
    //We don't have to override it, but it's possible
	protected virtual void Start () {
        eyes = GetComponentsInChildren<Eye>();

        anim = GetComponentInChildren<Animator>();
        //We use our team variable as an index in the array. This way, team 0 gets red, team 1 gets green, team 2 gets blue
        Color teamColor = GameManager.instance.teamColors[team];
        //transform.Find uses the hierarchy path to access an object
        transform.Find("Teddy/Teddy_Body").GetComponent<Renderer>().material.color = teamColor;
	}

    /// <summary>
    /// Determines if I look towards hitPos, if I see hitObj
    /// </summary>
    /// <param name="hitPos"></param>
    /// <param name="hitObj"></param>
    /// <returns></returns>
    protected bool CanSee (Vector3 hitPos, Transform hitObj)
    {
        foreach (Eye eye in eyes)
        {
            //We start our ray at the eye
            Vector3 startPos = eye.transform.position;
            //We calculate the direction from the eye to the hitPosition
            Vector3 dir = hitPos - startPos;

            //We create a ray that start at startPos, with the direction dir
            Ray eyeRay = new Ray(startPos, dir);

            RaycastHit hitInfo;
            if (Physics.Raycast(eyeRay, out hitInfo))
            {
                //If the raycast hits the same object as provided, we can see it
                //If there would be an object inbetween, that would be a different transform
                if (hitInfo.transform == hitObj)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected void ShootLasers (Vector3 hitPos, Transform hitObj)
    {
        //Displays two linerenderers, one for each eye
        foreach(Eye eye in eyes)
        {
            Laser laserClone = Instantiate(laserPrefab);
            laserClone.Init(eye.transform.position, hitPos);
        }
        //If the object has a Unit component, we hit it and do damage
        Unit unit = hitObj.GetComponent<Unit>();
        if (unit != null)
        {
            //We call OnHit on THE OTHER unit
            unit.OnHit(attackPower);
        }
    }

    public void OnHit (int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    //We make the Die method virtual, so we can override it in the inheriting classes
    protected virtual void Die ()
    {
        anim.SetBool("Death", true);
    }
	
}
