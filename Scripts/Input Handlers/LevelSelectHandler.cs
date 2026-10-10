using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectHandeler : InputReciever
{
    [SerializeField] private GameObject[] levelList;
    [SerializeField] private GameObject[] levelNumberList;

    [SerializeField] private int currentLevelIndex;
    [SerializeField] private GameObject currentLevel;

    private SaveDataManager saveManager;
    
    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveDataManager>();
        saveManager.ReadData();
        
        int highestLvl = saveManager.GetHighestLvl();

        for (int lvl = 0; lvl < levelList.Length; lvl++)
        {
            if (highestLvl < lvl)
            {
                levelList[lvl].GetComponent<LevelSelectButton>().SetUnlockStatus(false);
            }
            else
            {
                levelList[lvl].GetComponent<LevelSelectButton>().SetUnlockStatus(true);
            }
        }
        
        for (int lvl = 0; lvl < (levelList.Length); lvl++)
        {
            if (levelList[lvl].GetComponent<LevelSelectButton>().GetUnlockStatus() == false)
            {
                levelNumberList[lvl].SetActive(false);
            }
        }

        ChangeSelectedLevel();
    }

    public override void RightInput()
    {
        if (currentLevelIndex != levelList.Length - 1)
        {
            if (levelList[currentLevelIndex + 1].GetComponent<LevelSelectButton>().GetUnlockStatus() == true)
            {
                currentLevelIndex++;
                ChangeSelectedLevel();
            }
        }

    }

    public override void LeftInput()
    {
        if (currentLevelIndex != 0)
        {
            currentLevelIndex--;
            ChangeSelectedLevel();
        }
    }

    public override void SelectInput()
    {
        currentLevel.GetComponent<LevelSelectButton>().LoadNextLevel();
    }

    public override void StartInput()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void ChangeSelectedLevel()
    {
        foreach (GameObject level in levelList)
        {
            level.GetComponent<LevelSelectButton>().OnNotSelected();
        }
        currentLevel = levelList[currentLevelIndex];
        currentLevel.GetComponent<LevelSelectButton>().OnSelected();
    }
}
