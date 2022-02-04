using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Music _musicPrefab;
    [SerializeField] private MusicButton _musicButtonPrefab;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private Level _levelPrefab;

    private void Awake()
    {
        Music music = GameObject.FindObjectOfType<Music>();
        MusicButton musicButton = GameObject.FindObjectOfType<MusicButton>();

        if (music == null)
        {
            Instantiate(_musicPrefab);
        }

        if (musicButton == null)
        {
            Instantiate(_musicButtonPrefab);
        }

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount -1; i++)
        {
            Level level = Instantiate(_levelPrefab, _levelsPanel.transform);
            level.Init(i + 1);
        }
    }

    public void OnStartButtonPressed()
    {
        _startButton.SetActive(false);
        _levelsPanel.SetActive(true);
    }
}
