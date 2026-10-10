using System;
using UnityEngine;
using System.Collections.Generic;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private List<Sprite> unSelectedSprite;
    [SerializeField] private List<Sprite> selectedSprite;

    [SerializeField] private GameObject unitPrefab;

    [SerializeField] private GameObject cooldownBar;
    [SerializeField] private float cooldownDuration;
    private float cooldown;

    private int goldCost;

    private AnimationPlayer ap;

    private void Awake()
    {
        ap = GetComponent<AnimationPlayer>();
    }

    private void Start()
    {
        goldCost = unitPrefab.GetComponent<PlacableObject>().GetCost();
        ap = GetComponent<AnimationPlayer>();
    }

    public void ResetCooldown()
    {
        cooldown = cooldownDuration;
    }

    public void OnSelect()
    {
        ap.SetIdleTime();
        ap.SetAnimationFrames(selectedSprite);
    }

    public void OnNotSelected()
    { 
        ap.SetAnimationFrames(unSelectedSprite);
    }
    
    public GameObject GetUnitPrefab()
    {
        return unitPrefab;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0;
        }

        float percentageCooldown = cooldown / cooldownDuration;
        cooldownBar.transform.localScale = new Vector3(percentageCooldown, 1, 1);
        
        ap.PlayAnimation();
    }

    public int GetGoldCost()
    {
        return goldCost;
    }
}
