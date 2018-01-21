﻿using UnityEngine;
using System.Collections;

public class enemyManScript : MonoBehaviour {

	public float speed = 7f;
	public float interval = 1;

	float timeCheck = 0;
	float direction = 1f;
	Animator animator;
	Rigidbody2D body;
	
	void Start() {
	
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {

		body.velocity = new Vector2 (speed * direction, body.velocity.y);
		transform.localScale = new Vector3 (direction, 1, 1);
		
		animator.SetFloat("Speed", Mathf.Abs(speed));

		if (Time.time >= timeCheck) {
		
			direction *= -1f;
			timeCheck += interval;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
    
		if (col.gameObject.tag == "Platform") {

            direction *= -1f;
            timeCheck += interval;
        }
    }
}
