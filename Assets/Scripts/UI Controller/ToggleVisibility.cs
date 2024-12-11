using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibility : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // GameObject yang akan ditampilkan/disembunyikan
    private bool isTargetActive = false; // Status aktif/tidaknya targetObject

    private void Update()
    {
        // Periksa jika mouse diklik
        if (Input.GetMouseButtonDown(0)) // Tombol kiri mouse
        {
            // Membuat Raycast ke posisi mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Cek apakah Raycast mengenai GameObject ini
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // Jika terkena GameObject ini
                {
                    ToggleTargetObject();
                }
            }
        }
    }

    private void ToggleTargetObject()
    {
        if (targetObject != null)
        {
            // Toggle status targetObject
            isTargetActive = !isTargetActive;
            targetObject.SetActive(isTargetActive);
        }
        else
        {
            Debug.LogWarning("Target Object belum diset!");
        }
    }
}
