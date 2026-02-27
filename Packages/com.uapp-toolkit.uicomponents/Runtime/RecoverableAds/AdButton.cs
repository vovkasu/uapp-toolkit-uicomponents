using System;
using UAppToolkit.UIComponents.Tooltips;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents.RecoverableAds
{
    public class AdButton : MonoBehaviour
    {
        [SerializeField] private AdTicketBase _adTicket;
        
        [SerializeField] private Button _button;
        [SerializeField] private LocalizeStringEvent _buttonTextLocalized;
        [SerializeField] private LocalizeStringEvent _buttonTextSkipAdLocalized;
        [SerializeField] private TimerLocalized _timer;
        [SerializeField] private string _placementName;

        [Header("Tooltip")]
        [SerializeField] private TooltipSender _adsTooltipSender;
        [SerializeField] private LocalizedString _noAdsTooltipMessageLocalization;

        private bool _canSkipAd;
        public UnityEvent OnRewardedAdReceived;

        public Button Button => _button;

        public AdTicketBase AdTicket
        {
            get => _adTicket;
            set => _adTicket = value;
        }

        public string PlacementName
        {
            get => _placementName;
            set => _placementName = value;
        }

        private void Awake()
        {
            _button.interactable = false;
        }

        private void OnEnable()
        {
            _button.onClick.RemoveListener(TryBuyAdPack);
            _button.onClick.AddListener(TryBuyAdPack);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(TryBuyAdPack);
            _adTicket.AdButtonUpdated(this, PlacementName, false);
        }

        private void OnDestroy()
        {
            _adTicket.AdButtonUpdated(this, PlacementName, false);
        }

        public void SetLocalizedStringArgument(string argName, int value)
        {
            SetArgument(_buttonTextLocalized.StringReference, argName, value);
            SetArgument(_buttonTextSkipAdLocalized.StringReference, argName, value);
        }

        public void SetCanSkipAd(bool canSkipAd)
        {
            _canSkipAd = canSkipAd;
            UpdateAdButton();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateAdButton();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _adTicket.AdButtonUpdated(this, PlacementName, false);
        }

        public void UpdateAdButton()
        {
            if (_adTicket == null) 
                return;

            var ticketCount = _adTicket.Count;
            var canShowAd = ticketCount > 0;

            _buttonTextLocalized.gameObject.SetActive(canShowAd && !_canSkipAd);
            _buttonTextSkipAdLocalized.gameObject.SetActive(canShowAd && _canSkipAd);

            _button.interactable = canShowAd;
            if (canShowAd)
            {
                var localizedStringEvent = _canSkipAd ? _buttonTextSkipAdLocalized : _buttonTextLocalized;
                UpdateLocalizedStringArgs(localizedStringEvent, ticketCount);
                _timer.StopTimer();
                _timer.Hide();
                _adTicket.AdButtonUpdated(this, PlacementName, true);
            }
            else
            {
                _timer.Show();
                var waitTime = _adTicket.WaitTime;
                var updatePeriod = GetUpdatePeriod(waitTime);

                if (gameObject.activeInHierarchy)
                    _timer.StartTimer(waitTime, updatePeriod);

                _adTicket.AdButtonUpdated(this, PlacementName, false);
            }

            _adsTooltipSender.SetTooltip(() => _noAdsTooltipMessageLocalization.GetLocalizedString());
        }

        private void UpdateLocalizedStringArgs(LocalizeStringEvent stringEvent, int ticketCount)
        {
            var localizedString = stringEvent.StringReference;
            SetArgument(localizedString, "adCount", ticketCount);
            SetArgument(localizedString, "adMaxCount", _adTicket.MaxCount);
            localizedString.RefreshString();
        }

        private void TryBuyAdPack()
        {
            if (_adTicket.Count <= 0)
                return;

            if (!_adTicket.ShowRewardedAd(_placementName, RewardedAdReceived))
            {
                _adsTooltipSender.ShowTooltip();
            }
        }

        private void RewardedAdReceived()
        {
            OnRewardedAdReceived.Invoke();
            _adTicket.Count--;

            if (gameObject.activeInHierarchy)
                UpdateAdButton();
        }

        private float GetUpdatePeriod(TimeSpan time) => time switch
        {
            { TotalDays: > 1 } => 120,
            { TotalHours: > 1 } => 60f,
            _ => 1f
        };

        private void SetArgument(LocalizedString localizedString, string argName, int value)
        {
            localizedString.Add(argName, new IntVariable { Value = value });
        }
    }
}