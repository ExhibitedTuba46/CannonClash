using System.Collections;
using UnityEngine;

public class StartSequence : MonoBehaviour
{
    [SerializeField] private float nextFrameInterval;
    [SerializeField] private Sprite[] animationFrames;

    private int currentFrame = 0;

    private void Start()
    {
        StartCoroutine(AnimationSequence());
    }

    public float GetFrameInterval()
    {
        return nextFrameInterval;
    }

    public int GetFrameCount()
    {
        return animationFrames.Length;
    }

    private IEnumerator AnimationSequence()
    {
        yield return new WaitForSeconds(nextFrameInterval);

        if (currentFrame < animationFrames.Length - 1)
        {
            currentFrame++;
            GetComponent<SpriteRenderer>().sprite = animationFrames[currentFrame];

            StartCoroutine(AnimationSequence());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
