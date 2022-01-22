using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiHero : MonoBehaviour
{
    const string ANIMATOR_IS_DEAD = "isDead";
    const float RELATIVE_VEL_OFFSET = 5;
    ParticleSystem particleSystem;
    Animator animator;
    [SerializeField] Material dissolveMaterial;
    int hitTime;
    public Action<float> onDestroyAction;
    [SerializeField] float point;
    [SerializeField] AudioSource auidoDeath;
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        hitTime = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag(Tags.HERO) || collision.gameObject.CompareTag(Tags.BOX)) && (collision.relativeVelocity.y > RELATIVE_VEL_OFFSET || collision.relativeVelocity.x > RELATIVE_VEL_OFFSET))
        {
            hitTime += 1;
        }

        if ((collision.gameObject.CompareTag(Tags.HERO) || collision.gameObject.CompareTag(Tags.BOX)) && (collision.relativeVelocity.y > RELATIVE_VEL_OFFSET || collision.relativeVelocity.x > RELATIVE_VEL_OFFSET))
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
            hitTime += 1;
            checkDeath(true);
        }

        if (collision.gameObject.CompareTag(Tags.BAT))
        {
            Destroy(collision.gameObject);
        }
    }
    void checkDeath(bool force)
    {
        if (GetComponent<Bean>() != null && hitTime < 2)
        {
            return;
        }

        if (GetComponent<Joker>() != null)
        {
            GetComponent<Joker>().isDead = true;
        }

        StartCoroutine(OnHeroHit(force));
    }

    IEnumerator OnHeroHit(bool force)
    {
        if (!force)
        {
            yield return new WaitForSeconds(1.5f);
        }

        animator.SetBool(ANIMATOR_IS_DEAD, true);

        yield return new WaitForSeconds(0.5f);
        auidoDeath.Play();
        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        onDestroyAction.Invoke(point);
    }
}
