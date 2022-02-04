using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorScene : MonoBehaviour
{
    [Header("Plane")]
    [SerializeField] private Material _planeMaterial;
    [SerializeField] private Color[] _planeColors;

    [Header("Box wall")]
    [SerializeField] private Material _boxWallMaterial;
    [SerializeField] private Color[] _boxWallColors;

    private void Awake()
    {
        _planeMaterial.color = _planeColors[Random.Range(0, _planeColors.Length)];
        _boxWallMaterial.color = _boxWallColors[Random.Range(0, _boxWallColors.Length)];
    }
}
