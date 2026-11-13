using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScreenShake : MonoBehaviour
{

    private int _camSize;

    public IEnumerator Shake (float duration, float magnitude)
    {
        int originalSize = _camSize;

        float elapsed = 0.0f;

        while (elapsed <= duration && Time.timeScale > 0)
        {
            float z = .01f * magnitude;

            if (elapsed < duration / 5)
            {
                gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize - (z * 5);
            }
            if (elapsed >= duration / 5)
            {
                gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize + (z * 1.25f);
            }
            elapsed += Time.deltaTime;

            yield return null;

        }

        gameObject.GetComponent<Camera>().orthographicSize = originalSize;
    }

    public IEnumerator BigShake(float duration, float magnitude)
    {
        int originalSize = _camSize;

        float elapsed = 0.0f;

        while (elapsed < duration && Time.timeScale > 0)
        {
            float z = .05f * magnitude;

            if (elapsed < duration / 7)
            {
                gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize + (z * 7);
            }
            if (elapsed >= duration / 7)
            {
                gameObject.GetComponent<Camera>().orthographicSize = gameObject.GetComponent<Camera>().orthographicSize - (z * 1.67f);
            }
            elapsed += Time.deltaTime;

            yield return null;

        }

        gameObject.GetComponent<Camera>().orthographicSize = originalSize;
    }

    public void CameraSize(int size)
    {
        _camSize = size;
    }

}
