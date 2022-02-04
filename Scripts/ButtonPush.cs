using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonPush : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField]private float _cubeMaxPosZ;
    [SerializeField] private float _baseCubeMoveSpeed;
    
    private float _cubeSpawnTime;
    private Cube _cube;
    private bool _canMove = false;
    private Sequence _moveSequence;

    private void Start()
    {
        _cubeSpawnTime = _spawner.Cooldown;

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

    public void OnPushButtonPressed()
    {
        if (_cube != null)
        {
            _cube.Push();
            
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
