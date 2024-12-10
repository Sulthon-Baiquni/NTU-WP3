using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;

    public float fillHeight;

    public Color fillColor;
    public Color defaultColor;

    private void Update()
    {
        foreach (GameObject go in gameObjects)
        {
            if (go.transform.position.y < fillHeight)
            {
                go.GetComponent<Renderer>().material.color = fillColor;
            }
            else 
            {
                go.GetComponent<Renderer>().material.color = defaultColor;
            }
        }

    }

}
