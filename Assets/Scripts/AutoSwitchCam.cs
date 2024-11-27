using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSwitchCam : MonoBehaviour
{
    [System.Serializable]
    public class DropdownCameraMapping
    {
        public UnityEngine.UI.Dropdown dropdown; // Legacy UI Dropdown
        public List<GameObject> targetObjects; // List objek dengan kamera
        public List<GameObject> uiImages;
    }

    public List<DropdownCameraMapping> dropdownMappings; // Mapping Dropdown ke objek
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

            // Tambahkan listener ke Legacy Dropdown
            mapping.dropdown.onValueChanged.AddListener((index) =>
            {
                OnDropdownValueChanged(mapping, index);
            });

            // Tambahkan onClick event untuk menampilkan UI saat dropdown diklik
            Button dropdownButton = mapping.dropdown.GetComponent<Button>();
            if (dropdownButton != null)
            {
                dropdownButton.onClick.AddListener(() =>
                {
                    OnDropdownClicked(mapping);
                });
            }
        }
    }

    private void OnDropdownClicked(DropdownCameraMapping mapping)
    {
        // Aktifkan semua UI images saat dropdown diklik
        foreach (var ui in mapping.uiImages)
        {
            if (ui != null)
                ui.SetActive(true); // Tampilkan UI image
        }
    }

    private void OnDropdownValueChanged(DropdownCameraMapping mapping, int index)
    {
        if (index < 0 || index >= mapping.targetObjects.Count || index >= mapping.uiImages.Count)
        {
            Debug.LogWarning("Index tidak valid pada dropdown: " + mapping.dropdown.name);
            return;
        }

        GameObject targetObject = mapping.targetObjects[index];
        GameObject uiImage = mapping.uiImages[index];

        // Sembunyikan semua UI images sebelum menampilkan yang sesuai
        foreach (var ui in mapping.uiImages)
        {
            if (ui != null)
                ui.SetActive(false);
        }

        // Aktifkan UI yang sesuai dengan pilihan dropdown
        if (uiImage != null)
        {
            uiImage.SetActive(true);
        }

        // Pindahkan kamera utama ke target
        Camera targetCamera = targetObject.GetComponent<Camera>();
        if (targetCamera == null)
        {
            Debug.LogWarning("Camera component tidak ditemukan di dalam target object: " + targetObject.name);
            return;
        }

        StartCoroutine(SmoothTransition(targetCamera.transform.position, targetCamera.transform.rotation, 1f));
    }

    private IEnumerator SmoothTransition(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }
}
