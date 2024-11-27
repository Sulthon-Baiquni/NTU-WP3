using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllers : MonoBehaviour
{
    public UIFollowObject uiFollow; // Script yang mengatur UI mengikuti objek
    public GameObject uiImage;      // Referensi ke UI Image
    public UnityEngine.UI.Dropdown dropdown;       // Dropdown yang digunakan
    public List<Transform> targetObjects; // Objek target yang dihubungkan ke dropdown

    private void Start()
    {
        if (uiImage != null)
        {
            uiImage.SetActive(false); // Pastikan UI Image tidak aktif saat awal
        }

        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged); // Tambahkan listener untuk dropdown
        }
    }

    private void OnDropdownValueChanged(int index)
    {
        if (index < 0 || index >= targetObjects.Count)
        {
            Debug.LogWarning("Index dropdown tidak valid: " + index);
            uiImage.SetActive(false); // Sembunyikan UI Image jika tidak valid
            return;
        }

        Transform target = targetObjects[index];

        if (target != null && uiFollow != null)
        {
            // Tampilkan UI Image dan set target-nya
            uiImage.SetActive(true);
            uiFollow.targetObject = target; // Berikan target ke script UIFollowObject
        }
        else
        {
            uiImage.SetActive(false); // Sembunyikan UI Image jika tidak ada target
        }
    }
}
