using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectorHandler : InputReciever
{
    [SerializeField] private List<GameObject> unitSelectors = new List<GameObject>();
    [SerializeField] private GameObject[] unitSelectorTextObjects;
    private List<Text> confirmedSelectorTexts = new List<Text>();
    [SerializeField] private GameObject currentSelector;
    [SerializeField] private int currentIndex;

    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Color noGoldColor;

    private GoldBank goldBank;

    private void Awake()
    {
        ChangeSelector();
        goldBank = GetComponent<GoldBank>();
    }

    public void StartPassiveGold()
    {
        goldBank.StartPassiveGain();
    }

    public void StopPassiveGold()
    {
        goldBank.StopPassiveGain();
    }

    private void Start()
    {
        for (int textObject = 0; textObject < unitSelectorTextObjects.Length; textObject++)
        {
            if (textObject > unitSelectors.Count - 1)
            {
                unitSelectorTextObjects[textObject].SetActive(false);
            }
            else
            {   
                Text text = unitSelectorTextObjects[textObject].GetComponent<Text>();
                confirmedSelectorTexts.Add(text);
                text.text = unitSelectors[textObject].GetComponent<UnitSelector>().GetGoldCost().ToString();
            }
        }

        StartCoroutine(UIUpdate());
    }

    public void SetPasiveAd(int newPassive)
    {
        goldBank.SetPassiveAddtional(newPassive);
    }

    private IEnumerator UIUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            for (int text = 0; text < confirmedSelectorTexts.Count; text++)
            {
                if (goldBank.GetGold() >= unitSelectors[text].GetComponent<UnitSelector>().GetGoldCost())
                {
                    confirmedSelectorTexts[text].color = Color.white;
                }
                else
                {
                    confirmedSelectorTexts[text].color = noGoldColor;
                }
            }
        }
    }

    public override void RightInput()
    {
        if (currentIndex != (unitSelectors.Count - 1))
        {
            currentIndex ++;
            ChangeSelector();
        }
    }

    public override void LeftInput()
    {
        if (currentIndex != 0)
        {
            currentIndex --;
            ChangeSelector();
        }
    }

    public override void SelectInput()
    {
        UniversalInputHandler.instance.ChangeInputReciever(grid);
        grid.GetComponent<GridInputHandler>().HighlightTile();
    }

    public override void StartInput()
    {
        UniversalInputHandler.instance.ChangeInputReciever(pauseMenu);
        pauseMenu.GetComponent<Menu>().ChangeInputReceiver(gameObject);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    private void ChangeSelector()
    {
        ResetAllSelectors();
        currentSelector = unitSelectors[currentIndex];
        currentSelector.GetComponent<UnitSelector>().OnSelect();
    }

    private void ResetAllSelectors()
    {
         foreach (GameObject selector in unitSelectors)
        {
            selector.GetComponent<UnitSelector>().OnNotSelected();
        }
    }

    public GameObject GetCurrentSelector()
    {
        return currentSelector;
    }

    public GoldBank GetGoldBank()
    {
        return goldBank;
    }
}   
