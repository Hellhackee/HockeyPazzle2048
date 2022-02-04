using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyLabel;
    [SerializeField] private GameObject _gameOverPanel;

    private int _money = 0;

    private void Start()
    {
        _gameOverPanel.SetActive(false);
    }

    public void AddMoney(int value)
    {
        _money += value;
        _moneyLabel.text = _money.ToString();
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene(0);
    }
}
