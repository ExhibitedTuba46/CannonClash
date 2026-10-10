using System.Linq;
using UnityEngine;

public class ScreenTintSetting : Setting
{
    [SerializeField] private Color[] screenTintColours;
    [SerializeField] private string[] screenTintOptions;

    private int currentTint = 0;

    protected override void Start()
    {
        base.Start();

        currentTint = saveManager.GetScreenTintIndex();
        UpdateScreenTint();
    }

    public override void LeftInput()
    {
        if (currentTint > 0)
        {
            currentTint--;
            UpdateScreenTint();
        }
    }

    public override void RightInput()
    {
        if (currentTint < screenTintOptions.Count() - 1)
        {
            currentTint++;
            UpdateScreenTint();
        }
    }

    private void UpdateScreenTint()
    {
        saveManager.SetScreenTintIndex(currentTint);
        saveManager.SetScreenTintColour(screenTintColours[currentTint]);
        text.text = "Screen Tint: " + screenTintOptions[currentTint];
    }
}
