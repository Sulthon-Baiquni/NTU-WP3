using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomLake : MonoBehaviour
{
    [System.Serializable]
    public class PipeInfo
    {
        public GameObject pipeObject;      // Referensi objek pipa
        public float pipeSize;             // Ukuran pipa
        public GameObject informationPanel; // Panel informasi untuk pipa
        public Text pipeInfoText;           // Text komponen untuk detail pipa
    }

    [Header("Pipe Configuration")]
    public List<PipeInfo> availablePipes = new List<PipeInfo>();

    [Header("Objects to Randomize")]
    public GameObject particleSystem;
    public GameObject alertObject;

    [Header("Randomization Settings")]
    public float randomOffsetX = 0.5f;  // Offset random di sumbu X
    public float randomOffsetZ = 0.5f;  // Offset random di sumbu Z

    private bool hasRandomized = false;
    private PipeInfo currentSelectedPipe;

    void Start()
    {
        // Sembunyikan semua panel terlebih dahulu
        HideAllPanels();

        // Hanya randomize sekali saat pertama kali scene dijalankan
        if (!hasRandomized)
        {
            RandomizePipePosition();
            hasRandomized = true;
        }
    }

    void HideAllPanels()
    {
        // Sembunyikan semua panel informasi pipa
        foreach (PipeInfo pipeInfo in availablePipes)
        {
            if (pipeInfo.informationPanel != null)
            {
                pipeInfo.informationPanel.SetActive(false);
            }
        }
    }

    void RandomizePipePosition()
    {
        // Pastikan ada pipa yang tersedia
        if (availablePipes.Count == 0)
        {
            Debug.LogError("Tidak ada pipa yang tersedia untuk penempatan!");
            return;
        }

        // Pilih pipa secara acak
        currentSelectedPipe = availablePipes[Random.Range(0, availablePipes.Count)];

        // Pastikan pipa yang dipilih valid
        if (currentSelectedPipe.pipeObject == null)
        {
            Debug.LogError("Pipa yang dipilih tidak memiliki objek!");
            return;
        }

        // Dapatkan Renderer untuk mendapatkan ukuran pipa
        Renderer pipeRenderer = currentSelectedPipe.pipeObject.GetComponent<Renderer>();

        if (pipeRenderer == null)
        {
            Debug.LogError("Tidak ada Renderer pada pipa!");
            return;
        }

        // Dapatkan posisi puncak pipa
        Vector3 pipeBounds = pipeRenderer.bounds.center;
        float pipeTop = pipeRenderer.bounds.max.y;

        // Hasilkan posisi acak di sekitar puncak pipa
        Vector3 randomPosition = new Vector3(
            pipeBounds.x + Random.Range(-randomOffsetX, randomOffsetX),
            pipeTop,
            pipeBounds.z + Random.Range(-randomOffsetZ, randomOffsetZ)
        );

        // Set posisi partikel sistem
        if (particleSystem != null)
        {
            particleSystem.transform.position = randomPosition;
        }

        // Set posisi alert object
        if (alertObject != null)
        {
            alertObject.transform.position = randomPosition;
        }

        // Tampilkan panel informasi untuk pipa yang dipilih
        ShowPipeInformationPanel();
    }

    void ShowPipeInformationPanel()
    {
        // Sembunyikan semua panel terlebih dahulu
        HideAllPanels();

        // Tampilkan panel untuk pipa yang dipilih
        if (currentSelectedPipe.informationPanel != null)
        {
            currentSelectedPipe.informationPanel.SetActive(true);
        }

        // Update informasi text jika tersedia
        if (currentSelectedPipe.pipeInfoText != null)
        {
            currentSelectedPipe.pipeInfoText.text = $"Pipa {currentSelectedPipe.pipeSize} meter\n" +
                                                    $"Lokasi Kebocoran: ({particleSystem.transform.position.x:F2}, " +
                                                    $"{particleSystem.transform.position.y:F2}, " +
                                                    $"{particleSystem.transform.position.z:F2})";
        }
    }
}
