using UnityEngine;
using UnityEngine.UI;

public class GameModeSwitch : MonoBehaviour
{
    public GameObject pianoContainer; // Контейнер с пианино
    public GameObject sheetMusicContainer; // Контейнер с нотным станом
    public Button keyboardButton; // Кнопка для переключения на пианино
    public Button musicStaffButton; // Кнопка для переключения на нотный стан
    public RectTransform bg; // Родительский Image bg

    // Начальные локальные координаты
    private Vector3 pianoInitialPosition = new Vector3(-20f, -205f, 0f);
    private Vector3 sheetMusicInitialPosition = new Vector3(24f, -480f, 0f);

    // Локальные координаты для того, чтобы объект оказался за пределами экрана
    private Vector3 offscreenPosition = new Vector3(5000f, 5000f, 0f); // Объект далеко за экран

    void Start()
    {
        // Изначально показываем пианино, а нотный стан скрываем
        pianoContainer.GetComponent<RectTransform>().localPosition = pianoInitialPosition;
        sheetMusicContainer.GetComponent<RectTransform>().localPosition = offscreenPosition;

        // Назначаем события для кнопок
        keyboardButton.onClick.AddListener(SwitchToKeyboard);
        musicStaffButton.onClick.AddListener(SwitchToMusicStaff);
    }

    // Переключиться на режим пианино
    void SwitchToKeyboard()
    {
        // Перемещаем пианино в начальную позицию
        pianoContainer.GetComponent<RectTransform>().localPosition = pianoInitialPosition;

        // Перемещаем нотный стан за пределы экрана
        sheetMusicContainer.GetComponent<RectTransform>().localPosition = offscreenPosition;
    }

    // Переключиться на режим нотного стана
    void SwitchToMusicStaff()
    {
        // Перемещаем пианино за пределы экрана
        pianoContainer.GetComponent<RectTransform>().localPosition = offscreenPosition;

        // Перемещаем нотный стан в начальную позицию
        sheetMusicContainer.GetComponent<RectTransform>().localPosition = sheetMusicInitialPosition;
    }
}
