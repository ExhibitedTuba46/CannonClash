using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTint : MonoBehaviour
{
    [SerializeField] private GameObject screenTint;

    private SaveDataManager saveManager;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveDataManager>();
        screenTint.SetActive(true);
    }

    private void Update()
    {
        Color tintColor = saveManager.GetScreenTintColour();
        screenTint.GetComponent<Image>().color = tintColor;
    }
}
