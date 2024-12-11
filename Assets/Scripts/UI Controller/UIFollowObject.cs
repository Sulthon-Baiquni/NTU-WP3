using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIFollowObject : MonoBehaviour
{
    public Transform targetObject; // Objek yang ingin diikuti
    public Vector3 offset;         // Offset jarak di atas objek

    void Update()
    {
        if (targetObject != null)
        {
            // Set posisi UI tepat di atas objek, menggunakan posisi dunia
            transform.position = targetObject.position + offset;

            // Opsional: Pastikan rotasi UI tetap menghadap kamera
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Hanya rotasi horizontal
        }
    }
}
