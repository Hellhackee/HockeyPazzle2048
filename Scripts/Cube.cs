using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class Cube : MonoBehaviour
{
    public static int MainID = 0;

    [SerializeField] private float _force;
    [SerializeField] private GameObject _cube;
    [SerializeField] private TMP_Text[] _labels;
    [SerializeField] private ParticleSystem _dieEffect;

    private Spawner _spawner;
    private float _scaleSize;
    private int _number;
    private bool _isPushed = false;
    private Rigidbody _rb;
    private Collider _collider;
    private UI _ui;
    private Sequence _moveSequence;
    private int ID;   

    public event UnityAction CanMove;

    public int Number => _number;
    public bool IsPushed => _isPushed;

    private void Awake()
    {
        ID = MainID++;

        _spawner = GameObject.FindObjectOfType<Spawner>();
        _ui = GameObject.FindObjectOfType<UI>();
        _scaleSize = _spawner.CubeSize;

        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
            transform.DOScale(_scaleSize, 0.5f).OnComplete(SetCanMove);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Cube>(out Cube cube))
        {
            if (ID > cube.ID)
            {
                if (_isPushed == true && cube.IsPushed == true)
                {
                    if (cube.Number == _number)
                    {
                        Die();
                        cube.Die();

                        int newNumber = _number * 2;
                        Vector3 contactPoint = collision.contacts[0].point;

                        Cube newCube = _spawner.InstantiateCube(contactPoint, newNumber);
                        newCube.SetPushed();
                        newCube.SetBasicScale();
                        newCube.Explosive(contactPoint);

                        if (newNumber == 1024)
                        {
                            _ui.AddMoney(newNumber);

                            Destroy(newCube.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void Push(float forcePercent = 1f)
    {
        _rb.AddForce(Vector3.left * _force * forcePercent, ForceMode.Impulse);

        SetPushed();
    }

    public void Explosive(Vector3 contactPoint)
    {
        _rb.AddForce(new Vector3(-1f, 0.1f, 0f) * 5f, ForceMode.Impulse);

        float randomForceValue = Random.Range(-20f, 20f);
        Vector3 randomDirection = Vector3.one * randomForceValue;
        _rb.AddTorque(randomDirection);

        Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
        float explosionForce = 200f;
        float explosionRadius = 1.5f;

        foreach (Collider item in surroundedCubes)
        {
            if (item.TryGetComponent<Cube>(out Cube cube))
            {
                cube.AddExplosion(explosionForce, contactPoint, explosionRadius);
            }
        }
    }

    private void SetCanMove()
    {
        CanMove?.Invoke();
    }

    public void SetNumber(int value)
    {
        _number = value;

        foreach (var label in _labels)
        {
            label.text = value.ToString();
        }
    }

    public void SetBasicScale()
    {
        transform.DOScale(_scaleSize, 0f);
    }

    public void SetPushed()
    {
        _isPushed = true;
    }

    public void SetColor(Material newMaterial)
    {
        MeshRenderer renderer = _cube.GetComponent<MeshRenderer>();
        renderer.material = newMaterial;

        _dieEffect.startColor = newMaterial.color;
    }

    public float GetMagnitude()
    {
        return _rb.velocity.magnitude;
    }

    public void AddExplosion(float explosionForce, Vector3 contactPoint, float explosionRadius)
    {
        _rb.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
    }

    public void Die()
    {
        _cube.SetActive(false);
        Destroy(_collider);
        Destroy(_rb);

        _dieEffect.Play();

        Destroy(gameObject, 2f);
    }
}
