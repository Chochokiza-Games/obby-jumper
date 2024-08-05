using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail", menuName = "Trail/Info", order = 51)]

public class TrailInfo : ItemInfo
{
    public Color Color
    {
        get => _color;
    }

    [SerializeField] private Color _color;
}