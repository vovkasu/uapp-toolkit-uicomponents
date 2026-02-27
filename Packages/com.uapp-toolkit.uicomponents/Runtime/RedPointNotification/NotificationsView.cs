using System.Collections.Generic;
using UnityEngine;

namespace UAppToolkit.UIComponents.RedPointNotification
{
    public class NotificationsView : NotificationViewBase
    {
        [SerializeField] private List<NotificationViewBase> _views;

        public override bool IsVisible
        {
            get
            {
                foreach (var view in _views)
                {
                    if (view.gameObject.activeSelf && view.IsVisible)
                        return true;
                }

                return false;
            }
        }

        public override void Init()
        {
            base.Init();
            foreach (var view in _views)
            {
                view.Init();
            }
        }

        protected override void SubscribeChangeState()
        {
            foreach (var view in _views)
            {
                view.OnChangedState -= OnChangedModelState;
                view.OnChangedState += OnChangedModelState;
            }
        }

        protected override void UnsubscribeChangeState()
        {
            foreach (var view in _views)
            {
                view.OnChangedState -= OnChangedModelState;
            }
        }

        protected override void UpdateView()
        {
            NotificationViewBase newActive = null;
            foreach (var view in _views)
            {
                if (view.IsVisible)
                {
                    newActive = view;
                    break;
                }
            }

            foreach (var view in _views)
            {
                bool shouldBeActive = view == newActive;
                if (view.gameObject.activeSelf != shouldBeActive)
                    view.gameObject.SetActive(shouldBeActive);
            }
        }
    }
}