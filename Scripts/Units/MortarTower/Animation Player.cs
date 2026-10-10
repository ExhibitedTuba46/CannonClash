using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private int frameRate;
    [SerializeField] private List<Sprite> animation;
    [SerializeField] private bool isOneTime;
    [SerializeField] private bool shouldPlayAuto;
    private float idleTime;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetIdleTime();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (shouldPlayAuto)
        {
            PlayAnimation();
        }
    }
    public void PlayAnimation()
    {
        float playTime = Time.time - idleTime;
        int totalFrames = (int)(playTime * frameRate);
        int frame = totalFrames % animation.Count;
        GetComponent<SpriteRenderer>().sprite = animation[frame];
        if (frame == animation.Count - 1 && isOneTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetAnimationFrames(List<Sprite> frames)
    {
        animation = frames;
    }

    public void SetIdleTime()
    {
        idleTime = Time.time;
    }
}
