using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera; // Referensi ke Main Camera

    // Struktur untuk menyimpan pasangan opsi dropdown dan objek kamera
    [System.Serializable]
    public class CameraGroup
    {
        public Dropdown dropdown; // Dropdown UI
        public List<CameraOption> cameraOptions; // Daftar opsi kamera terkait
    }

    [System.Serializable]
    public class CameraOption
    {
        public string optionName; // Nama opsi pada dropdown
        public GameObject cameraObject; // Objek yang memiliki kamera
    }

    public List<CameraGroup> cameraGroups; // Daftar semua grup dropdown dan opsi kameranya

    private void Start()
    {
        // Validasi Main Camera
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera tidak ditemukan!");
            return;
        }

        // Validasi setiap grup dropdown dan opsi kameranya
        foreach (var group in cameraGroups)
        {
            if (group.dropdown == null)
            {
                Debug.LogError("Dropdown tidak ditemukan di salah satu grup!");
                continue;
            }

            // Tambahkan listener ke setiap dropdown
            group.dropdown.onValueChanged.AddListener((index) => SwitchCamera(group, index));

            // Nonaktifkan semua kamera di setiap grup di awal
            foreach (var option in group.cameraOptions)
            {
                if (option.cameraObject != null)
                    option.cameraObject.SetActive(false);
            }

            // Inisialisasi dropdown dengan opsi
            PopulateDropdown(group);
        }
    }

    private void PopulateDropdown(CameraGroup group)
    {
        group.dropdown.ClearOptions(); // Hapus opsi lama
        List<string> options = new List<string>();

        foreach (var option in group.cameraOptions)
        {
            options.Add(option.optionName); // Tambahkan nama opsi ke dropdown
        }

        group.dropdown.AddOptions(options); // Masukkan opsi ke dropdown
    }

    // Fungsi untuk mengganti kamera berdasarkan dropdown yang dipilih
    public void SwitchCamera(CameraGroup group, int index)
    {
        if (index < 0 || index >= group.cameraOptions.Count)
        {
            Debug.LogWarning("Indeks dropdown tidak valid!");
            return;
        }

        // Nonaktifkan semua kamera sebelum mengaktifkan yang baru
        foreach (var option in group.cameraOptions)
        {
            if (option.cameraObject != null)
                option.cameraObject.SetActive(false);
        }

        // Ambil opsi kamera yang dipilih berdasarkan indeks
        CameraOption selectedOption = group.cameraOptions[index];
        if (selectedOption != null && selectedOption.cameraObject != null)
        {
            // Aktifkan kamera objek untuk mendapatkan transformasinya
            selectedOption.cameraObject.SetActive(true);

            // Sinkronkan posisi dan rotasi Main Camera dengan kamera dari objek tersebut
            Camera selectedCamera = selectedOption.cameraObject.GetComponent<Camera>();
            if (selectedCamera != null)
            {
                mainCamera.transform.position = selectedCamera.transform.position;
                mainCamera.transform.rotation = selectedCamera.transform.rotation;
            }
            else
            {
                Debug.LogWarning("Kamera tidak ditemukan di objek: " + selectedOption.cameraObject.name);
            }
        }
    }
}
