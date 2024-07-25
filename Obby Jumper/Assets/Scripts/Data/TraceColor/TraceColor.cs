using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trace Color", menuName = "Trace/Color", order = 51)]
public class TraceColor : ScriptableObject 
{
    public int ShadesCount
    {
        get => _colors.Length;
    }

    [SerializeField] private Color[] _colors = new Color[5];

    public Color GetShade(int id)
    {
        return _colors[id];
    }
}