using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    const float RELATIVE_VEL_OFFSET = 5;
    [SerializeField] float point;
    public Action<float> onDestroyAction;
    [SerializeField] AudioSource auidoDestroy;


    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag(Tags.HERO) && (collision.relativeVelocity.y > RELATIVE_VEL_OFFSET || collision.relativeVelocity.x > RELATIVE_VEL_OFFSET))
        {
            checkDeath(true);
        }

        if ((collision.gameObject.CompareTag(Tags.PLATFORM) && collision.relativeVelocity.y > RELATIVE_VEL_OFFSET))
        {
            checkDeath(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.BAT) || collision.gameObject.CompareTag(Tags.LASER))
        {
            checkDeath(true);
        }
        if (collision.gameObject.CompareTag(Tags.BAT))
        {
            Destroy(collision.gameObject);
        }
    }
    void checkDeath(bool force)
    {
        StartCoroutine(OnHeroHit(force));
    }

    IEnumerator OnHeroHit(bool force)
    {
        if (!force)
        {
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        auidoDestroy.Play();
        gameObject.SetActive(false);
        onDestroyAction.Invoke(point);
    }
}
