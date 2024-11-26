using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 10f; // Kecepatan pergerakan kamera (geser)
    public float rotationSpeed = 100f; // Kecepatan rotasi kamera
    public float zoomSpeed = 5f; // Kecepatan zoom
    public float minZoomDistance = 5f; // Jarak minimum zoom
    public float maxZoomDistance = 50f; // Jarak maksimum zoom

    private float pitch = 0f; // Sudut rotasi vertikal
    private float yaw = 0f; // Sudut rotasi horizontal
    private Camera cam; // Referensi ke kamera utama

    private void Start()
    {
        cam = Camera.main; // Mendapatkan kamera utama
        if (cam == null)
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    private void Update()
    {
        MoveCamera(); // Memproses pergerakan kamera
        RotateCamera(); // Memproses rotasi kamera
        ZoomCamera(); // Memproses zoom kamera
    }

    private void MoveCamera()
    {
        if (Input.GetMouseButton(0)) // Klik kiri untuk menggeser
        {
            float moveX = Input.GetAxis("Mouse X");
            float moveZ = Input.GetAxis("Mouse Y");

            // Arah pergerakan relatif terhadap kamera
            Vector3 moveDirection = (transform.right * moveX + transform.up * moveZ) * movementSpeed * Time.deltaTime;

            // Update posisi kamera
            transform.position += moveDirection;
        }
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButton(1)) // Klik kanan untuk rotasi
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            yaw += mouseX; // Update rotasi horizontal
            pitch -= mouseY; // Update rotasi vertikal

            // Membatasi sudut vertikal agar kamera tidak terbalik
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            // Update rotasi kamera
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }

    private void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f) // Pastikan ada input dari scroll
        {
            // Mendapatkan posisi target zoom
            Vector3 zoomDirection = cam.transform.forward * scroll * zoomSpeed;
            Vector3 newCameraPosition = cam.transform.position + zoomDirection;

            // Menghitung jarak antara kamera dan posisi asalnya
            float distanceFromOrigin = Vector3.Distance(transform.position, newCameraPosition);

            // Membatasi jarak zoom
            if (distanceFromOrigin >= minZoomDistance && distanceFromOrigin <= maxZoomDistance)
            {
                cam.transform.position = newCameraPosition;
            }
        }
    }
}
