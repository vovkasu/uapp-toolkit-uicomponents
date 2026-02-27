using UnityEngine.Events;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class ButtonLongPressToggle : ButtonLongPress
    {
        public UnityEvent<bool> OnToggleValueChanged;

        public SpriteState ToggledSpriteState;
        public ColorBlock ToggledColorBlock;
        public AnimationTriggers ToggledAnimationTriggers;

        private ColorBlock _defaultColorBlock;
        private SpriteState _defaultSpriteState;
        private AnimationTriggers _defaultAnimationTriggers;

        private bool _isToggledOn;
        public bool IsToggleOn
        {
            get => _isToggledOn;
            set
            {
                _isToggledOn = value;
                UpdateSprite();
                OnToggleValueChanged?.Invoke(_isToggledOn);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();

            OnLongPressAction.AddListener(OnLongPress);
            onClick.AddListener(OnClick);

            _defaultColorBlock = colors;
            _defaultSpriteState = spriteState;
            _defaultAnimationTriggers = animationTriggers;
        }

        public void SetToggledOnSilently(bool value)
        {
            _isToggledOn = value;
            UpdateSprite();
        }

        private void OnLongPress()
        {
            IsToggleOn = !IsToggleOn;
        }

        private void OnClick()
        {
            if (IsToggleOn)
            {
                IsToggleOn = false;
            }
        }

        private void UpdateSprite()
        {
            switch (transition)
            {
                case Transition.ColorTint:
                    colors = IsToggleOn ? ToggledColorBlock : _defaultColorBlock;
                    break;
                case Transition.SpriteSwap:
                    spriteState = IsToggleOn ? ToggledSpriteState : _defaultSpriteState;
                    break;

                case Transition.Animation:
                    animationTriggers = IsToggleOn ? ToggledAnimationTriggers : _defaultAnimationTriggers;
                    break;
            }
        }
    }
}