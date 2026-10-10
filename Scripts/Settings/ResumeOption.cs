using UnityEngine;

public class ResumeOption : Setting
{
    [SerializeField] private GameObject pauseMenuManager;

    public override void SelectInput()
    {
        pauseMenuManager.GetComponent<PauseMenu>().StartInput();
    }
}
