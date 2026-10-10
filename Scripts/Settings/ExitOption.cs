using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitOption : Setting
{
    [SerializeField] private String levelString;

    public override void SelectInput()
    {
        SceneManager.LoadScene(levelString);
    }
}
