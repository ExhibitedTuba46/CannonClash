using UnityEngine;
using System.Collections.Generic;

public class Menu : InputReciever
{
    [SerializeField] protected List<GameObject> options;
    [SerializeField] protected int currentOption;

    [SerializeField] protected GameObject optionsFlag1;
    [SerializeField] protected GameObject optionsFlag2;

    protected GameObject lastInputReceiver;

    protected void SettingsFlagsUpdate()
    {
        optionsFlag1.transform.localPosition = options[currentOption].GetComponent<Setting>().GetFlag1Position();
        optionsFlag2.transform.localPosition = options[currentOption].GetComponent<Setting>().GetFlag2Position();
    }

    public void ChangeInputReceiver(GameObject inputReceiver)
    {
        lastInputReceiver = inputReceiver;
    }

    public override void DownInput()
    {
        if(currentOption < options.Count - 1)
        {
            currentOption++;
            SettingsFlagsUpdate();
        }
    }

    public override void UpInput()
    {
        if (currentOption > 0)
        {
            currentOption--;
            SettingsFlagsUpdate();
        }
    }
    protected virtual void Start()
    {
        SettingsFlagsUpdate();
    }

    public override void LeftInput()
    {
        options[currentOption].GetComponent<Setting>().LeftInput();
    }

    public override void RightInput()
    {
        options[currentOption].GetComponent<Setting>().RightInput();
    }

    public override void SelectInput()
    {
        options[currentOption].GetComponent<Setting>().SelectInput();
    }
}
