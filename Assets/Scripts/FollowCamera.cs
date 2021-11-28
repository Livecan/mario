using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject followedObject;

    // Update is called once per frame
    void LateUpdate()
    {
        if (followedObject.transform.position.x > transform.position.x)
        {
            transform.position = new Vector3(followedObject.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
