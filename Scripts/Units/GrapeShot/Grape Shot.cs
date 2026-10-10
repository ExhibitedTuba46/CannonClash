using Unity.Mathematics;
using UnityEngine;

public class GrapeShot : PlacableObject
{
    [SerializeField] private int rangeInTiles;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private GameObject proj;

    protected override void Start()
    {
        base.Start();
        
        if (isPlayer)
        {
            targetMask = 64;
            gameObject.layer = 3;
        }
        else
        {
            targetMask = 8;
            gameObject.layer = 6;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !isPlayer;
        timerBeforeFire = fireRate / 2;
    }

    protected override void Update()
    {
        base.Update();

        if (!isPlacedDown)
        {
            return;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(rangeInTiles * 1.6f, 0f, 0), Color.red);
            timerBeforeFire -= Time.deltaTime;
            if (timerBeforeFire <= 0)
            {
                RaycastHit2D[] objectsInSight;
                if (isPlayer)
                {
                    objectsInSight = Physics2D.BoxCastAll(transform.position, new Vector2((rangeInTiles * 1.6f), 4.6f), 0, Vector2.right, rangeInTiles * 0.8f, targetMask);
                    Debug.DrawLine(transform.position, transform.position + new Vector3(rangeInTiles * 1.6f, 0f, 0), Color.red);
                }
                else
                {
                    objectsInSight = Physics2D.BoxCastAll(transform.position, new Vector2((rangeInTiles * 1.6f), 4.6f), 0, Vector2.left, rangeInTiles * 0.8f, targetMask);
                }
                if (objectsInSight.Length > 0)
                {
                    if (PlayAnimation(attackAnimation))
                    {
                        Attack();
                    }
                }
            }
            else
            {
                idleTime = Time.time;
            }
        }
    }

    protected override void UpdateIndicator()
    {
        base.UpdateIndicator();

        indicator.transform.position = new Vector3(transform.position.x + 3.2f, transform.position.y, transform.position.z);
    }

    protected override void Attack()
    {
        for(int i = -1; i < 2; i++)
        {
            GameObject projectile = Instantiate(proj, transform.position + new Vector3(0, i * 1.6f, 0), quaternion.identity, transform.parent);
            float projectileLifespan = (rangeInTiles * 1.6f) / speed;
            projectile.GetComponent<Projectile>().SetUpProjectile(speed, damage, isPlayer, projectileLifespan, 2);
        }
        timerBeforeFire = fireRate;
    }
}
