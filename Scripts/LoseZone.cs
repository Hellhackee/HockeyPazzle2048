using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseZone : MonoBehaviour
{
    [SerializeField] private UI _ui;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Cube>(out Cube cube))
        {
            if (cube.IsPushed == true && cube.GetMagnitude() < 0.1f)
            {
                _ui.GameOver();
            }
        }
    }
}
