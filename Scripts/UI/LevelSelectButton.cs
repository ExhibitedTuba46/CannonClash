using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private bool isUnlocked = true;

    private void Start()
    {
        if (isUnlocked == false)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
    }

    public void OnSelected()
    {
        if (isUnlocked == true)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }

    public void OnNotSelected()
    {
        if (isUnlocked == true)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void SetUnlockStatus(bool newStatus)
    {
        isUnlocked = newStatus;
    }

    public bool GetUnlockStatus()
    {
        return isUnlocked;
    }
}
