using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "Palette", order = 51)]
public class Palette : ScriptableObject
{
    public Color[] Colors
    {
        get => _colors;
    }  
    [SerializeField] private Color[] _colors;

}