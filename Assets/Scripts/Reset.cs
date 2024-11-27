using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Camera mainCamera; // Referensi ke Main Camera

    [System.Serializable]
    public class DropdownCameraMapping
    {
        public Dropdown dropdown; // Dropdown untuk grup ini
        public List<CameraOption> cameraOptions; // Opsi kamera untuk dropdown ini
    }

    [System.Serializable]
    public class CameraOption
    {
        public string optionName; // Nama opsi dropdown
        public GameObject cameraObject; // Objek kamera yang terkait
    }

    public List<DropdownCameraMapping> dropdownMappings; // Semua dropdown dan opsi kameranya

    void Start()
{
    // Coba otomatis mencari Main Camera
    if (mainCamera == null)
    {
        mainCamera = Camera.main; // Cari kamera dengan tag "MainCamera"
    }

    // Jika masih null, laporkan error
    if (mainCamera == null)
    {
        Debug.LogError("Main Camera tidak ditemukan! Pastikan ada kamera dengan tag 'MainCamera' di scene.");
        return;
    }
    else
    {
        Debug.Log("Main Camera ditemukan: " + mainCamera.name);
    }

    // Inisialisasi dropdown
    foreach (var mapping in dropdownMappings)
    {
        if (mapping.dropdown == null)
        {
            Debug.LogError("Dropdown belum diatur pada mapping.");
            continue;
        }

        // Tambahkan listener
        mapping.dropdown.onValueChanged.AddListener((index) => OnDropdownValueChanged(mapping, index));

        // Nonaktifkan semua kamera dalam grup
        foreach (var option in mapping.cameraOptions)
        {
            if (option.cameraObject != null)
            {
                option.cameraObject.SetActive(false);
            }
        }

        // Isi opsi dropdown
        PopulateDropdown(mapping);
    }
}


    private void PopulateDropdown(DropdownCameraMapping mapping)
    {
        if (mapping.dropdown == null)
            return;

        mapping.dropdown.ClearOptions(); // Bersihkan opsi lama
        List<string> options = new List<string>();

        foreach (var option in mapping.cameraOptions)
        {
            options.Add(option.optionName); // Tambahkan nama opsi ke dropdown
        }

        mapping.dropdown.AddOptions(options); // Tambahkan opsi ke dropdown UI
    }

    private void OnDropdownValueChanged(DropdownCameraMapping mapping, int index)
    {
        if (index < 0 || index >= mapping.cameraOptions.Count)
        {
            Debug.LogWarning("Indeks dropdown tidak valid untuk grup ini.");
            return;
        }

        // Nonaktifkan semua kamera pada grup ini
        foreach (var option in mapping.cameraOptions)
        {
            if (option.cameraObject != null)
            {
                option.cameraObject.SetActive(false);
            }
        }

        // Aktifkan kamera yang dipilih
        CameraOption selectedOption = mapping.cameraOptions[index];
        if (selectedOption != null && selectedOption.cameraObject != null)
        {
            selectedOption.cameraObject.SetActive(true);

            // Sinkronkan posisi dan rotasi Main Camera
            Camera selectedCamera = selectedOption.cameraObject.GetComponent<Camera>();
            if (selectedCamera != null)
            {
                mainCamera.transform.position = selectedCamera.transform.position;
                mainCamera.transform.rotation = selectedCamera.transform.rotation;
            }
        }
    }
}
