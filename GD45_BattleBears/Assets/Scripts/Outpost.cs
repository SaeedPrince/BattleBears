using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outpost : MonoBehaviour {
    
    //internal is like public, but it doesn't show in the inspector
    //internal means it is only visible in this assembly
    internal int currentTeam;
    internal float captureValue;

    private float captureRate = .02f;

    private SkinnedMeshRenderer flag;

	// Use this for initialization
	void Start () {
        flag = GetComponentInChildren<SkinnedMeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Color flagColor = GameManager.instance.teamColors[currentTeam];
        //We set the flag to be white when not captured at all, 
        //and slowly transition into the team color as the captureValue increases
        flag.material.color = Color.Lerp(Color.white, flagColor, captureValue);

    }

    void OnTriggerStay (Collider otherCollider)
    {
        Unit unit = otherCollider.GetComponent<Unit>();
        //If the collider in our trigger area has a Unit component...
        if (unit != null)
        {
            if (unit.team == currentTeam)
            {
                //If the units team is the same as the flags team, we increase the captureValue (= raise the flag)
                captureValue += captureRate;
                if (captureValue >= 1)
                {
                    captureValue = 1;
                }
            } else
            {
                captureValue -= captureRate;
                //If the flag is fully 'decaptured' we swap the teams to whoever decaptured it
                if (captureValue <= 0)
                {
                    currentTeam = unit.team;
                    captureValue = 0;
                }
            }
        }
    }
}
