using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Camera mainCamera;        
    public Transform targetObject;   
    public float focusSpeed = 2.0f;  
    public Vector3 offset;           

    private bool shouldFocus = false; 

    void Update()
    {
        if (shouldFocus && mainCamera != null && targetObject != null)
        {
            // Pindahkan posisi kamera ke target
            Vector3 targetPosition = targetObject.position + offset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, focusSpeed * Time.deltaTime);

            // Rotasi kamera ke arah target
            Vector3 direction = targetObject.position - mainCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, focusSpeed * Time.deltaTime);

            // Jika kamera cukup dekat ke target, matikan fokus
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.1f)
            {
                shouldFocus = false;
            }
        }
    }

    // Fungsi untuk memulai fokus kamera
    public void StartFocus()
    {
        shouldFocus = true;
    }
}
