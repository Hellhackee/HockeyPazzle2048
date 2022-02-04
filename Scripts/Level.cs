using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    private int _index;

    public void Init(int index)
    {
        _index = index;

        _label.text = _index.ToString();
    }

    public void OnButtonPressed()
    {
        SceneManager.LoadScene(_index);
    }
}
