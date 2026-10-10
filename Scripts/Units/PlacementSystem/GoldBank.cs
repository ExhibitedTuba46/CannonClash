using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoldBank : MonoBehaviour
{
    [SerializeField] private int gold = 0;
    [SerializeField] private int passiveGold = 1;
    [SerializeField] private int passiveGoldAdditional = 0;
    [SerializeField] private float passiveGoldTiming = 1;
    [SerializeField] private bool canGeneratePassiveGold = false;
    [SerializeField] private GameObject goldText;

    private void Start()
    {
        gold = Mathf.Clamp(gold, 0, 999);
        goldText.GetComponent<Text>().text = gold.ToString();
        StartCoroutine(UIUpdate());
    }

    private IEnumerator PassiveGoldGain()
    {
        while (canGeneratePassiveGold == true)
        {
            yield return new WaitForSeconds(passiveGoldTiming);
            gold += passiveGold + passiveGoldAdditional;
        }
        
    }

    public void SetPassiveAddtional(int newPassive)
    {
        passiveGoldAdditional = newPassive;
    }

    private IEnumerator UIUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            goldText.GetComponent<Text>().text = gold.ToString();
        }
    }

    public int GetGold()
    {
        return gold;
    }

    public void StartPassiveGain()
    {
        canGeneratePassiveGold = true;
        StartCoroutine(PassiveGoldGain());
    }

    public void StopPassiveGain()
    {
        canGeneratePassiveGold = false;
    }

    public void IncreaseGold(int goldGained)
    {
        gold += goldGained;
    }

    public void DecreasGold(int goldSpent)
    {
        gold -= goldSpent;
    }
}
