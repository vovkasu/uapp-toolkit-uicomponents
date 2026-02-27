using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace UAppToolkit.UIComponents.Tooltips
{
    public class TooltipView : TooltipViewBase
    {
        [SerializeField] private LocalizedString _defaultTooltipMessageLocalization;
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning($"{nameof(TooltipView)}: Tooltip message is empty");
                text = _defaultTooltipMessageLocalization.GetLocalizedString();
            }

            _text.text = text;
        }
    }
}