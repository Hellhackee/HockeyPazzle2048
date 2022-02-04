using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSlider : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private TouchSlider _slider;
    [SerializeField] private float _cubeMaxPosZ;
    [SerializeField] private float _baseCubeMoveSpeed;
    [SerializeField] private Image _handler;
    [SerializeField] private Sprite _fingerDown;
    [SerializeField] private Sprite _fingerUp;

    private bool _isPointerDown = false;
    private float _cubeSpawnTime;
    private Cube _cube;
    private bool _canMove = false;
    private Sequence _moveSequence;

    private void OnEnable()
    {
        _slider.OnPointerDownHandler += OnPointerDown;
        _slider.OnPointerUpHandler += OnPointerUp;        
    }

    private void Start()
    {
        _cubeSpawnTime = _spawner.Cooldown;
        _handler.sprite = _fingerUp;

        StartCoroutine(InstantiateCube(0f));
    }

    private void Update()
    {
        if (_cube != null && _canMove == true)
        {
            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(_cube.transform.DOMoveZ(_cubeMaxPosZ, _baseCubeMoveSpeed).SetLoops(int.MaxValue, LoopType.Yoyo).SetEase(Ease.Linear));
            _moveSequence.Play();

            _canMove = false;
        }
    }

    private void OnPointerUp()
    {
        if (_isPointerDown == true && _cube != null)
        {
            _handler.sprite = _fingerUp;
            _isPointerDown = false;
            _cube.Push(_slider.GetSliderValue());
            _cube.CanMove -= OnCubeCanMove;
            _canMove = false;
            _cube = null;

            if (_moveSequence != null)
            {
                _moveSequence.Kill();
            }

            StartCoroutine(InstantiateCube(_cubeSpawnTime));
        }
    }

    private void OnPointerDown()
    {
        _isPointerDown = true;
        _handler.sprite = _fingerDown;
    }

    private IEnumerator InstantiateCube(float spawnTime)
    {
        _canMove = false;

        yield return new WaitForSeconds(spawnTime);

        Vector3 spawnPosition = _spawner.transform.position;
        spawnPosition.z -= _cubeMaxPosZ;

        _cube = _spawner.InstantiateCube(spawnPosition);
        _cube.CanMove += OnCubeCanMove;
    }

    private void OnCubeCanMove()
    {
        _canMove = true;
    }
}
