using System.Collections.Generic;
using UnityEngine;

public class Boarder : PlacableObject
{
    [SerializeField] private List<Sprite> walkingAnim;
    [SerializeField] private List<Sprite> boatAnim;
    [SerializeField] private List<Sprite> boatAttack;
    [SerializeField] private LayerMask targetMask;
    private RaycastHit2D hit;
    private bool isOnWater;
    protected override void Start()
    {
        base.Start();
        
        if (isPlayer)
        {
            targetMask = 320;
            gameObject.layer = 3;
        }
        else
        {
            targetMask = 520;
            gameObject.layer = 6;
        }
        gameObject.GetComponent<SpriteRenderer>().flipX = !isPlayer;
        timerBeforeFire = fireRate;
    }

    protected override void Update()
    {
        base.Update();
        if (!isPlacedDown)
        {
            return;
        }
        timerBeforeFire -= Time.deltaTime;
        if(Physics2D.Raycast(transform.position, Vector2.right, 0.1f, 128).collider.gameObject.GetComponent<Tile>().GetTileType() == Tile.TileType.Ocean)
        {
            isOnWater = true;
        }
        else
        {
            isOnWater = false;
        }
        if (isPlayer)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right, 0.8f, targetMask);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left, 0.8f, targetMask);
        }
        if (timerBeforeFire <= 0)
        {
            if (hit)
            {
                if (isOnWater)
                {
                    if (PlayAnimation(boatAttack))
                    {
                        Attack();
                    }
                    return;
                }
                else
                {
                    if (PlayAnimation(attackAnimation))
                    {
                        Attack();
                    }
                    return;
                }

            }
        }
        if (!hit)
        {
            if (isPlayer)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if (isOnWater)
            {
                PlayAnimation(boatAnim);
                return;
            }
            else
            {
                PlayAnimation(walkingAnim);
                return;
            }
        }
        idleTime = Time.time;
    }

    protected override void Attack()
    {
        if (hit.collider.gameObject.layer == 3 || hit.collider.gameObject.layer == 6)
        {
            hit.collider.gameObject.GetComponent<PlacableObject>().TakeDamage(damage);
            timerBeforeFire = fireRate;
        }
        if (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 9)
        {
            hit.collider.gameObject.GetComponent<HealthController>().TakeDamage(damage * 8);
            Destroy(gameObject);
        }
    }
}
