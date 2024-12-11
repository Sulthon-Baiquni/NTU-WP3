using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColorManagement : MonoBehaviour
{
    // Warna
    public enum WaterColor
    {
        Brown,
        Orange,
        Blue,
        White
    }

    // Parameter air
    [System.Serializable]
    public class WaterParameters
    {
        public float pH;
        public float turbidity;
        public float tds;
    }

    // warna air berdasarkan parameter
    public static WaterColor DetermineWaterColor(WaterParameters parameters)
    {
        // Warna Brown
        if (parameters.pH < 5.0f || parameters.pH > 9.5f ||
            parameters.turbidity > 30f ||
            parameters.tds > 1500f)
        {
            return WaterColor.Brown;
        }

        // Warna Orange
        if ((parameters.pH >= 5.0f && parameters.pH < 6.0f) ||
            (parameters.pH >= 8.5f && parameters.pH <= 9.5f) ||
            (parameters.turbidity >= 10f && parameters.turbidity <= 30f) ||
            (parameters.tds > 1000f && parameters.tds <= 1500f))
        {
            return WaterColor.Orange;
        }

        // Warna Blue
        if (parameters.pH >= 6.0f && parameters.pH <= 9.0f &&
            parameters.turbidity < 10f &&
            parameters.tds >= 500f && parameters.tds <= 1000f)
        {
            return WaterColor.Blue;
        }

        // Warna White
        if (parameters.pH >= 6.5f && parameters.pH <= 8.0f &&
            parameters.turbidity < 5f &&
            parameters.tds < 500f)
        {
            return WaterColor.White;
        }

        // Default
        return WaterColor.Brown;
    }
}
