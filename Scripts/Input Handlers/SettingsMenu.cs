public class SettingsMenu : Menu
{
    public override void StartInput()
    {
        UniversalInputHandler.instance.ChangeInputReciever(lastInputReceiver);
        Destroy(this.gameObject);
    }
}
