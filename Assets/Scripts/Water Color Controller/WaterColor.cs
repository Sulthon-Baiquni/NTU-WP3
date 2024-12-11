using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColor : MonoBehaviour
{
    [Header("Rendering Settings")]
    public Renderer waterRenderer; // Renderer untuk shader
    public string colorPropertyName = "_Color"; // Nama property warna di shader

    [Header("Color Transition Settings")]
    public Color initialColor = Color.blue; // Warna awal
    public Color targetColor = Color.white; // Warna target
    public float transitionDuration = 20f; // Durasi transisi warna
    public bool autoTransition = true; // Apakah transisi otomatis
    public float delayBeforeTransition = 0f; // Penundaan sebelum transisi

    [Header("Transition Type")]
    public TransitionType transitionType = TransitionType.Lerp;

    public enum TransitionType
    {
        Lerp,
        Instant,
        Fade
    }

    private void Start()
    {
        // Set warna awal
        if (waterRenderer != null)
        {
            waterRenderer.material.SetColor(colorPropertyName, initialColor);
        }

        // Mulai transisi jika auto transition aktif
        if (autoTransition)
        {
            StartCoroutine(TransitionWaterColor());
        }
    }

    public void StartManualTransition()
    {
        StartCoroutine(TransitionWaterColor());
    }

    private IEnumerator TransitionWaterColor()
    {
        // Tunggu delay awal
        yield return new WaitForSeconds(delayBeforeTransition);

        // Pilih metode transisi
        switch (transitionType)
        {
            case TransitionType.Lerp:
                yield return StartCoroutine(LerpColorTransition());
                break;
            case TransitionType.Instant:
                InstantColorTransition();
                break;
            case TransitionType.Fade:
                yield return StartCoroutine(FadeColorTransition());
                break;
        }
    }

    private IEnumerator LerpColorTransition()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // Interpolasi warna
            Color newColor = Color.Lerp(initialColor, targetColor, elapsedTime / transitionDuration);

            // Update warna shader
            waterRenderer.material.SetColor(colorPropertyName, newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Pastikan warna akhir tepat
        waterRenderer.material.SetColor(colorPropertyName, targetColor);
    }

    private void InstantColorTransition()
    {
        // Ganti warna langsung
        waterRenderer.material.SetColor(colorPropertyName, targetColor);
    }

    private IEnumerator FadeColorTransition()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // Fade dengan kurva sine untuk efek halus
            float t = elapsedTime / transitionDuration;
            float fadeValue = Mathf.Sin(t * Mathf.PI * 0.5f);

            Color newColor = Color.Lerp(initialColor, targetColor, fadeValue);

            waterRenderer.material.SetColor(colorPropertyName, newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Pastikan warna akhir tepat
        waterRenderer.material.SetColor(colorPropertyName, targetColor);
    }

    // Metode untuk mengubah warna secara manual dari script lain
    public void SetInitialColor(Color newColor)
    {
        initialColor = newColor;
        if (waterRenderer != null)
        {
            waterRenderer.material.SetColor(colorPropertyName, initialColor);
        }
    }

    public void SetTargetColor(Color newColor)
    {
        targetColor = newColor;
    }
}
