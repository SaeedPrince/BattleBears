using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngrySphere : MonoBehaviour
{

	// Use this for initialization
    public float thrust;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(0, 0, thrust, ForceMode.Impulse);
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider coll in hitColliders)
        {
            Unit otherUnit = coll.GetComponent<Unit>();
            if (otherUnit != null && otherUnit.health > 0)
            {
                otherUnit.OnHit(100);
            }
        }
    }
}
