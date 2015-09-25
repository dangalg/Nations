using UnityEngine;
using System.Collections;

public class UnitAttack : MonoBehaviour {

	public bool attack;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != gameObject.tag) {
			AttackEnemy(other);
		}
	}

	void AttackEnemy(Collider other){
		attack = true;
		MoveTowardsEnemyPosition ();
	}

	void MoveTowardsEnemyPosition(){
		
	}

	// Applies an upwards force to all rigidbodies that enter the trigger.
	void OnTriggerStay(Collider other) {
		if (other.attachedRigidbody)
			other.attachedRigidbody.AddForce(Vector3.up * 10);        
	}

}
