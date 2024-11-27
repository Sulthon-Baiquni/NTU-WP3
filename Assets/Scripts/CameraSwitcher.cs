using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class DropdownCameraMapping
    {
        public Dropdown dropdown; // Dropdown UI
        public List<GameObject> targetObjects; // List of objects with camera components as reference
    }

    public List<DropdownCameraMapping> dropdownMappings; // List of dropdown to object mappings
    private Camera mainCamera;

    private void Start()
    {
        // Cari Main Camera di scene
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera tidak ditemukan!");
            return;
        }

        // Tambahkan listener untuk setiap dropdown
        foreach (var mapping in dropdownMappings)
        {
            if (mapping.dropdown == null)
            {
                Debug.LogError("Dropdown belum diatur dalam mapping.");
                continue;
            }

            mapping.dropdown.onValueChanged.AddListener((index) => OnDropdownValueChanged(mapping, index));
        }
    }

    private void OnDropdownValueChanged(DropdownCameraMapping mapping, int index)
    {
        if (index < 0 || index >= mapping.targetObjects.Count)
        {
            Debug.LogWarning("Index tidak valid pada dropdown: " + mapping.dropdown.name);
            return;
        }

        // Dapatkan objek target berdasarkan pilihan dropdown
        GameObject targetObject = mapping.targetObjects[index];
        if (targetObject == null)
        {
            Debug.LogWarning("Target object tidak ditemukan untuk pilihan index: " + index);
            return;
        }

        // Ambil kamera di dalam objek target
        Camera targetCamera = targetObject.GetComponent<Camera>();
        if (targetCamera == null)
        {
            Debug.LogWarning("Camera component tidak ditemukan di dalam target object: " + targetObject.name);
            return;
        }

        // Pindahkan Main Camera ke posisi dan rotasi kamera target
        mainCamera.transform.position = targetCamera.transform.position;
        mainCamera.transform.rotation = targetCamera.transform.rotation;

        Debug.Log("Main Camera dipindahkan ke: " + targetObject.name);
    }
}
