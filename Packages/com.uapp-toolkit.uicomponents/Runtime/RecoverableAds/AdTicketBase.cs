using System;
using UnityEngine;

namespace UAppToolkit.UIComponents.RecoverableAds
{
    public abstract class AdTicketBase : ScriptableObject
    {
        public abstract int Count { get; set; }
        public abstract int MaxCount { get; }
        public abstract TimeSpan WaitTime { get; }
        public abstract bool ShowRewardedAd(string placementName, Action rewardedAdReceived);
        public abstract void AdButtonUpdated(MonoBehaviour adButton, string placementName, bool isActive);
    }
}