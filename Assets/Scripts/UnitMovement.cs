using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour {

	public float speed;
	public bool isSelected;
	public GameObject selectionBox;

	private Vector3 mousePosition;
	private Vector3 touchPosition;
	private bool move;
	private bool dragSelect;

	private Movement movement;

	// Use this for initialization
	void Start () {
		isSelected = false;
		mousePosition = transform.position;
		movement = new Movement ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){

//		if(Input.GetMouseButtonDown (1)){
//			mousePosition = getMousePosition ();
//			dragSelect = true;
//
//			// make selection box around soldiers and make all inside box selected
//
//		}
//		else 
		if (Input.GetMouseButtonUp (1) && isSelected) {
//			if(!dragSelect){
				// Move to click position
				mousePosition = getMousePosition ();
			movement.turnTowardsTarget (mousePosition, transform);
				move = true;
		
				// if enemy is at position attack
//			}
		} else {
			if (Input.GetMouseButtonDown (0) && selectedMe ()) {
				// make unit selected
				if(!isSelected){
					mousePosition = getMousePosition ();
					isSelected = true;
					setSelection (true);
				}else{
					mousePosition = getMousePosition ();
					isSelected = false;
					setSelection (false);
				}
				
			}else if(isSelected && selectedUnit()){
				// Do nothing because this is multi select
			}
			else if (isSelected && Input.GetMouseButtonDown (0) && !selectedMe ()) {
				mousePosition = getMousePosition ();
			}else if (isSelected && Input.GetMouseButtonUp (0) && !selectedMe ()) {
				if(!move && checkIfLastMouseClickCloseToCurrentMouseClick()){
					isSelected = false;
					setSelection (false);
				}
			}
		}

		if (move) {
			move = movement.gotoTarget (mousePosition, speed, transform);
		}
	}

	void setSelection(bool select){
		selectionBox.SetActive(select);
	}

	bool selectedMe() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit.collider !=null && hit.transform == transform) {
			return true;
		}
		return false;
	}

	bool selectedUnit(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit.collider !=null && hit.transform.tag == "Unit") {
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
}
