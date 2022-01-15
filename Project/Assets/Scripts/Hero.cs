using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public const float LUANCH_FORCE = 400;
    public const float TRAJECTORY_LUANCH_FORCE = 4;
    const float MAX_DRAG_DISTANCE = 2;
    Vector2 startPosition;
    new Rigidbody2D rigidbody;

    [SerializeField] Trajectory trajectory;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPosition = rigidbody.position;
        rigidbody.isKinematic = true;
    }

    void OnMouseUp()
    {
        Vector2 currentPosition = rigidbody.position;
        Vector2 direction = startPosition - currentPosition;
        float distance = Vector2.Distance(startPosition, currentPosition);
        direction.Normalize();

        rigidbody.isKinematic = false;
        rigidbody.AddForce(direction * distance * LUANCH_FORCE);
        rigidbody.angularVelocity = -20;
        trajectory.Hide();
    }

    void OnMouseDown()
    {
        trajectory.Show();
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetPosition = mousePosition;
        Vector2 direction = targetPosition - startPosition;

        float distance = Vector2.Distance(targetPosition, startPosition);
        if (distance > MAX_DRAG_DISTANCE)
        {
            direction.Normalize();
            targetPosition = startPosition + (direction * MAX_DRAG_DISTANCE);
        }

        if (targetPosition.x > startPosition.x)
        {
            targetPosition.x = startPosition.x;
        }

        rigidbody.position = targetPosition;
        rigidbody.rotation = Vector2.SignedAngle(startPosition, targetPosition);

        float trajectoryDistance = Vector2.Distance(targetPosition, startPosition);
        Vector2 trajectoryDirection = startPosition - targetPosition;
        trajectory.UpdateDots(transform.position, trajectoryDistance * trajectoryDirection * TRAJECTORY_LUANCH_FORCE);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(OnHeroHit(collision));
    }

    private IEnumerator OnHeroHit(Collision2D collision)
    {
        yield return new WaitForSeconds(3);
        rigidbody.isKinematic = true;
        transform.position = startPosition;
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.rotation = 0;
        rigidbody.angularVelocity = 0;
    }

    void Update()
    {
       
    }
}
