using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 0;
    
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private int damageGold = 10;

    [SerializeField] private bool playerGoldBoost = true;

    [SerializeField] private bool aiGoldBoost = false;

    private bool noHealth = false;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health < 0)
        {
            health = 0;
        }

        if (health == 0 && noHealth == false)
        {
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().EndGame(isPlayer);
            noHealth = true;
        }

        float floatHealth = health;
        float floatMaxHealth = maxHealth;
        
        float percentageHealth = floatHealth / floatMaxHealth;
        healthBar.transform.localScale = new Vector3(percentageHealth, 1, 1);
    }

    public void TakeDamage(float damage)
    {
        health -= (int)damage;
        if (isPlayer == false)
        {
           GameObject.FindWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>().GetGoldBank().IncreaseGold(damageGold); 
           if (aiGoldBoost == true)
            {
                GameObject.FindWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>().GetGoldBank().IncreaseGold(10); 
            }
        }
        else
        {
            GameObject.FindWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>().GetGoldBank().IncreaseGold(damageGold);
            if (playerGoldBoost == true)
            {
                GameObject.FindWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>().GetGoldBank().IncreaseGold(10); 
            }
        }
    }
}
