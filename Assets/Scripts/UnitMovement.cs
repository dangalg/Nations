using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour {

	public float speed;
	public bool isSelected;
	public GameObject selectionBox;

	private Vector3 mousePosition;
	private Vector3 touchPosition;
	private bool move;

	// Use this for initialization
	void Start () {
		isSelected = false;
		mousePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){

		if (Input.GetMouseButtonDown (1) && isSelected) {
			// Move to click position
			mousePosition = getMousePosition ();
			turnTowardsClickPosition (mousePosition);
			move = true;
			
			// if enemy is at position attack
		} else {
			if (Input.GetMouseButtonDown (0) && clickedMe ()) {
				// make unit selected
				mousePosition = getMousePosition ();
				isSelected = true;
				setSelection (true);
				
			} else if (isSelected && Input.GetMouseButtonDown (0) && !clickedMe ()) {
				mousePosition = getMousePosition ();
			}else if (isSelected && Input.GetMouseButtonUp (0) && !clickedMe ()) {
				if(!move && checkIfLastMouseClickCloseToCurrentMouseClick()){
					isSelected = false;
					setSelection (false);
				}
			}
		}

		if (move) {
			gotoMousePosition (mousePosition);
		}
	}

	void setSelection(bool select){
		selectionBox.SetActive(select);
	}

	bool clickedMe() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit.collider !=null && hit.transform == transform) {
			return true;
		}
		return false;
	}

	Vector3 getMousePosition(){
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return mousePosition;
	}

	bool checkIfLastMouseClickCloseToCurrentMouseClick(){
		Vector3 lastMousePosition = mousePosition;
		mousePosition = getMousePosition ();
		if(mousePosition.x <= lastMousePosition.x + 0.1f && 
		   mousePosition.x >= lastMousePosition.x - 0.1f &&
		   mousePosition.y <= lastMousePosition.y + 0.1f &&
		   mousePosition.y >= lastMousePosition.y - 0.1f){
			return true;
		}
		return false;
	}

	void turnTowardsClickPosition(Vector3 mousePosition){
		Quaternion rot = Quaternion.LookRotation (transform.position - mousePosition, Vector3.forward);
		transform.rotation = rot;
		transform.eulerAngles = new Vector3 (0.0f, 0.0f, transform.eulerAngles.z);
		GetComponent<Rigidbody2D> ().angularVelocity = 0;
	}

//	void OnMouseOver(){
//		if (Input.GetMouseButtonDown(0)) {
//			// make unit selected
//			isSelected = true;
//		}
//	}

	void gotoMousePosition(Vector3 target){
		target.z = 0.0f;
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
		if (transform.position == target) {
			move = false;
		}
	}
}
