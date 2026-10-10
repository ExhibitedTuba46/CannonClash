using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetting : Setting
{
    [SerializeField] private AudioMixer audioMixer;

    private float volumeValue;

    protected override void Start()
    {
        base.Start();

        audioMixer.GetFloat("Volume", out volumeValue);
        volumeValue = saveManager.GetVolumeValue();
        audioMixer.SetFloat("Volume", volumeValue);
        if (text != null)
        {
            text.text = "Volume: " + (volumeValue + 100);
        }
    }
    public override void LeftInput()
    {
        if(volumeValue > -100)
        {
            volumeValue -= 5;
            audioMixer.SetFloat("Volume", volumeValue);
            saveManager.SetVolumeValue(volumeValue);
            if (text != null)
            {
                text.text = "Volume: " + (volumeValue + 100);
            }
        }
    }

    public override void RightInput()
    {
        if(volumeValue < 0)
        {
            volumeValue += 5;
            audioMixer.SetFloat("Volume", volumeValue);
            saveManager.SetVolumeValue(volumeValue);
            if (text != null)
            {
                text.text = "Volume: " + (volumeValue + 100);
            }
        }
    }
}
