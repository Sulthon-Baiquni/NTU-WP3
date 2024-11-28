using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIWaterQuality : MonoBehaviour
{
    public Material waterShader;

    private void Start()
    {
        StartCoroutine(ChangeWaterColorPeriodically());
    }

    private System.Collections.IEnumerator ChangeWaterColorPeriodically()
    {
        while (true)
        {
            // Generate nilai pH, turbiditas, dan TDS secara random
            float ph = Random.Range(4.5f, 10.0f);
            float turbidity = Random.Range(0f, 40f);
            float tds = Random.Range(100f, 2000f);

            // Panggil API untuk mendapatkan warna air
            yield return StartCoroutine(GetWaterColor(ph, turbidity, tds));

            // Tunggu 10 detik sebelum mengubah warna lagi
            yield return new WaitForSeconds(10f);
        }
    }

    private System.Collections.IEnumerator GetWaterColor(float ph, float turbidity, float tds)
    {
        string url = $"http://localhost:5000/water-color?ph={ph}&turbidity={turbidity}&tds={tds}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Gagal mengambil data warna air: " + www.error);
            }
            else
            {
                WaterColorResponse response = JsonUtility.FromJson<WaterColorResponse>(www.downloadHandler.text);
                UpdateShaderColor(response.color);
            }
        }
    }

    private void UpdateShaderColor(string color)
    {
        // Peta warna ke nilai RGB
        Dictionary<string, Color32> colorMap = new Dictionary<string, Color32>
        {
            { "cokelat", new Color32(139, 69, 19, 255) },
            { "oranye", new Color32(255, 165, 0, 255) },
            { "biru", new Color32(0, 191, 255, 255) },
            { "putih", new Color32(255, 255, 255, 255) }
        };

        if (colorMap.TryGetValue(color, out Color32 shaderColor))
        {
            waterShader.SetColor("_Color", shaderColor);
        }
        else
        {
            // Jika warna tidak ditemukan, atur warna air ke biru
            waterShader.SetColor("_Color", new Color32(0, 191, 255, 255));
        }
    }

    [System.Serializable]
    private class WaterColorResponse
    {
        public string color;
    }
}
