using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteRecognitionGame : MonoBehaviour
{
    public Button playButton; // Кнопка "Play"
    public Slider progressSlider; // Слайдер прогресса уровня
    public Text levelText; // Текст, отображающий уровень
    public List<Button> pianoKeys; // Список всех клавиш
    public Button[] musicStaffNotes; // Список кнопок нот на нотном стане
    public AudioClip[] noteSounds; // Массив звуков нот
    public AudioSource audioSource; // Аудиоисточник для воспроизведения звуков

    private int currentLevel = 1; // Текущий уровень
    private List<int> currentSequence = new List<int>(); // Последовательность нот на текущем уровне
    private int currentNoteIndex; // Индекс текущей ноты в последовательности

    private bool isInputEnabled = false; // Флаг, можно ли нажимать на клавиши
    private bool isLevelStarted = false; // Флаг, запущен ли уровень
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>(); // Хранение оригинальных цветов

    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        progressSlider.value = 0;
        progressSlider.maxValue = 1; // Изначально 1, пересчитывается в StartLevel
        levelText.text = "Level " + currentLevel;

        // Настройка клавиш пианино
        foreach (Button key in pianoKeys)
        {
            originalColors[key] = key.GetComponent<Image>().color;
            key.onClick.AddListener(() => OnPianoKeyClick(key));
        }

        // Настройка кнопок для нотного стана
        foreach (Button note in musicStaffNotes)
        {
            originalColors[note] = note.GetComponent<Image>().color;
            note.onClick.AddListener(() => OnMusicStaffNoteClick(note));
        }
    }

    void HighlightKey(Button key, Color color)
    {
        key.GetComponent<Image>().color = color;
    }

    void ResetKeyColor(Button key)
    {
        if (originalColors.ContainsKey(key))
        {
            key.GetComponent<Image>().color = originalColors[key];
        }
    }

    void OnPianoKeyClick(Button pressedKey)
    {
        if (!isInputEnabled) return;

        int pressedKeyIndex = pianoKeys.IndexOf(pressedKey);

        if (pressedKeyIndex == currentSequence[currentNoteIndex])
        {
            HighlightKey(pressedKey, Color.green);
            progressSlider.value = currentNoteIndex + 1;
            currentNoteIndex++;

            if (currentNoteIndex >= currentSequence.Count)
            {
                isInputEnabled = false;
                StartCoroutine(LevelComplete());
            }
        }
        else
        {
            HighlightKey(pressedKey, Color.red);
            Button correctKey = pianoKeys[currentSequence[currentNoteIndex]];
            HighlightKey(correctKey, Color.green);

            StartCoroutine(ResetGameWithDelay());
        }
    }

    void OnMusicStaffNoteClick(Button pressedNote)
    {
        if (!isInputEnabled) return;

        int pressedNoteIndex = System.Array.IndexOf(musicStaffNotes, pressedNote);

        if (pressedNoteIndex == currentSequence[currentNoteIndex])
        {
            HighlightKey(pressedNote, Color.green);
            progressSlider.value = currentNoteIndex + 1;
            currentNoteIndex++;

            if (currentNoteIndex >= currentSequence.Count)
            {
                isInputEnabled = false;
                StartCoroutine(LevelComplete());
            }
        }
        else
        {
            HighlightKey(pressedNote, Color.red);
            Button correctNote = musicStaffNotes[currentSequence[currentNoteIndex]];
            HighlightKey(correctNote, Color.green);

            StartCoroutine(ResetGameWithDelay());
        }
    }

    IEnumerator ResetGameWithDelay()
    {
        isInputEnabled = false;
        isLevelStarted = false;

        yield return new WaitForSeconds(1f);

        foreach (Button key in pianoKeys)
        {
            ResetKeyColor(key);
        }

        foreach (Button note in musicStaffNotes)
        {
            ResetKeyColor(note);
        }

        currentLevel = 1;
        levelText.text = "Level " + currentLevel;
        progressSlider.value = 0;
        progressSlider.maxValue = 1;
    }

    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(0.5f);

        if (currentLevel > 12)
        {
            Debug.Log("Игра пройдена!");
            yield break;
        }

        foreach (Button key in pianoKeys)
        {
            ResetKeyColor(key);
        }

        foreach (Button note in musicStaffNotes)
        {
            ResetKeyColor(note);
        }

        currentLevel++;
        progressSlider.maxValue = currentLevel;
        progressSlider.value = 0;
        levelText.text = "Level " + currentLevel;

        isLevelStarted = false;
    }

    void OnPlayButtonClicked()
    {
        if (!isLevelStarted)
        {
            StartLevel();
        }
        else
        {
            StartCoroutine(PlaySequence());
        }
    }

    void StartLevel()
    {
        isLevelStarted = true;
        currentSequence.Clear();
        currentNoteIndex = 0;

        progressSlider.maxValue = currentLevel;
        progressSlider.value = 0;

        for (int i = 0; i < currentLevel; i++)
        {
            currentSequence.Add(Random.Range(0, noteSounds.Length));
        }

        string sequenceString = "Уровень " + currentLevel + ": последовательность клавиш - ";
        foreach (int index in currentSequence)
        {
            sequenceString += index + " ";
        }
        Debug.Log(sequenceString);

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        isInputEnabled = false;

        foreach (int noteIndex in currentSequence)
        {
            audioSource.PlayOneShot(noteSounds[noteIndex]);
            yield return new WaitForSeconds(1f);
        }

        isInputEnabled = true;
    }
}
