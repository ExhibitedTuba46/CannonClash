using UnityEngine;

public class BasicTower : PlacableObject
{
    [SerializeField] protected GameObject proj;
    protected override void Start()
    {
        base.Start();

        if (isPlayer)
        {
            gameObject.layer = 3;
        }
        else
        {
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
            timerBeforeFire -= Time.deltaTime;
            if (timerBeforeFire <= 0)
            {
                if (PlayAnimation(attackAnimation))
                {
                    Attack();
                }
            }
            else
            {
                idleTime = Time.time;
            }
        }
    }

    protected override void Attack()
    {
        Vector3 offset;
        if (isPlayer)
        {
            offset = new Vector3(0.8f, 0, 0);
        }
        else
        {
            offset = new Vector3(-0.8f, 0, 0);
        }
            GameObject projectile = Instantiate(proj, transform.position + offset, Quaternion.identity, transform.parent);
        timerBeforeFire = fireRate;
        projectile.GetComponent<Projectile>().SetUpProjectile(speed, damage, isPlayer, 0);
    }
}
