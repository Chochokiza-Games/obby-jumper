using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxChanger : MonoBehaviour
{   
    [SerializeField] private PaletteChanger _paletteChanger;
    [SerializeField] private Material _skyBox;
    private Color[] _currentPaletteColors;


    public void OnLevelChanged()
    {
        // _currentPaletteColors = _paletteChanger.CurrentPalette.Colors;


        // (_currentPaletteColors[Random.Range(0, _currentPaletteColors.Length - 1)]);
        
    }
    private void Start()
    {

        RenderSettings.skybox = _skyBox;
        _skyBox.color = _currentPaletteColors[Random.Range(0, _currentPaletteColors.Length - 1)];

    }
}
