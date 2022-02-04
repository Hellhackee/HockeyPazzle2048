using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderPusher : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _cubeMaxPosZ;
    [SerializeField] private TouchSlider _slider;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Image _handler;
    [SerializeField] private Sprite _fingerDown;
    [SerializeField] private Sprite _fingerUp;
    [SerializeField] private bool _invertSliderValue;
    [SerializeField] private bool _moveCubeAfterPush;

    private float _cubeSpawnTime;
    private float _offsetZ;
    private Cube _cube;
    private bool _isPointerDown = false;
    private Vector3 _cubePosition;
    private bool _canMove = false;

    private void OnEnable()
    {
        _slider.OnPointerDownHandler += OnPointerDown;
        _slider.OnPointerUpHandler += OnPointerUp;
        _slider.OnSliderDragHandler += OnPointerDrag;
    }

    private void Start()
    {
        _cubeSpawnTime = _spawner.Cooldown;
        _offsetZ = _spawner.OffsetZ;
        _handler.sprite = _fingerUp;

        StartCoroutine(InstantiateCube(0f));
    }

    private void Update()
    {
        if (_isPointerDown == true && _cube != null && _canMove == true)
        {
            _cube.transform.position = Vector3.Lerp(_cubePosition, _cubePosition, _speed * Time.deltaTime);
        }
    }

    private void OnPointerUp()
    {
        if (_isPointerDown == true && _cube != null && _canMove == true)
        {
            _handler.sprite = _fingerUp;
            _isPointerDown = false;
            _cube.Push();
            _cube.CanMove -= OnCubeCanMove;
            _canMove = false;
            _cube = null;

            StartCoroutine(InstantiateCube(_cubeSpawnTime));
        }
    }

    private void OnPointerDown()
    {
        _isPointerDown = true;
        _handler.sprite = _fingerDown;
    }

    private void OnPointerDrag(float value)
    {
        if (_invertSliderValue == true)
        {
            value *= -1;
        }

        float OffsetZ = _offsetZ;

        if (_invertSliderValue == true)
        {
            OffsetZ *= -1;
        }

        if (_isPointerDown == true && _cube != null && _canMove == true)
        {
            _cubePosition = _cube.transform.position;
            _cubePosition.z = value * _cubeMaxPosZ + OffsetZ;
        }
    }

    private void OnDisable()
    {
        _slider.OnPointerDownHandler -= OnPointerDown;
        _slider.OnPointerUpHandler -= OnPointerUp;
        _slider.OnSliderDragHandler -= OnPointerDrag;
    }

    private IEnumerator InstantiateCube(float spawnTime)
    {
        _canMove = false;

        yield return new WaitForSeconds(spawnTime);

        Vector3 spawnPosition = _spawner.transform.position;
        float OffsetZ = _offsetZ;

        if (_invertSliderValue == true)
        {
            OffsetZ *= -1;
        }

        spawnPosition.z = spawnPosition.z + OffsetZ;
        _cube = _spawner.InstantiateCube(spawnPosition);
        _cube.CanMove += OnCubeCanMove;
    }

    private void OnCubeCanMove()
    {
        _canMove = true;
    }
}
