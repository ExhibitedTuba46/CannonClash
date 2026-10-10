using Unity.Mathematics;
using UnityEngine;

public class SettingsOption : Setting
{
    [SerializeField] private GameObject settingsMenuPrefab;
    [SerializeField] private GameObject pauseMenu;
 
    public override void SelectInput()
    {
        GameObject settingsMenu = Instantiate(settingsMenuPrefab, Vector2.zero, quaternion.identity);
        settingsMenu.GetComponent<SettingsMenu>().ChangeInputReceiver(pauseMenu);
        UniversalInputHandler.instance.ChangeInputReciever(settingsMenu);
    }
}
