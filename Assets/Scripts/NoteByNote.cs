using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteByNote : MonoBehaviour
{
    public Button playButton; // Кнопка Play
    public List<Button> pianoKeys; // Кнопки пианино
    public Button[] musicStaffNotes; // Кнопки для нотного стана

    private int correctNoteIndex; // Индекс правильной ноты
    private bool isInputEnabled = false; // Флаг, можно ли нажимать клавиши

    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>(); // Хранение оригинальных цветов для сброса

    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);

        // Сохраняем оригинальные цвета для всех клавиш пианино
        foreach (Button key in pianoKeys)
        {
            originalColors[key] = key.GetComponent<Image>().color;
            key.onClick.AddListener(() => OnPianoKeyClick(key)); // Добавляем обработчик нажатия
        }

        // Сохраняем оригинальные цвета для всех нот
        foreach (Button note in musicStaffNotes)
        {
            originalColors[note] = note.GetComponent<Image>().color;
        }

        // Скрываем все ноты на нотном стане в начале
        foreach (Button note in musicStaffNotes)
        {
            note.gameObject.SetActive(false);
        }
    }

    // Логика нажатия клавиш пианино
    void OnPianoKeyClick(Button pressedKey)
    {
        if (!isInputEnabled) return; // Игнорируем нажатия, если они отключены

        int pressedKeyIndex = pianoKeys.IndexOf(pressedKey);

        // Если нажата правильная клавиша
        if (pressedKeyIndex == correctNoteIndex)
        {
            HighlightKey(pressedKey, Color.green); // Подсветка зелёным
            StartCoroutine(LevelComplete()); // Увеличиваем уровень
        }
        else
        {
            HighlightKey(pressedKey, Color.red); // Подсветка красным для неправильной клавиши
            Button correctKey = pianoKeys[correctNoteIndex];
            HighlightKey(correctKey, Color.green); // Подсвечиваем правильную клавишу зелёным
            StartCoroutine(ResetGameWithDelay()); // Сброс через 1 секунду
        }

        isInputEnabled = false; // Блокируем ввод до завершения проверки
    }

    // Подсветить клавишу или ноту заданным цветом
    void HighlightKey(Button key, Color color)
    {
        key.GetComponent<Image>().color = color;
    }

    // Сбросить игру через 1 секунду
    IEnumerator ResetGameWithDelay()
    {
        yield return new WaitForSeconds(1f);

        // Сбрасываем цвета всех клавиш пианино
        foreach (Button key in pianoKeys)
        {
            ResetKeyColor(key);
        }

        // Сбрасываем цвета всех нот
        foreach (Button note in musicStaffNotes)
        {
            ResetKeyColor(note);
        }

        isInputEnabled = true; // Разрешаем снова нажимать на клавиши
    }

    // Сброс цвета кнопки
    void ResetKeyColor(Button key)
    {
        if (originalColors.ContainsKey(key))
        {
            key.GetComponent<Image>().color = originalColors[key];
        }
    }

    // Когда нажимается кнопка Play
    void OnPlayButtonClicked()
    {
        ShowRandomNoteOnStaff();
    }

    // Показать случайную ноту на нотном стане
    void ShowRandomNoteOnStaff()
    {
        // Скрываем все ноты
        foreach (Button note in musicStaffNotes)
        {
            note.gameObject.SetActive(false);
        }

        // Выбираем случайную ноту
        correctNoteIndex = Random.Range(0, musicStaffNotes.Length);
        musicStaffNotes[correctNoteIndex].gameObject.SetActive(true); // Показываем одну случайную ноту

        // Разрешаем ввод с клавиш пианино
        isInputEnabled = true;
    }

    // Увеличить уровень и перейти к следующей ноте
    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(0.5f);

        // Сброс цветов всех клавиш и нот
        foreach (Button key in pianoKeys)
        {
            ResetKeyColor(key);
        }
        foreach (Button note in musicStaffNotes)
        {
            ResetKeyColor(note);
        }

        ShowRandomNoteOnStaff(); // Показать новую случайную ноту
    }
}
