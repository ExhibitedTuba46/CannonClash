using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class PlacableObject : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    //Used for walk speed or projectile speed depending on if object moves
    [SerializeField] protected float speed;
    [SerializeField] protected float fireRate;
    [SerializeField] protected int cost;
    [SerializeField] protected bool isPlayer;
    [SerializeField] protected bool isPlacedDown = false;

    [SerializeField] protected GameObject rangeIndicatorPrefab;
    
    protected GameObject indicator;

    [SerializeField] protected int damageGold = 1;
    protected float timerBeforeFire;
    protected BoxCollider2D col;
    protected SpriteRenderer sr;


    //Animation stuff
    [Header("Animation Stuff")]
    [SerializeField] protected float framerate;
    protected float idleTime;
    [SerializeField] protected List<Sprite> attackAnimation;

    protected void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (!isPlacedDown)
        {
            col.enabled = false;
        }
        if (isPlayer)
        {
            gameObject.layer = 3;
        }
        else
        {
            gameObject.layer = 6;
        }

            cost = Mathf.Clamp(cost, 0, 999);
    }

    protected virtual void Start()
    {
        if (isPlacedDown == false && rangeIndicatorPrefab != null)
        {
            GameObject newIndicator = Instantiate(rangeIndicatorPrefab, transform.position, quaternion.identity);
            indicator = newIndicator;
        }
    }

    protected virtual void Update()
    {
        if (indicator != null && isPlayer)
        {
            if (isPlacedDown == false)
            {
                UpdateIndicator();
            }
            else
            {
                Destroy(indicator);
            }
        }
    }

    protected virtual void UpdateIndicator()
    {
        indicator.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected virtual void Attack()
    {

    }

    public int GetCost()
    {
        return cost;
    }

    protected void OnDestroy()
    {
        if (indicator != null)
        {
            Destroy(indicator);
        }

        if (gameObject.scene.isLoaded == true && isPlacedDown == true)
        {
            GameMaster gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

            if (isPlayer == true)
            {
                gameMaster.RemovePlayerUnit();
            }   
            else
            {
                gameMaster.RemoveEnemyUnit();
            }
        }
        
    }

    public virtual void PlaceTower()
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
    }

    public bool GetPlayerControlStatus()
    {
        return isPlayer;
    }

    public void SetPlayerControlStatus(bool hasplayerControl)
    {
        isPlayer = hasplayerControl;
        if (isPlayer == false)
        {
            gameObject.layer = 6;
        }
    }

    //Returns if animation is finished
    protected bool PlayAnimation(List<Sprite> animation)
    {
        float playTime = Time.time - idleTime;
        int totalFrames = (int)(playTime * framerate);
        int frame = totalFrames % animation.Count;
        sr.sprite = animation[frame];
        if(frame == animation.Count - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (isPlayer == false)
        {
            GameObject.FindWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>().GetGoldBank().IncreaseGold(damageGold);
        }
        else
        {
            GameObject.FindWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>().GetGoldBank().IncreaseGold(damageGold);
        }
    }
}
