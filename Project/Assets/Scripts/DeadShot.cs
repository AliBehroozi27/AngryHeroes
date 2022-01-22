using System.Collections;
using UnityEngine;

public class DeadShot : MonoBehaviour
{
    const string ANIMATOR_IS_SHOOTING = "isShooting";
    GameObject bullet;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioSource auidoBullet;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        
        float waitTime = Random.Range(2.5f, 4f);
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger(ANIMATOR_IS_SHOOTING);
        yield return new WaitForSeconds(0.6f);
        if (bullet != null)
        {
            Destroy(bullet);
        }
        bullet = Instantiate(bulletPrefab, null);
        bullet.transform.position = transform.position;
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = new Vector2(Random.RandomRange(-1, 1), Random.RandomRange(-1, 1));
        Vector2 direction = targetPosition - currentPosition;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().AddForce(direction * 300);
        auidoBullet.Play();
        StartCoroutine(Shoot());
    }
}
