using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // REFACTOR Split direction movement and point movement into two different scripts
    public enum Direction { Left, Right };

    public enum KillType { JumpOn };

    public Direction movementDirection;

    public List<Vector2> points;

    private int nextPoint = 0;

    public float speed;

    Rigidbody2D rigidbody;

    private bool isDead = false;

    private float destroyYRange = -10;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            if (points.Count == 0)
            // movement according to movement direction
            {
                rigidbody.transform.position += (movementDirection == Direction.Left ? Vector3.left : Vector3.right) * speed * Time.fixedDeltaTime;
            }
            else
            // movement following the points
            {
                Vector3 nextPosition = new Vector3(points[nextPoint].x, points[nextPoint].y);
                Vector3 deltaPosition = (nextPosition - rigidbody.transform.position).normalized * speed * Time.fixedDeltaTime;
                rigidbody.transform.position += deltaPosition;
                float distanceFromTarget = Mathf.Abs((nextPosition - rigidbody.transform.position).magnitude);
                if (distanceFromTarget < Mathf.Abs(deltaPosition.magnitude))
                {
                    nextPoint++;
                    nextPoint %= points.Count;
                }
            }
        }
        else
        {
            if (transform.position.y < destroyYRange)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Kill(KillType killType)
    {
        isDead = true;

        Destroy(GetComponent<Collider2D>());
    }
}
