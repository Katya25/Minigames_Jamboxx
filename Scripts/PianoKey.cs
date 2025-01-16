using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoKey : MonoBehaviour
{
    public AudioClip sound; // Сюда добавим нужный звук
    private AudioSource audioSource;

    private Button button; // Ссылка на кнопку
    private Color originalColor; // Цвет кнопки по умолчанию
    public Color highlightColor = Color.gray; // Цвет подсветки при нажатии

    void Start()
    {
        // Получаем необходимые компоненты
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound;

        button = GetComponent<Button>();
        originalColor = button.image.color;

        // Добавляем метод PlaySound() в onClick кнопки
        button.onClick.AddListener(PlaySound);
    }

    public void PlaySound()
    {
        // Воспроизводим звук
        if (sound != null)
        {
            audioSource.Play();
        }
    }

}
