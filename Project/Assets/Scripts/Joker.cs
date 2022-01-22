using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : MonoBehaviour
{
    Vector3 firstPosition, secondPosition;
    [SerializeField] GameObject firstBox, secondBox;
    [SerializeField] float waitTime = 1f;
    private IEnumerator coroutine;

    public bool isDead = false;
    void Start()
    {
        updatePositions();
        coroutine = changePosition(true);
        StartCoroutine(coroutine);
    }

    void Update()
    {
        updatePositions();
    }

    private void updatePositions()
    {
        firstPosition = new Vector3(firstBox.transform.position.x, firstBox.transform.position.y + firstBox.GetComponent<BoxCollider2D>().bounds.size.y, firstBox.transform.position.z);
        secondPosition = new Vector3(secondBox.transform.position.x, secondBox.transform.position.y + secondBox.GetComponent<BoxCollider2D>().bounds.size.y, secondBox.transform.position.z);
    }
    private IEnumerator changePosition(bool position)
    {
        yield return new WaitForSeconds(waitTime);

        if (isDead)
        {
            StopCoroutine(coroutine);
        }

        if (position)
        {
            transform.position = secondPosition;
            coroutine = changePosition(false);
        }
        else
        {
            transform.position = firstPosition;
            coroutine = changePosition(true);
        }

        if (!isDead)
        {
            StartCoroutine(coroutine);
        }
    }
}
