using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private bool isPlayer;
    private bool isMortar;
    private GameObject target;
    private Vector3 targetPos;
    [SerializeField] private float lifespan = 5;
    [SerializeField] private GameObject explosion;
    [SerializeField] private List<Sprite> sprites;
    private SpriteRenderer sr;

    void Update()
    {
        lifespan -= Time.deltaTime;
        if (isPlayer)
        {
            if (isMortar && transform.localScale.x < 2 && target.transform.position.x - transform.position.x > 2.8f)
            {
                transform.localScale += new Vector3(1.6f, 1.6f, 0) * Time.deltaTime;
            }
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            if (isMortar && transform.localScale.x < 2 && transform.position.x - target.transform.position.x > 2.8f)
            {
                transform.localScale += new Vector3(1.6f, 1.6f, 0) * Time.deltaTime;
            }
            transform.position -= Vector3.right * speed * Time.deltaTime;
        }
        if (target != null && isMortar)
        {
            if (isPlayer && target.transform.position.x - transform.position.x < 2.8f)
            {
                transform.localScale -= new Vector3(1.6f, 1.6f, 0) * Time.deltaTime;
                if (transform.localScale.x <= 1)
                {
                    GameObject explosionObject = Instantiate(explosion, target.transform.position, Quaternion.identity, transform.parent);
                    target.GetComponent<PlacableObject>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
            else if (!isPlayer && transform.position.x- target.transform.position.x < 2.8f)
            {
                transform.localScale -= new Vector3(1.6f, 1.6f, 0) * Time.deltaTime;
                if (transform.localScale.x <= 1)
                {
                    GameObject explosionObject = Instantiate(explosion, target.transform.position, Quaternion.identity, transform.parent);
                    target.GetComponent<PlacableObject>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
        else if (isMortar)
        {
            Destroy(gameObject);
        }
        if(lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetUpProjectile(float projSpeed, float projDamage, bool isPlayerProj, int spriteNumber)
    {
        sr = GetComponent<SpriteRenderer>();
        speed = projSpeed;
        damage = projDamage;
        isPlayer = isPlayerProj;
        sr.sprite = sprites[spriteNumber];
        if (!isPlayer)
        {
            sr.flipX = true;
        }
    }

    public void SetUpProjectile(float projSpeed, float projDamage, bool isPlayerProj, float projLifespan, int spriteNumber)
    {
        sr = GetComponent<SpriteRenderer>();
        speed = projSpeed;
        damage = projDamage;
        isPlayer = isPlayerProj;
        lifespan = projLifespan;
        sr.sprite = sprites[spriteNumber];
        if (!isPlayer)
        {
            sr.flipX = true;
        }
    }

    public void SetUpProjectile(float projSpeed, float projDamage, bool isPlayerProj, int spriteNumber, bool isProjMortar, GameObject projTarget)
    {
        sr = GetComponent<SpriteRenderer>();
        speed = projSpeed;
        damage = projDamage;
        isPlayer = isPlayerProj;
        isMortar = isProjMortar;
        target = projTarget;
        targetPos = projTarget.transform.position;
        lifespan = 999f;
        GetComponent<CircleCollider2D>().enabled = false;
        sr.sprite = sprites[spriteNumber];
        sr.sortingOrder = 5;
        if (!isPlayer)
        {
            sr.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && isPlayer)
        {
            collision.gameObject.GetComponent<PlacableObject>().TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == 3 && !isPlayer)
        {
            collision.gameObject.GetComponent<PlacableObject>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8 && isPlayer)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 9 && !isPlayer)
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
