using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject map;
	public static bool isPanning; 

	private float orthoZoomSpeed = 0.15f;        // The rate of change of the orthographic size in orthographic mode.
	private float orthoCamSize;
	private Vector3 startCamPos;
	private float maxZoom;
	private float minZoom = 2;
	private float panSpeed = -0.4f;
	private float screenWidth;
	private float SideMenuWidth;
	       // Is the camera being panned?
	
	Vector3 mapBottomLeft;
	Vector3 mapTopRight;
	
	Vector3 cameraTopRight;
	Vector3 cameraBottomLeft;

	void Start()
	{
		
		float mapWidth = map.GetComponent<MeshRenderer> ().bounds.size.x;
		float mapHeight = map.GetComponent<MeshRenderer>().bounds.size.y;
		screenWidth = Screen.width;
		SideMenuWidth = mapWidth * 0.1953f;
		isPanning = false;
		maxZoom = Camera.main.orthographicSize;
		orthoCamSize = maxZoom;
		startCamPos = Camera.main.transform.position;
		
		//set max map bounds (assumes camera is max zoom and centered on Start)
		mapTopRight = new Vector3(map.transform.position.x + (mapWidth / 2), map.transform.position.y + (mapHeight / 2), map.transform.position.z);
		mapBottomLeft = new Vector3(map.transform.position.x - (mapWidth / 2), map.transform.position.y - (mapHeight / 2), map.transform.position.z);

		getCameraPoints ();
	}

	void getCameraPoints(){

		Camera cam = Camera.main;
		float camHeight = 2f * cam.orthographicSize;
		float camWidth = camHeight * cam.aspect;
		cameraTopRight = new Vector3(cam.transform.position.x + (camWidth / 2), cam.transform.position.y + (camHeight / 2), cam.transform.position.z);
		cameraBottomLeft = new Vector3(cam.transform.position.x - (camWidth / 2), cam.transform.position.y - (camHeight / 2), cam.transform.position.z);
		Debug.Log ("camera");

	}
	
	void Update ()
	{
		Debug.Log ("Mouse X: " + Input.mousePosition.x + " Mouse Y: " + Input.mousePosition.y);
		#if UNITY_EDITOR
		//click and drag
		if(Input.GetMouseButton(0)){
			isPanning = true;
			float x = Input.GetAxis("Mouse X") * panSpeed;
			float y = Input.GetAxis("Mouse Y") * panSpeed;
			transform.Translate(x,y,0);
			isPanning = false;
		}
		#endif
		
		
		// One Finger Pan
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved){
			isPanning = true;
			//            Touch touchZero = Input.GetTouch(0);
			//            float x = touchZero.position.x * panSpeed;
			//            float y = touchZero.position.y * panSpeed;
			float x = Input.GetAxis("Mouse X") * panSpeed;
			float y = Input.GetAxis("Mouse Y") * panSpeed;
			transform.Translate(x,y,0);
			isPanning = false;
		}
		
		#if UNITY_EDITOR
		//zoom
		if((Input.GetAxis("Mouse ScrollWheel") > 0) && Camera.main.orthographicSize > minZoom ) // forward
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize - orthoZoomSpeed;
			getCameraPoints();
		}
		
		if ((Input.GetAxis("Mouse ScrollWheel") < 0) && Camera.main.orthographicSize < maxZoom) // back          
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize + orthoZoomSpeed;
		}

		#endif
		
		// 2 finger Zoom
		if (Input.touchCount == 2){
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			
			// If the camera is orthographic...
			if (Camera.main.orthographic)
			{
				// ... change the orthographic size based on the change in distance between the touches.
				Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				
				// Make sure the orthographic size never drops below zero.
				Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, minZoom);
				
				// Make sure the orthographic size never goes above original size.
				Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, maxZoom);
			}
		}
		
		
		// On double tap image will be set at original position and scale
		else if(Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).tapCount==2)
		{
			Camera.main.orthographicSize = orthoCamSize;
			Camera.main.transform.position = startCamPos;

		}
		
		
		//check if camera is out-of-bounds, if so, move back in-bounds

		//check if out of bounds

		getCameraPoints();

		//out of right side
		if(mapTopRight.x < cameraTopRight.x)
		{
			float subtractNum = Mathf.Abs((Mathf.Abs(cameraTopRight.x) - Mathf.Abs(mapTopRight.x)));
			transform.position = new Vector3(transform.position.x - subtractNum, transform.position.y, transform.position.z);
		}

		//out of top
		if(mapTopRight.y < cameraTopRight.y)
		{
			float subtractNum = Mathf.Abs((Mathf.Abs(cameraTopRight.y) - Mathf.Abs(mapTopRight.y)));
			transform.position = new Vector3(transform.position.x, transform.position.y - subtractNum, transform.position.z);
		}

		//out of left
		if(mapBottomLeft.x > cameraBottomLeft.x)
		{
			float subtractNum = Mathf.Abs((Mathf.Abs(cameraBottomLeft.x) - Mathf.Abs(mapBottomLeft.x)));
			if(transform.position.x < 0){ subtractNum *= -1; }
			transform.position = new Vector3(transform.position.x - subtractNum, transform.position.y, transform.position.z);
		}

		//out of bottom
		if(mapBottomLeft.y > cameraBottomLeft.y)
		{
			float subtractNum = Mathf.Abs((Mathf.Abs(cameraBottomLeft.y) - Mathf.Abs(mapBottomLeft.y)));
			if(transform.position.y < 0){ subtractNum *= -1; }
			transform.position = new Vector3(transform.position.x, transform.position.y - subtractNum, transform.position.z);
		}
//		
		
		// If back button press andriod
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		
	}

//	public float speed = 0.05F;
//	void Update() {
//		
//	}
}
