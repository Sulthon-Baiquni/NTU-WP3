using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class APIWaterQuality : MonoBehaviour
{
    public Material waterShader;
    public string backendUrl = "http://localhost:3000"; // URL backend Anda

    private void Start()
    {
        StartCoroutine(FetchWaterQualityPeriodically());
    }

    private System.Collections.IEnumerator FetchWaterQualityPeriodically()
    {
        while (true)
        {
            yield return StartCoroutine(FetchWaterQualityData());
            // Tunggu 10 detik sebelum permintaan berikutnya
            yield return new WaitForSeconds(10f);
        }
    }

    private System.Collections.IEnumerator FetchWaterQualityData()
    {
        // Daftar titik monitoring
        string[] monitoringPoints = {
            "point-situ",
            "point-wtp",
            "point-filtrasi",
            "point-groundTank",
            "point-dorm"
        };

        foreach (string pointName in monitoringPoints)
        {
            // Buat URL untuk mengambil parameter terbaru
            string url = $"{backendUrl}/parameters/{pointName}";

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Gagal mengambil data untuk {pointName}: " + www.error);
                    continue;
                }

                try
                {
                    // Parse JSON response
                    string jsonResponse = www.downloadHandler.text;
                    WaterQualityData waterQualityData = JsonUtility.FromJson<WaterQualityData>(jsonResponse);

                    // Ubah warna air berdasarkan parameter
                    UpdateWaterColor(waterQualityData.ph, waterQualityData.turbidity, waterQualityData.tds);

                    // Log data yang diterima
                    Debug.Log($"Data untuk {pointName}: " +
                              $"pH={waterQualityData.ph}, " +
                              $"Turbidity={waterQualityData.turbidity}, " +
                              $"TDS={waterQualityData.tds}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Kesalahan parsing data untuk {pointName}: " + e.Message);
                }
            }
        }
    }

    private void UpdateWaterColor(float ph, float turbidity, float tds)
    {
        // Logic untuk menentukan warna air berdasarkan parameter
        Color waterColor = DetermineWaterColor(ph, turbidity, tds);

        // Terapkan warna ke shader air
        waterShader.SetColor("_Color", waterColor);
    }

    private Color DetermineWaterColor(float ph, float turbidity, float tds)
    {
        // Logic sederhana untuk menentukan warna air
        if (ph < 6.5f || ph > 8.5f)
        {
            return new Color32(139, 69, 19, 255); // Cokelat (air tidak sehat)
        }

        if (turbidity > 30f)
        {
            return new Color32(255, 165, 0, 255); // Oranye (kekeruhan tinggi)
        }

        if (tds > 1000f)
        {
            return new Color32(255, 255, 255, 255); // Putih (TDS tinggi)
        }

        return new Color32(0, 191, 255, 255); // Biru (kondisi normal)
    }

    // Class untuk memetakan struktur data JSON dari backend
    [Serializable]
    private class WaterQualityData
    {
        public float ph;
        public float turbidity;
        public float tds;
    }
}