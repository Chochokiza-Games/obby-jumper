using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trace Color", menuName = "Trace/Color", order = 51)]
public class TraceColor : ScriptableObject 
{
    public int ShadesCount
    {
        get => _shades.Length;
    }

    [SerializeField] private Color[] _shades = new Color[5];

    public Color GetShade(int id)
    {
        return _shades[id];
    }
}