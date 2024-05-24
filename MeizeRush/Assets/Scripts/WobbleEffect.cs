using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    public float wobbleFrequency = 2f; // Frequência do balanço
    public float wobbleAmplitude = 0.1f; // Amplitude do balanço
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Salva a escala original do objeto
    }

    void Update()
    {
        // Calcula a nova escala baseado em um senoide
        float wobble = Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
        transform.localScale = new Vector3(originalScale.x, originalScale.y + wobble, originalScale.z);
    }
}
