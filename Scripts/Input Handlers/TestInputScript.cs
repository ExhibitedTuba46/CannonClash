using UnityEngine;

public class TestInputScript : InputReciever
{
    public override void UpInput()
    {
        Debug.Log("UpInputRecieved");
    }

    public override void RightInput()
    {
        Debug.Log("RightInputRecieved");
    }

    public override void DownInput()
    {
        Debug.Log("DownInputRecieved");
    }

    public override void LeftInput()
    {
        Debug.Log("LeftInputRecieved");
    }

    public override void AInput()
    {
        Debug.Log("AInputRecieved");
    }

    public override void BInput()
    {
        Debug.Log("BInputRecieved");
    }

    public override void SelectInput()
    {
        Debug.Log("SelectInputRecieved");
    }

    public override void StartInput()
    {
        Debug.Log("StartInputRecieved");
    }
}
