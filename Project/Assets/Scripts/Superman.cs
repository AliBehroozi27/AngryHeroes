using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superman : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    
    bool isShooting = false;
    bool hasShot = false;
    new Rigidbody2D rigidbody;
    Vector2 targetPosition;
    private Laser laser;
    [SerializeField] AudioSource auidoLaser;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasShot && !rigidbody.isKinematic)
        {
            auidoLaser.Play();
            isShooting = true;
            hasShot = true;
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            laser = Instantiate(laserPrefab, null).GetComponent<Laser>();
            laser.draw(transform.position, targetPosition);
            StartCoroutine(stopLaser());
        }

        if (isShooting)
        {
            laser.update(rigidbody.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isShooting = false;
        if (laser != null)
        {
            laser.stop();
            Destroy(laser);
        }
    }

    private IEnumerator stopLaser()
    {
        yield return new WaitForSeconds(0.5f);
        if (laser != null)
        {
            laser.stop();
        }
        isShooting = false;
    }
}
