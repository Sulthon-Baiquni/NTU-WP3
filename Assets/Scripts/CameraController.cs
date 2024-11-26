using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 2f; // Kecepatan pergerakan kamera
    public float rotationSpeed = 100f; // Kecepatan rotasi kamera
    public float zoomSpeed = 5f; // Kecepatan zoom
    public float minZoomDistance = 5f; // Jarak minimum zoom
    public float maxZoomDistance = 50f; // Jarak maksimum zoom

    private float pitch = 0f; // Sudut rotasi vertikal
    private float yaw = 0f; // Sudut rotasi horizontal
    private Camera cam; // Referensi ke kamera utama

    private Vector3 dragOrigin; // Posisi awal drag mouse

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
        PanCamera(); // Memproses geser peta dengan klik kiri
        RotateCamera(); // Memproses rotasi kamera
        ZoomCamera(); // Memproses zoom kamera
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0)) // Ketika klik kiri ditekan
        {
            dragOrigin = Input.mousePosition; // Simpan posisi awal drag
        }

        if (Input.GetMouseButton(0)) // Ketika klik kiri di-drag
        {
            Vector3 dragDelta = Input.mousePosition - dragOrigin; // Hitung pergeseran mouse
            dragOrigin = Input.mousePosition; // Update posisi awal untuk frame berikutnya

            // Hitung arah pan (geser) berdasarkan sumbu horizontal dan vertikal
            float panX = -dragDelta.x * movementSpeed * Time.deltaTime;
            float panZ = -dragDelta.y * movementSpeed * Time.deltaTime;

            // Update posisi kamera
            transform.position += transform.right * panX + transform.up * panZ;
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
            // Mendapatkan arah zoom
            Vector3 zoomDirection = cam.transform.forward * scroll * zoomSpeed;

            // Update posisi kamera berdasarkan input scroll
            cam.transform.position += zoomDirection;
        }
    }
}
