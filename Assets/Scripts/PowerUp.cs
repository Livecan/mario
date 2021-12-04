using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    Rigidbody2D rigidbody;
    float speed = 2.5f;
    Vector3 direction = Vector3.right;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.transform.position += direction * Time.fixedDeltaTime * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var contactPointCount = collision.contactCount;
        ContactPoint2D[] contactPoints = new ContactPoint2D[contactPointCount];
        collision.GetContacts(contactPoints);
        //var contactNormal = collision.GetContact(0).normal;

        //Debug.Log(contactNormal.x + ", " + contactNormal.y);

        if (contactPoints.Any(contactPoint => contactPoint.normal.x < -0.5f))
        {
            direction = Vector3.left;
        }
        else if (contactPoints.Any(contactPoint => contactPoint.normal.x > 0.5f))
        {
            direction = Vector3.right;
        }
    }
}
