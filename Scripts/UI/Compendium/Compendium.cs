using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Compendium : InputReciever
{
    [SerializeField] private List<GameObject> pages;
    private int pageNumber = 0;
    public override void LeftInput()
    {
        if(pageNumber > 0)
        {
            pages[pageNumber].SetActive(false);
            pageNumber--;
            pages[pageNumber].SetActive(true);
        }
    }

    public override void RightInput()
    {
        if (pageNumber < 4)
        {
            pages[pageNumber].SetActive(false);
            pageNumber++;
            pages[pageNumber].SetActive(true);
        }
    }

    public override void BInput()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main Menu"));
    }

    public override void StartInput()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
