using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class StaticBendingCylinder : MonoBehaviour
{
    public float maxStraightLength = 5f; // Panjang lurus maksimal
    public float bendRadius = 3f; // Radius kelengkungan
    public float bendAngle = 90f; // Sudut kelengkungan dalam derajat
    public Vector3 bendAxis = Vector3.right; // Sumbu kelengkungan (misalnya ke kanan)

    private MeshFilter meshFilter;
    private Mesh originalMesh;
    private Vector3[] originalVertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.mesh;
        originalVertices = originalMesh.vertices;
        ApplyBend();
    }

    void ApplyBend()
    {
        if (originalMesh == null || originalVertices == null) return;

        Vector3[] modifiedVertices = new Vector3[originalVertices.Length];
        Quaternion rotation = Quaternion.AngleAxis(bendAngle, bendAxis.normalized);

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];

            // Jika vertex dalam panjang lurus
            if (vertex.y <= maxStraightLength)
            {
                modifiedVertices[i] = vertex;
            }
            else
            {
                // Panjang relatif terhadap awal kelengkungan
                float relativeY = vertex.y - maxStraightLength;

                // Hitung sudut kelengkungan berdasarkan posisi Y
                float angle = Mathf.Min((relativeY / bendRadius) * Mathf.Rad2Deg, bendAngle);

                // Rotasi posisi awal berdasarkan sudut kelengkungan
                Vector3 offset = new Vector3(0, maxStraightLength, 0);
                Vector3 relativePos = vertex - offset;
                relativePos = rotation * relativePos;

                // Kembalikan ke posisi dunia
                modifiedVertices[i] = offset + relativePos;
            }
        }

        // Perbarui mesh dengan vertex yang dimodifikasi
        originalMesh.vertices = modifiedVertices;
        originalMesh.RecalculateNormals();
        originalMesh.RecalculateBounds();
    }

    void OnValidate()
    {
        if (meshFilter == null) return;
        ApplyBend();
    }
}
