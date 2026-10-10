using UnityEngine;

public class BackOption : Setting
{
   [SerializeField] private GameObject menu;

    public override void SelectInput()
    {
        menu.GetComponent<Menu>().StartInput();
    }
}
