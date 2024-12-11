using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectScaler : MonoBehaviour
{
    public Transform pipePath;

    private void Update()
    {
        // Dapatkan skala pipa pada sumbu Y
        float pipeScale = pipePath.localScale.y;

        // Setel skala cylinder agar mengikuti skala pipa pada sumbu Y
        transform.localScale = new Vector3(
            transform.localScale.x,
            pipeScale,
            transform.localScale.z
        );
    }
}
