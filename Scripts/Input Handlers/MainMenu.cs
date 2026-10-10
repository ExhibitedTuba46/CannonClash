using UnityEngine;

public class MainMenu : Menu
{
    public override void StartInput()
    {
        Application.Quit();
    }
}
