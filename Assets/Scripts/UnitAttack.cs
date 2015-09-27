using UnityEngine;
using System.Collections;

public class UnitAttack : MonoBehaviour {

	public bool attack;
	public float speed;

	private Movement movement;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		movement = new Movement ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag != gameObject.tag) {
			anim.SetBool ("Attack", true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag != gameObject.tag) {
			anim.SetBool ("Attack", false);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag != gameObject.tag) {
			AttackEnemy(other);
		}
	}

	void AttackEnemy(Collider2D other){
		attack = true;
		movement.turnTowardsTarget (other.transform.position, transform);
		movement.gotoTarget (other.transform.position + new Vector3(2.0f,0.0f,0.0f), speed, transform);
	}

}
