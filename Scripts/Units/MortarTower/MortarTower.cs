using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.Collections;

public class MortarTower : PlacableObject
{
    //Start of range in tiles in front of tower
    [SerializeField] private int rangeStart;
    //Range end in tiles in front of tower
    [SerializeField] private int rangeEnd;
    private GameObject target;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] protected GameObject proj;

    [SerializeField] private GameObject aiRangeIndicatorPrefab;

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
        if (!isPlayer)
        {
            indicator = Instantiate(aiRangeIndicatorPrefab, transform.position, Quaternion.identity, transform);
            indicator.transform.localScale = Vector3.one;
            SpriteRenderer sr = indicator.GetComponent<SpriteRenderer>();
            sr.enabled = true;
            CalculateIndicatorPos();
        }
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
            Debug.DrawLine(transform.position + new Vector3(0.8f + rangeStart * 1.6f, 0, 0), transform.position + new Vector3(0.8f + rangeStart * 1.6f, 0, 0) + new Vector3((rangeEnd - rangeStart) * 1.6f, 0, 0), Color.red);
            timerBeforeFire -= Time.deltaTime;
            if (timerBeforeFire <= 0)
            {
                RaycastHit2D[] objectsInSight;
                if (isPlayer)
                {
                    objectsInSight = Physics2D.BoxCastAll(transform.position + new Vector3(0.8f + rangeStart * 1.6f + 1.6f, 0, 0), new Vector2((rangeEnd - rangeStart) * 1.6f - 0.2f, 1.4f), 0, Vector2.right, 0, targetMask);
                }
                else
                {
                    objectsInSight = Physics2D.BoxCastAll(transform.position - new Vector3(0.8f + rangeStart * 1.6f + 1.6f, 0, 0), new Vector2((rangeEnd - rangeStart) * 1.6f - 0.2f, 1.4f), 0, Vector2.left, 0, targetMask);
                }
                if (objectsInSight.Length > 0)
                {
                    if (PlayAnimation(attackAnimation))
                    {
                        target = objectsInSight[0].collider.gameObject;
                        Attack();
                    }
                    else
                    {
                        objectsInSight = new RaycastHit2D[0];
                    }
                }
                else
                {
                    sr.sprite = attackAnimation[0];
                    idleTime = Time.time;
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
        
        indicator.transform.position = new Vector3(transform.position.x + 8.8f, transform.position.y, transform.position.z);
        CalculateIndicatorPos();
    }

    protected override void Attack()
    {
        GameObject projectile = Instantiate(proj, transform.position, Quaternion.identity, transform.parent);
        projectile.GetComponent<Projectile>().SetUpProjectile(speed, damage, isPlayer, 1, true, target);
        timerBeforeFire = fireRate;
    }

    public override void PlaceTower()
    {
        isPlacedDown = true;
        if (col == null)
        {
            col = GetComponent<BoxCollider2D>();
        }
        else
        {
            col.enabled = true;
        }

        GameMaster gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        if (isPlayer == true)
        {
            gameMaster.PlayerUnitAdded();
        }
        else
        {
            gameMaster.EnemyUnitAdded();
        }
        switch (Mathf.Abs(transform.position.x))
        {
            case 2.4f:
                rangeStart = 4;
                rangeEnd = 6;
                break;
            case 4f:
                rangeStart = 5;
                rangeEnd = 7;
                break;
            case 5.6f:
                rangeStart = 4;
                rangeEnd = 6;
                break;
            case 7.2f:
                rangeStart = 5;
                rangeEnd = 7;
                break;
        }
    }

    private void CalculateIndicatorPos()
    {
        float offset;
        if (isPlayer)
        {
            offset = 1;
        }
        else
        {
            offset = -1;
        }
            switch (Mathf.Abs(transform.position.x))
            {
                case 2.4f:
                    indicator.transform.position = new Vector3(transform.position.x + (8.8f * offset), transform.position.y, 0);
                    break;
                case 4f:
                    indicator.transform.position = new Vector3(transform.position.x + (10.4f * offset), transform.position.y, 0);
                    break;
                case 5.6f:
                    indicator.transform.position = new Vector3(transform.position.x + (8.8f * offset), transform.position.y, 0);
                    break;
                case 7.2f:
                    indicator.transform.position = new Vector3(transform.position.x + (10.4f * offset), transform.position.y, 0);
                    break;
            }
    }
}
