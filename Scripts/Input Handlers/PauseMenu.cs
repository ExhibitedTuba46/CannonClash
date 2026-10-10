using UnityEngine;

public class PauseMenu : Menu
{
    public override void StartInput()
    {
        Debug.Log(lastInputReceiver);
        UniversalInputHandler.instance.ChangeInputReciever(lastInputReceiver);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
