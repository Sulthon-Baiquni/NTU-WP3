using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public CameraFocus cameraFocusScript; // Referensi ke skrip CameraFocus
    void Update()
    {
        //klik mouse kiri
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Jika objek ini terkena raycast, mulai fokus kamera
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    cameraFocusScript.StartFocus();
                }
            }
        }
    }
}
