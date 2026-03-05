using UnityEngine;
using UnityEngine.Localization;

namespace UAppToolKit.UIComponents.Tooltips
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