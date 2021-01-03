using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float[] duration, magnitude;

    public void ShakeCam(shake _power)
    {
        switch (_power)
        {
            case shake.very_small:
                StartCoroutine(Shake(duration[0], magnitude[0]));
                break;
            case shake.small:
                StartCoroutine(Shake(duration[1], magnitude[1]));
                break;
            case shake.medium:
                StartCoroutine(Shake(duration[2], magnitude[2]));
                break;
            case shake.big:
                StartCoroutine(Shake(duration[3], magnitude[3]));
                break;
            case shake.very_big:
                StartCoroutine(Shake(duration[4], magnitude[4]));
                break;
            case shake.extrem:
                StartCoroutine(Shake(duration[5], magnitude[5]));
                break;
        }



        
    }

    public IEnumerator Shake(float durantion, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;
        do
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;

        }
        while (elapsed < durantion);
        transform.localPosition = originalPosition;
    }
}
