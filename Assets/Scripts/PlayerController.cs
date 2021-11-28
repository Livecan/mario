using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float normalspeed = 4.0f;
    float runningSpeed = 8.0f;
    float currentSpeed;

    float jumpForce = 1700f;

    [SerializeField]
    string[] standOnTags;

    bool isOnGround = false;

    float horizontal = 0;
    bool jump = false;
    bool fire = false;

    float allowedXMovement = 10;

    float leftBoundary;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentSpeed = normalspeed;
        leftBoundary = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        jump |= Input.GetButtonDown("Jump") && isOnGround;

        fire = Input.GetButton("Fire1");

        leftBoundary = Mathf.Max(leftBoundary, transform.position.x - allowedXMovement);
    }

    private void FixedUpdate()
    {
        if (!fire)
        {
            currentSpeed = Mathf.Max(normalspeed, currentSpeed - 10 * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.Min(runningSpeed, currentSpeed + 10 * Time.fixedDeltaTime);
        }

        if (transform.position.x > leftBoundary || horizontal > 0)
        {
            transform.position += Vector3.right * currentSpeed * Time.deltaTime * horizontal;
        }

        if (jump)
        {
            jump = false;
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (standOnTags.Contains(collision.gameObject.tag))
        {
            isOnGround = true;
        }
    }
}
