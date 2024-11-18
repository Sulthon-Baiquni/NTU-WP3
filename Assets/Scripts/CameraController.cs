using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 10f;     
    public float rotationSpeed = 100f;     
    public float zoomSpeed = 5f;          
    public float minZoomDistance = 5f;     
    public float maxZoomDistance = 50f;   

    private float pitch = 0f;              
    private float yaw = 0f;                
    private Camera cam;

    private void Start()
    {
        cam = Camera.main; 
        if (cam == null)
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    private void Update()
    {
        
        MoveCamera();

        
        RotateCamera();

        
        ZoomCamera();
    }

    private void MoveCamera()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical");   

        Vector3 moveDirection = transform.forward * moveZ + transform.right * moveX;
        transform.position += moveDirection * movementSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        
        if (Input.GetMouseButton(1))
        {
            
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            
            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }

    private void ZoomCamera()
    {
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        
        Vector3 zoomDirection = cam.transform.forward * scroll * zoomSpeed;
        cam.transform.position += zoomDirection;

        
        float distanceFromOrigin = Vector3.Distance(transform.position, cam.transform.position);
        if (distanceFromOrigin < minZoomDistance || distanceFromOrigin > maxZoomDistance)
        {
            cam.transform.position -= zoomDirection;
        }
    }
}
