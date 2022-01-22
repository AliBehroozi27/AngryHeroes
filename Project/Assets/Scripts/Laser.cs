using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    const float LASER_SPEED = 10f;
    const float DISTANCE = 50;
    private float counter;

    private LineRenderer lineRenderer;

    internal void draw(Vector2 currentPosition, Vector2 targetPosition)
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetWidth(0.05f, 0.1f);
        }

        Vector2 direction = targetPosition - currentPosition;
        direction.Normalize();

        lineRenderer.SetPosition(0, currentPosition);
        lineRenderer.SetPosition(1, targetPosition * 1000);


        /*if (counter < DISTANCE)
        {
            counter += 1f / LASER_SPEED;
            float x = Mathf.Lerp(0, DISTANCE, counter);

            Vector2 point = x * direction + currentPosition;

        }*/
    }

    public void update(Vector2 currentPosition)
    {
        lineRenderer.SetPosition(0, currentPosition);
    }

    internal void stop()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }
}
