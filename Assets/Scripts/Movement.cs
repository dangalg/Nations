using UnityEngine;
using System.Collections;

public class Movement {

	public void turnTowardsTarget(Vector3 target, Transform transform){
		Quaternion rot = Quaternion.LookRotation (transform.position - target, Vector3.forward);
		transform.rotation = rot;
		transform.eulerAngles = new Vector3 (0.0f, 0.0f, transform.eulerAngles.z);
		transform.GetComponent<Rigidbody2D> ().angularVelocity = 0;
	}
	
	public bool gotoTarget(Vector3 target, float speed, Transform transform){
		target.z = 0.0f;
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
		if (transform.position == target) {
			return false;
		}
		return true;
	}
}
