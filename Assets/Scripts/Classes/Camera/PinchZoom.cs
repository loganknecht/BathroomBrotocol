using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour
{
	public float scale_factor= 0.2f;
	public float MAXSCALE = 4.1f, MIN_SCALE = 1.0f; // zoom-in and zoom-out limits
	private bool isMousePressed;
	// private bool isFingerPressed;
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private Vector2 midPoint = new Vector2(0,0);
	private Vector3 originalPos;
	private GameObject parentObject;

	void Start ()
	{
		// Game Object will be created and make current object as its child (only because we can set virtual anchor point of gameobject and can zoom in and zoom out from particular position)
		parentObject = new GameObject("ParentObject");
		parentObject.transform.parent = transform.parent;
		parentObject.transform.position = new Vector3(transform.position.x*-1, transform.position.y*-1, transform.position.z);

		transform.parent = parentObject.transform;
		originalPos = transform.position;
		isMousePressed = false;
		// isFingerPressed = false;

	}
	void Update ()
	{
		if (Input.GetMouseButtonDown (0))
			isMousePressed = true;
		else if (Input.GetMouseButtonUp (0))
			isMousePressed = false;

		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began && Input.GetTouch (0).tapCount == 1 )
		{
			return;
		}

		else if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved)
		{
			camera.transform.position += camera.transform.right * -Input.GetTouch (0).deltaPosition.x * 0.1f;
			camera.transform.position += camera.transform.up * -Input.GetTouch (0).deltaPosition.y * 0.1f;
		}
		// These lines of code will pan/drag the object around untill the edge of the image
		else if (isMousePressed)
		{
			camera.transform.position += camera.transform.right * -Input.GetAxis ("Mouse X") * 0.1f;
			camera.transform.position += camera.transform.up * -Input.GetAxis ("Mouse Y") * 0.1f;
		}

		// On device (only) double tap image will be set at original position and scale
		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began && Input.GetTouch (0).tapCount == 2 )
		{
			parentObject.transform.localScale = Vector3.one;
			parentObject.transform.position = new Vector3 (originalPos.x * -1, originalPos.y * -1, originalPos.z * -1);
			transform.position = originalPos;
		}

		//pc scrollWheel to zoom in and out.
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && camera.orthographicSize > MIN_SCALE)
		{
			camera.orthographicSize -= scale_factor;
		}
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0 && camera.orthographicSize < MAXSCALE)
		{
			camera.orthographicSize += scale_factor;
		}
		checkForMultiTouch();
	}

	// Following method check multi touch
	private void checkForMultiTouch()
	{
		// These lines of code will take the distance between two touches and zoom in - zoom out at middle point between them
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
		{
			midPoint = new Vector2(((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x)/2), ((Input.GetTouch(0).position.y + Input.GetTouch(1).position.y)/2));
			midPoint = Camera.main.ScreenToWorldPoint(midPoint);

			curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;

			//current distance between finger touches
			prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));

			//difference in previous locations using delta positions
			float touchDelta = curDist.magnitude - prevDist.magnitude;

			// device Zoom out
			if(touchDelta<0)
			{
				if(camera.orthographicSize < MAXSCALE)
				{
					camera.orthographicSize = camera.orthographicSize+scale_factor;
				}
			}

			//device Zoom in
			else if(touchDelta>0 )
			{
				if(camera.orthographicSize > MIN_SCALE)
				{
					camera.orthographicSize = camera.orthographicSize-scale_factor;
				}
			}

			//mouse zoom out
			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				if(camera.orthographicSize < MAXSCALE)
				{
					camera.orthographicSize = camera.orthographicSize+scale_factor;
				}
			}

			//mouse zoom in
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if(camera.orthographicSize > MIN_SCALE)
				{
					camera.orthographicSize = camera.orthographicSize+scale_factor;
				}
			}

		}

		// On touch end just check whether image is within screen or not, or reset to closest edge
		else if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended ))
		{
//			if(camera.transform.position.x >-3 || camera.transform.position.x <15)
//			{
//				//camera.transform.position.x = -8;
//				//camera.transform.position.y = -10;
//			}
		}
	}
}
