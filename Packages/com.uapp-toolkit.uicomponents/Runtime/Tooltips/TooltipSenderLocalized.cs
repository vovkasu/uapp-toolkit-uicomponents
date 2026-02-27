using UnityEngine;
using UnityEngine.Localization;

namespace UAppToolkit.UIComponents.Tooltips
{
    public class TooltipSenderLocalized : TooltipSender
    {
        [SerializeField] private LocalizedString _localizedTooltipMessage;

        public void Start()
        {
            this.SetTooltip(() => _localizedTooltipMessage.GetLocalizedString());
        }
    }
}