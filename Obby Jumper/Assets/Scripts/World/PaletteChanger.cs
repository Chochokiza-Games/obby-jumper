using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteChanger : MonoBehaviour
{
    public Palette CurrentPalette
    {
        get => _currentPalette;
    }

    [SerializeField] private Palette[] _palettes;

    private Palette _currentPalette;
    private int _prevPaletteId = -1;

    private void Awake()
    {   
        if (_palettes.Length > 1)
        {
            _currentPalette = _palettes[Random.Range(0, _palettes.Length - 1)];
        } else 
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
