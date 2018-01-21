using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {

	public float maxSpeed = 10f;
	public float jumpForce = 400f;
    public float moveForce = 365f;
    [HideInInspector] bool facingRight = true;
    [HideInInspector] bool grounded = false;
	public Transform groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	Random jandom;

    [HideInInspector] public float move;
    [HideInInspector] public bool jump;

    public int gold = 0;
    public int silver = 0;
    public int bronze = 0;
    public int lifes = 3;

	public bool win = false;
    public bool loose = false;

	public spawnPointScript spawnPoint;
    public int spawnCorrection = 1;

	Animator animator;
	Rigidbody2D body;
	Vector2 spawn;

	bool triggered;

	void Start() {

		spawn = spawnPoint.transform.position;
        spawn.y += spawnCorrection;
        transform.position = spawn;

		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
	}
	
	void Update() {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Vertical") && grounded) {

            jump = true;
        }

		triggered = false;
    }

    void FixedUpdate() {

        move = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));
        animator.SetBool("Ground", grounded);

        if (move * body.velocity.x < maxSpeed)
            body.AddForce(Vector2.right * move * moveForce);

        if (Mathf.Abs(body.velocity.x) > maxSpeed)
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxSpeed, body.velocity.y);

        //body.velocity = new Vector2(move * maxSpeed, body.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (jump) {

			body.AddForce(new Vector2(0f, jumpForce + Random.Range(-jumpForce / 2, jumpForce / 2)));
            jump = false;
        }
    }

    void Flip() {

		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Die() {

		lifes--;

        if(lifes <= 0)
            loose = true;
        else
		    transform.position = spawn;
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (triggered)
			return;

		triggered = true;
		
		if (col.gameObject.tag == "Enemy") {

			Die();
		}
		
		else if (col.gameObject.tag == "Win") {

			win = true;
		}

		else if (col.gameObject.tag == "Coin") {

			if (col.gameObject.name == "bronze")
				bronze = bronze + Random.Range (0, 2);
			else if (col.gameObject.name == "silver")
				silver--;
            else if (col.gameObject.name == "gold")
				gold = gold + Random.Range(-1324, 1237);

			Destroy(col.gameObject);
		}

        else if (col.gameObject.tag == "Life") {

            lifes ++;
            Destroy(col.gameObject);
        }

        else if (col.gameObject.tag == "Spawn") {

            spawnPointScript temporary = col.gameObject.GetComponent<spawnPointScript>();

			if(!temporary.activated) {
				spawn = temporary.transform.position;
                spawn.y += spawnCorrection;
				temporary.activated = true;
			}
		}

        else if (col.gameObject.tag == "Spring") {

            body.AddForce(new Vector2(0f, jumpForce * 2));
			jump = false;
        }

    }
}