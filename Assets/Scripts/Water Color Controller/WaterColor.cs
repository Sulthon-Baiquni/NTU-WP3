using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColor : MonoBehaviour
{
    public Renderer waterRenderer;
    public Color initialColor = new Color(0, 0.5f, 1);   // Warna awal (biru)
    public Color changedColor = new Color(1, 0.5f, 0);   // Warna setelah berubah (orange)
    public float transitionTime = 10f;                  // Waktu transisi warna (10 detik)

    private Color currentColor;

    void Start()
    {
        // Set warna awal
        currentColor = initialColor;
        waterRenderer.material.SetColor("_Color", currentColor);

        // Mulai transisi warna setelah beberapa waktu
        StartCoroutine(TransitionColor());
    }

    IEnumerator TransitionColor()
    {
        yield return new WaitForSeconds(transitionTime);

        // Ubah warna ke warna yang berbeda
        currentColor = changedColor;
        waterRenderer.material.SetColor("_Color", currentColor);

        Debug.Log("Water color changed!");
    }
}
