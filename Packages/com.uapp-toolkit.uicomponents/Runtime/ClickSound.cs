using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class ClickSound : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        public string SoundName = "UI/button_click";
        public string SoundDisabledName = "UI/button_wrong";

        private static IUISoundPlayer _clickSoundProvider;
        private Selectable _selectable;
        private bool? _selectableInteractableOnPointerDown;

        public static void Init(IUISoundPlayer clickSoundProvider)
        {
            _clickSoundProvider = clickSoundProvider;
        }

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            TrySubscribeLongPressButton(_selectable);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _selectableInteractableOnPointerDown = _selectable?.interactable;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlaySound();
        }

        private void PlaySound()
        {
            string soundName = SoundName;
            if (_selectableInteractableOnPointerDown.HasValue && !_selectableInteractableOnPointerDown.Value)
                soundName = SoundDisabledName;

            if (_clickSoundProvider == null)
            {
                Debug.LogError("ClickSoundProvider is null");
                return;
            }

            _clickSoundProvider.PlayUISound(soundName);
        }

        private void TrySubscribeLongPressButton(Selectable selectable)
        {
            if (selectable is ButtonLongPress longPress)
                longPress.OnLongPressAction.AddListener(PlaySound);
        }
    }
}