using UnityEngine;
using System.Collections;

public class SplashLogic : MonoBehaviour {
    public float delayTime;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        delayTime -= Time.deltaTime;
        if (delayTime < 0)
        {
            Application.LoadLevel("Title");
        }
        if(Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Application.LoadLevel("Title");
        }
	}
}
