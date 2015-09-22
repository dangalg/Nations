using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour {

	public float speed;
	public bool isSelected;

	private Vector3 mousePosition;
	private Vector3 touchPosition;
	public GameObject selectionBox;

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
			
			// if enemy is at position attack
		} else {
			if (Input.GetMouseButtonDown (0) && clickedMe ()) {
				// make unit selected
				isSelected = true;
				setSelection (true);

			} else if (Input.GetMouseButtonDown (0) && !clickedMe ()) {
				isSelected = false;
				setSelection (false);
			}
		}

		gotoMousePosition(mousePosition);
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
	}
}
