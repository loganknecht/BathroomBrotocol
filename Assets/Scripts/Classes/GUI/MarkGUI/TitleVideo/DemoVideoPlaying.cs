using UnityEngine;
using System.Collections;

public class DemoVideoPlaying : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public float timeUntilDemoVidDone;
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Application.LoadLevel("Title");
        }
        timeUntilDemoVidDone -= Time.deltaTime;
        if (timeUntilDemoVidDone < 0)
        {
            Application.LoadLevel("Title");
        }
	}
}
