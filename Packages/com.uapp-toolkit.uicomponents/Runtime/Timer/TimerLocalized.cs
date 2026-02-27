using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace UAppToolkit.UIComponents
{
    public class TimerLocalized : TimerBase
    {
        [SerializeField] private LocalizeStringEvent _localizeStringEvent;

        protected override void UpdateText(TimeSpan leftTime)
        {
            var stringRef = _localizeStringEvent.StringReference;
            stringRef.Arguments = new List<object>() { leftTime };
            stringRef.RefreshString();
        }
    }
}