using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batman : MonoBehaviour
{
    [SerializeField] GameObject batPrefab;
    bool hasShot = false;
    new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasShot && !rigidbody.isKinematic)
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = targetPosition - currentPosition;
            direction.Normalize();

            GameObject bat = Instantiate(batPrefab, null);
            bat.transform.position = transform.position;

            Rigidbody2D batRigidBody = bat.GetComponent<Rigidbody2D>();
            batRigidBody.angularVelocity = -1000;
            batRigidBody.AddForce(direction * 700);
        }
    }
}
