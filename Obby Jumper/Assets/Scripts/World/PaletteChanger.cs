using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteChanger : MonoBehaviour
{
    public Palette CurrentPalette
    {
        get => _currentPalette;
    }

    [SerializeField] private int _debugId = -1;
    [SerializeField] private Palette[] _palettes;

    private Palette _currentPalette;
    private int _prevPaletteId = -1;

    private void Awake()
    {   
        if (_palettes.Length > 1)
        {
            _currentPalette = _palettes[_debugId != -1 ? _debugId : Random.Range(0, _palettes.Length - 1)];
            Debug.Log($"Current Pallete: {_currentPalette}");
        } 
        else 
        {
            Debug.LogError("_palettes len < 2");
        }
    }

    public void OnLevelChanged()
    {
        if (_palettes.Length > 1)
        {
            Palette newPalette;
            do
            {
                newPalette = _palettes[Random.Range(0, _palettes.Length - 1)];
            } while(newPalette == _currentPalette);

            _currentPalette = newPalette;
        } else
        {
            Debug.LogError("_palettes len < 2");
        }
    }



}
