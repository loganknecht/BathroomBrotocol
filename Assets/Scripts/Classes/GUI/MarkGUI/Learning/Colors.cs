using UnityEngine;
using System.Collections;

public class Colors : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void makeRed()
    {
        GetComponent<UIWidget>().color = Color.red;
    }
    public void makeBlue()
    {
        GetComponent<UIWidget>().color = Color.blue;
    }
    public void makeGreen()
    {
        GetComponent<UIWidget>().color = Color.green;
    }
}
