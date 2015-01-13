using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class ChallengeMode : MonoBehaviour {
    
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Selectlevel(string Level)
    {
        Application.LoadLevel("Challenge" + Level); //load the level
    }
}
