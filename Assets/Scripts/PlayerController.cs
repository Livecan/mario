using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    // TODO Refactor
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var contactNormal = collision.GetContact(0).normal;
        bool hitTop = contactNormal.y < -0.5;
        bool hitGround = contactNormal.y > 0.5;
        bool hitSide = !hitTop && !hitGround;

        // todo: the point of contact must be - y == 0! or not the sides of x??
        if (standOnTags.Contains(collision.gameObject.tag)) {
            var contactPoint = (collision.GetContact(0).point + collision.GetContact(1).point) / 2;

            if (collision.gameObject.CompareTag("Bricks") && hitTop)
            {
                Tilemap bricks = collision.gameObject.GetComponent<Tilemap>();
                var tilePosition = bricks.WorldToCell(contactPoint);
                bricks.SetTile(tilePosition, null);
            }

            if (collision.gameObject.CompareTag("Crates") && hitTop)
            {
                collision.gameObject.GetComponent<PowerUpCrate>().DestroyTile(contactPoint);
                // TODO produce powerup
                // TODO remove the sprite from the tilemap
            }

            if (hitGround)
            {
                isOnGround = true;
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hitGround)
            {
                collision.gameObject.GetComponent<EnemyController>().Kill(EnemyController.KillType.JumpOn);
            }
            else
            {
                Destroy(GetComponent<Collider2D>());
                FindObjectOfType<GameManager>().GameOver();
            }
        }
    }
}
