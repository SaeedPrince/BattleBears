using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedParticle : Unit
{
    // Use this for initialization
    protected override void Start()
    {
        Destroy(gameObject, 5);
        bThereIsARedParticle = true;
    }

	// Update is called once per frame
	void Update ()
    {

    }

    private void OnDestroy()
    {
        bThereIsARedParticle = false;
    }
}
