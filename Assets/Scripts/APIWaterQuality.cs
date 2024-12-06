using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class APIWaterQuality : MonoBehaviour
{
    public Material waterShader;
    public string baseUrl = "https://waterwise-server.urbansolv.co.id";

    private void Start()
    {
        StartCoroutine(FetchWaterQualityPeriodically());
    }

    private System.Collections.IEnumerator FetchWaterQualityPeriodically()
    {
        while (true)
        {
            yield return StartCoroutine(FetchWaterQualityData());
            yield return new WaitForSeconds(10f);
        }
    }

    private System.Collections.IEnumerator FetchWaterQualityData()
    {
        // Daftar titik monitoring - pastikan nama persis sesuai dengan backend
        string[] monitoringPoints = {
            "point-situ",
            "point-wtp",
            "point-filtrasi",
            "point-groundTank",
            "point-dorm"
        };

        foreach (string pointName in monitoringPoints)
        {
            // Konstruksi URL dengan lebih detail
            string fullUrl = $"{baseUrl}/parameters/{pointName}";

            Debug.Log($"Mencoba mengakses URL: {fullUrl}");

            using (UnityWebRequest www = UnityWebRequest.Get(fullUrl))
            {
                // Tambahkan header untuk memastikan request valid
                www.SetRequestHeader("Accept", "application/json");

                yield return www.SendWebRequest();

                // Cetak informasi detail respons
                Debug.Log($"Respons untuk {pointName}:");
                Debug.Log($"Kode Respons: {www.responseCode}");
                Debug.Log($"Error: {www.error}");
                Debug.Log($"Respon Text: {www.downloadHandler.text}");

                // Penanganan berbagai skenario respons
                switch (www.responseCode)
                {
                    case 200: // Sukses
                        try
                        {
                            // Parse JSON dengan metode yang lebih robust
                            string jsonResponse = www.downloadHandler.text;
                            ProcessWaterQualityData(pointName, jsonResponse);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Kesalahan parsing data untuk {pointName}: {e.Message}");
                        }
                        break;

                    case 404: // Not Found
                        Debug.LogWarning($"Endpoint tidak ditemukan untuk {pointName}. Periksa URL atau konfigurasi backend.");
                        break;

                    case 500: // Server Error
                        Debug.LogError($"Kesalahan server untuk {pointName}. Hubungi administrator.");
                        break;

                    default:
                        Debug.LogWarning($"Respons tidak terduga: {www.responseCode}");
                        break;
                }
            }
        }
    }

    private void ProcessWaterQualityData(string pointName, string jsonResponse)
    {
        // Debugging struktur JSON
        Debug.Log($"JSON untuk {pointName}: {jsonResponse}");

        // Coba parsing JSON dengan metode yang lebih fleksibel
        try
        {
            // Jika struktur JSON berbeda, gunakan JsonUtility.FromJsonOverwrite atau manual
            WaterQualityData waterQualityData = JsonUtility.FromJson<WaterQualityData>(jsonResponse);

            if (waterQualityData != null)
            {
                UpdateWaterColor(waterQualityData.ph, waterQualityData.turbidity, waterQualityData.tds);

                Debug.Log($"Data berhasil diproses untuk {pointName}: " +
                          $"pH={waterQualityData.ph}, " +
                          $"Turbidity={waterQualityData.turbidity}, " +
                          $"TDS={waterQualityData.tds}");
            }
            else
            {
                Debug.LogError($"Gagal mem-parsing data untuk {pointName}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Kesalahan parsing JSON untuk {pointName}: {e.Message}");
        }
    }

    private void UpdateWaterColor(float ph, float turbidity, float tds)
    {
        Color waterColor = DetermineWaterColor(ph, turbidity, tds);
        waterShader.SetColor("_Color", waterColor);
    }

    private Color DetermineWaterColor(float ph, float turbidity, float tds)
    {
        if (ph < 6.5f || ph > 8.5f)
        {
            return new Color32(139, 69, 19, 255); // Cokelat (air tidak sehat)
        }

        if (turbidity > 30f || tds > 1000f)
        {
            return new Color32(255, 165, 0, 255); // Oranye (kekeruhan tinggi atau TDS tinggi)
        }

        return new Color32(0, 191, 255, 255); // Biru (kondisi normal)
    }

    [Serializable]
    private class WaterQualityData
    {
        public float ph;
        public float turbidity;
        public float tds;
    }
}