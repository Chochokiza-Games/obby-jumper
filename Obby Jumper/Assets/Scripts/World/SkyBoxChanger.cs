using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxChanger : MonoBehaviour
{   
    [SerializeField] private bool _debug;
    [SerializeField] private PaletteChanger _paletteChanger;
    private Material _skyBox;


    public void OnLevelChanged()
    {
        PaintSkyboxFromPalette();

    }

    private void PaintSkyboxFromPalette()
    {
        Debug.Log(_paletteChanger.CurrentPalette.Colors[0]);
        _skyBox.SetColor("_SkyColor", _paletteChanger.CurrentPalette.Colors[0]);
        _skyBox.SetColor("_EquatorColor", _paletteChanger.CurrentPalette.Colors[1]);
        _skyBox.SetColor("_GroundColor", _paletteChanger.CurrentPalette.Colors[2]);
    } 

    private void Start()
    {
        _skyBox = RenderSettings.skybox;
        PaintSkyboxFromPalette();    
    }

    public void Update()
    {
        if (_debug)
        {
            PaintSkyboxFromPalette();
        }
    }
}
