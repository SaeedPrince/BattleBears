using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Color[] teamColors;

    internal Outpost[] outposts;

    void Awake ()
    {
        //We assign the value of instance, to point towards this object
        //Now other scripts can easily access this object by writing e.g. GameManager.instance.teamColors;
        GameManager.instance = this;
        outposts = FindObjectsOfType<Outpost>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
