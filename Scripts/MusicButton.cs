using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MusicButton : MonoBehaviour
{
    [SerializeField] private Sprite _musicOnIcon;
    [SerializeField] private Sprite _musicOffIcon;

    private Image _music;
    private bool _musicEnable = true;
    private AudioSource _audioSource;

    private void Start()
    {
        _music = GetComponent<Image>();
        _audioSource = GameObject.FindObjectOfType<AudioSource>();

        int musicEnable = PlayerPrefs.GetInt("Music", 1);

        if (_audioSource != null)
        {
            if (musicEnable != 1)
            {
                _musicEnable = false;
                _audioSource.volume = 0f;

            }
            else
            {
                _musicEnable = true;
                _audioSource.volume = 100f;
            }

            ChangeSprite();
        }
    }

    public void OnMusicButtonPressed()
    {
        if (_audioSource != null)
        {
            if (_musicEnable == true)
            {
                _audioSource.volume = 0f;
                _musicEnable = false;
                PlayerPrefs.SetInt("Music", 0);
            }
            else
            {
                _audioSource.volume = 100f;
                _musicEnable = true;
                PlayerPrefs.SetInt("Music", 1);
            }

            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        if (_music != null)
        {
            if (_musicEnable == true)
            {
                _music.sprite = _musicOnIcon;
            }
            else
            {
                _music.sprite = _musicOffIcon;
            }
        }
    }
}
