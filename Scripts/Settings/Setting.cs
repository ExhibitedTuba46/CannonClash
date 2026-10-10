using UnityEngine;
using TMPro;

public class Setting : MonoBehaviour
{
    [SerializeField] protected Vector3 flag1Position;
    [SerializeField] protected Vector3 flag2Position;

    [SerializeField] protected TextMeshProUGUI text;

    protected SaveDataManager saveManager;

    protected virtual void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveDataManager>();
    }

    public Vector3 GetFlag1Position()
    {
        return flag1Position;
    }

    public Vector3 GetFlag2Position()
    {
        return flag2Position;
    }
    
    public virtual void LeftInput()
    {
        
    }

    public virtual void RightInput()
    {
        
    }

    public virtual void SelectInput()
    {
        
    }
}
