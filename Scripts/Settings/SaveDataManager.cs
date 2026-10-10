using System;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [SerializeField] private int highestLvl = 0;
    [SerializeField] private Color screenTintColour;
    [SerializeField] private int screenTintIndex = 0;

    [SerializeField] private float volumeValue;

    public static SaveDataManager instance;

    public class SaveData
    {
        public int highestLvlData;
        
        public Color screenTintData;
        public int screenTintIndexData;

        public float volumeValueData;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            ReadData();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void WriteData()
    {
        SaveData data = new SaveData();
        data.highestLvlData = highestLvl;
        data.screenTintData = screenTintColour;
        data.screenTintIndexData = screenTintIndex;
        data.volumeValueData = volumeValue;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }

    public void ReadData()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highestLvl = data.highestLvlData;
            screenTintColour = data.screenTintData;
            screenTintIndex = data.screenTintIndexData;
            volumeValue = data.volumeValueData;
        }
    }

    public void SetHighestLvl(int newHighestLvl)
    {
        highestLvl = newHighestLvl;
        WriteData();
    }
    
    public int GetHighestLvl()
    {
        ReadData();
        return highestLvl;
    }

    public void SetScreenTintColour(Color newScreenTint)
    {
        screenTintColour = newScreenTint;
        WriteData();
    }

    public Color GetScreenTintColour()
    {
        ReadData();
        return screenTintColour;
    }

    public void SetScreenTintIndex(int newScreenTintIndex)
    {
        screenTintIndex = newScreenTintIndex;
        WriteData();
    }
    
    public int GetScreenTintIndex()
    {
        ReadData();
        return screenTintIndex;
    }

    public void SetVolumeValue(float newVolumeValue)
    {
        volumeValue = newVolumeValue;
        WriteData();
    }
    
    public float GetVolumeValue()
    {
        ReadData();
        return volumeValue;
    }
}
