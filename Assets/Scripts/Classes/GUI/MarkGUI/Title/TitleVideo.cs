using UnityEngine;
using System.Collections;

public class TitleVideo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    public float timeUntilDemoVid;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Application.LoadLevel("GameMenu");
        }
        timeUntilDemoVid -= Time.deltaTime;
        if (timeUntilDemoVid < 0)
        {
            Application.LoadLevel("TitleVideo");
        }
    }
}
