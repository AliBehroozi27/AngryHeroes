using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public const float LUANCH_FORCE = 400;
    public const float TRAJECTORY_LUANCH_FORCE = 4;
    const float MAX_DRAG_DISTANCE = 2;
    Vector2 FLASH_VEL = new Vector2(20, 0);
    Vector2 startPosition;
    new Rigidbody2D rigidbody;

    public bool isReadyToLaunch;

    [SerializeField] public float point;
    [SerializeField] Trajectory trajectory;
    [SerializeField] AudioSource auidoDeath;

    public Action onLuanchAction;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rigidbody.isKinematic = true;
    }

    void OnMouseUp()
    {
        if (!isReadyToLaunch)
        {
            return;
        }

        Vector2 currentPosition = rigidbody.position;
        Vector2 direction = startPosition - currentPosition;
        float distance = Vector2.Distance(startPosition, currentPosition);
        direction.Normalize();

        rigidbody.isKinematic = false;

        if (GetComponent<Flash>() != null)
        {
            rigidbody.velocity = FLASH_VEL * new Vector2(1,0);
        }

        rigidbody.AddForce(direction * distance * LUANCH_FORCE * rigidbody.mass);
        rigidbody.angularVelocity = -20;
        trajectory.Hide();
        onLuanchAction?.Invoke();
    }

    void OnMouseDown()
    {
        if (!isReadyToLaunch)
        {
            return;
        }

        trajectory.Show();
    }

    void OnMouseDrag()
    {
        if (!isReadyToLaunch)
        {
            return;
        }

        startPosition = LevelManager.luanchPosition;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.BULLET))
        {
            destroy();
        }
    }

    private IEnumerator OnHeroHit(Collision2D collision)
    {
        yield return new WaitForSeconds(2);
        destroy();
    }

    void destroy()
    {
        auidoDeath.Play();
        gameObject.SetActive(false);
    }
}
