using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Unit {

    public float range;
    public float shootInterval;

    //We create an offset, so the lasers don't shoot at the feet of the other teddy, but the chest.
    private Vector3 shootOffset = new Vector3(0, 1.5f);
    
    private NavMeshAgent agent;

    //When we create an enum variable, this creates a new type of variable
    //In this case, the type is 'State'
    //The values that we define inside the brackets, are the values that this State variable can have
    private enum State
    {
        Idle,
        MovingToOutpost,
        ChasingEnemy,
        MovingToParticle
    }

    private State currentState;

    private Outpost currentOutpost;
    private Unit currentEnemy;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        SetState(State.Idle);
    }

    private void SetState (State newState)
    {
        //When we switch states, we stop all the current Coroutines, 
        //so we can never be in two states at the same time
        StopAllCoroutines();
        currentState = newState;
        /*
        switch (currentState)
        {
            case State.Idle:
                StartCoroutine(OnIdle());
                break;
            case State.MovingToOutpost:
                StartCoroutine(OnMovingToOutpost());
                break;
            case State.ChasingEnemy:
                StartCoroutine(OnChasingEnemy());
                break;
        }
        */
        switch (currentState)
        {
            case State.Idle:
                StartCoroutine(OnIdle());
                break;
            case State.MovingToOutpost:
                StartCoroutine(OnMovingToOutpost());
                break;
            case State.ChasingEnemy:
                StartCoroutine(OnChasingEnemy());
                break;
            case State.MovingToParticle:
                StartCoroutine(OnMovingToParticle());
                break;
        }


    }

    IEnumerator OnMovingToParticle()
    {
        agent.SetDestination(redParticleLocation);
        while(bThereIsARedParticle)
        {
            SetState(State.MovingToParticle);
            yield return null;
        }
        SetState(State.Idle);
    }


    //A method that returns an IEnumerator is called a Coroutine.
    //Coroutines function like regular methods, BUT, 
    //they have the ability to pause execution in the middle of the method
    /*
    IEnumerator OnIdle ()
    {
        //This effectively creates a second update loop. Everything in the while loop gets called every frame
        while (currentOutpost == null)
        {
            LookForOutpost();
            //This pauses the execution for one frame
            yield return null;
        }

        SetState(State.MovingToOutpost);
    }
    */
    public void GoToParticle()
    {
        agent.SetDestination(redParticleLocation);
        SetState(State.MovingToParticle);
    }

    IEnumerator OnIdle()
    {
        //This effectively creates a second update loop. Everything in the while loop gets called every frame
        if (team == 0)
        {
            while (bThereIsARedParticle)
            {
                GoToParticle();
                SetState(State.MovingToParticle);
                //This pauses the execution for one frame
                yield return null;
            }

            while (currentOutpost == null)
            {
                LookForOutpost();
                //This pauses the execution for one frame
                yield return null;
            }

            SetState(State.MovingToOutpost);
        }
        else
        {
            while (currentOutpost == null)
            {
                LookForOutpost();
                //This pauses the execution for one frame
                yield return null;
            }

            SetState(State.MovingToOutpost);
        }
    }

    IEnumerator OnMovingToOutpost()
    {
        //This is only executed once (the outpost isn't moving)
        agent.SetDestination(currentOutpost.transform.position);

        //As long as captureValue is not one or the currentTeam is not my team
        while (!(currentOutpost.captureValue == 1 && currentOutpost.currentTeam == team))
        {
            LookForEnemies();
            //This pauses the execution for one frame
            yield return null;
        }

        //When the outpost is captured, we go back to Idle
        currentOutpost = null;
        SetState(State.Idle);
    }

    IEnumerator OnChasingEnemy()
    {
        //Stop moving
        agent.ResetPath();
        float shootTimer = 0;

        //Always keep chasing until the enemy is dead
        while (currentEnemy.health > 0)
        {
            //We increment the shootTimer by the duration of the last frame
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0;
                ShootLasers(currentEnemy.transform.position + shootOffset, currentEnemy.transform);
            }
            //This pauses the execution for one frame
            yield return null;
        }

        //When the enemy is dead, we set it to null and go back to Idle
        currentEnemy = null;
        SetState(State.Idle);
    }

    void LookForOutpost ()
    {
        //We get an integer ranging from 0 until the total amount of outposts
        //Regardless of the amount of outposts we have, this can be used to select a random one
        int r = Random.Range(0, GameManager.instance.outposts.Length);
        currentOutpost = GameManager.instance.outposts[r];
    }

    void LookForEnemies ()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, range);
        foreach(Collider c in surroundingColliders)
        {
            Unit otherUnit = c.GetComponent<Unit>();
            if (otherUnit != null && otherUnit.team != team && otherUnit.health > 0)
            {
                currentEnemy = otherUnit;
                SetState(State.ChasingEnemy);
                return; //We can use break here as well
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //PlayerController player = FindObjectOfType<PlayerController>();
        //agent.SetDestination(player.transform.position);
        if(team==0)
        {
            //Debug.Log("boolParticle=" + bThereIsARedParticle);
        }
        if (bThereIsARedParticle && team==0 )
        {
            //Debug.Log("MovingToParticle");
            StopAllCoroutines();
            SetState(State.MovingToParticle);
        }
        anim.SetFloat("VerticalInput", agent.velocity.magnitude);
	}

    protected override void Die()
    {
        base.Die();
        agent.Stop();
        StopAllCoroutines();
        Destroy(GetComponent<Collider>());
    }
}
