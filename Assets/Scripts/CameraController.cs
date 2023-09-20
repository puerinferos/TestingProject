using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Cannon cannon;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;

    private void Start()
    {
        cannon.OnShoot += ShakeCamera;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = originalPos.x+Random.Range(-1f, 1f) * magnitude*Time.deltaTime;
            float y = originalPos.y+Random.Range(-1f, 1f) * magnitude*Time.deltaTime;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake(shakeDuration, shakeStrength));
    }
}
