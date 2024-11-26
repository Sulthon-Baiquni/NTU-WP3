using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Camera mainCamera;
    public Dropdown cameraDropdown;
    public GameObject[] cameraObjects; // Array untuk menyimpan game object yang memiliki kamera

    void Start()
    {
        // Menambahkan listener untuk dropdown
        cameraDropdown.onValueChanged.AddListener(delegate { SwitchCamera(cameraDropdown.value); });

        // Pastikan hanya main camera yang aktif di awal
        ActivateCamera(mainCamera);
    }

    void SwitchCamera(int index)
    {
        // Nonaktifkan semua kamera
        foreach (GameObject camObj in cameraObjects)
        {
            camObj.SetActive(false);
        }

        // Aktifkan kamera yang dipilih
        if (index >= 0 && index < cameraObjects.Length)
        {
            cameraObjects[index].SetActive(true);
        }
        else
        {
            // Jika index tidak valid, aktifkan main camera
            ActivateCamera(mainCamera);
        }
    }

    void ActivateCamera(Camera cam)
    {
        // Nonaktifkan semua kamera
        foreach (GameObject camObj in cameraObjects)
        {
            camObj.SetActive(false);
        }

        // Aktifkan kamera yang diinginkan
        cam.gameObject.SetActive(true);
    }
}
