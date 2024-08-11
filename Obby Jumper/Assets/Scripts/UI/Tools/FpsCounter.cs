using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;

    private void Update()
    {
        _label.text = (1f / Time.deltaTime).ToString("N0");
    }
}
