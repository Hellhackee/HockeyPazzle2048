using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Cube _cube;
    [SerializeField] private float _cubeSize;
    [SerializeField] private float _offsetZ;
    [SerializeField] private int[] _startNumbers;
    [SerializeField] private int[]  _numbers;
    [SerializeField] private Material[] _colors;

    Dictionary<int, Material> _numbersAndColors = new Dictionary<int, Material>();
    public float Cooldown => _cooldown;
    public float CubeSize => _cubeSize;
    public float OffsetZ => _offsetZ;

    private void Awake()
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            _numbersAndColors.Add(_numbers[i], _colors[i]);
        }
    }

    public Cube InstantiateCube(Vector3 position, int number = 0)
    {
        if (number == 0)
        {
            int dictionaryIndex = Random.Range(0, _startNumbers.Length);
            number = _numbers[dictionaryIndex];
        }

        Material material = _numbersAndColors[number];

        Vector3 cubePosition = position;
        Cube cube = Instantiate(_cube, cubePosition, Quaternion.identity, transform);

        cube.SetNumber(number);
        cube.SetColor(material);

        return cube;
    }
}
