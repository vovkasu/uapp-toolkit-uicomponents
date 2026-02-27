using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UAppToolkit.UIComponents.Tooltips
{
    public static class TooltipExtensions
    {
        public static void SetTooltip(this TooltipSender sender, Func<string> msgGenerator)
        {
            sender.SetTooltip(msgGenerator, sender.TooltipEventType);
        }

        public static void SetTooltip(this TooltipSender sender, Func<string> msgGenerator, TooltipEventType tooltipEventType)
        {
            sender.SetTooltip(
                view =>
                {
                    if (view is TooltipView tooltipView)
                        tooltipView.SetText(msgGenerator?.Invoke());
                    else
                        Debug.LogWarning($"{nameof(TooltipSender)}: TooltipView is not of type {nameof(TooltipView)}", sender);
                },
                tooltipEventType
            );
        }

        public static TooltipSender SetTooltip(this UIBehaviour uiBehaviour, Func<string> msgGenerator)
        {
            var tooltipSender = uiBehaviour.gameObject.GetOrAddComponent<TooltipSender>();
            tooltipSender.SetTooltip(msgGenerator);
            return tooltipSender;
        }

        public static TooltipSender SetTooltip(this UIBehaviour uiBehaviour, Action<TooltipViewBase> setDataFunc)
        {
            var tooltipSender = uiBehaviour.gameObject.GetOrAddComponent<TooltipSender>();
            tooltipSender.SetTooltip(setDataFunc);
            return tooltipSender;
        }

        public static TooltipSender SetTooltip(this UIBehaviour uiBehaviour, Func<string> msgGenerator, TooltipEventType type)
        {
            var tooltipSender = uiBehaviour.gameObject.GetOrAddComponent<TooltipSender>();
            tooltipSender.SetTooltip(msgGenerator, type);
            return tooltipSender;
        }

        public static void RemoveTooltip(this UIBehaviour uiBehaviour)
        {
            var tooltipSender = uiBehaviour.gameObject.GetComponent<TooltipSender>();
            tooltipSender?.RemoveTooltip();
        }

        private static T GetOrAddComponent<T>(this GameObject gm) where T : Component
        {
            return gm.GetComponent<T>() ?? gm.AddComponent<T>();
        }
    }
}