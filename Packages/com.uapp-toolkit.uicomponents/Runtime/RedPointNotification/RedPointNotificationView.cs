using UnityEngine;

namespace UAppToolkit.UIComponents.RedPointNotification
{
    public class RedPointNotificationView : NotificationViewBase
    {
        [SerializeField] private GameObject View;
        [SerializeField] private RedPointNotificationModelBase NotificationModel;

        public override bool IsVisible => NotificationModel != null && NotificationModel.IsVisible;
        public RedPointNotificationModelBase Model => NotificationModel;

        public void Init()
        {
            if (!NotificationModel)
                Debug.LogError("NotificationModel==null", this);

            base.Init();
        }

        public void Init(RedPointNotificationModelBase model)
        {
            Deinit();
            NotificationModel = model;
            InitInternal();
        }

        protected override void SubscribeChangeState()
        {
            if (NotificationModel)
            {
                NotificationModel.OnChangedState -= OnChangedModelState;
                NotificationModel.OnChangedState += OnChangedModelState;
            }
        }
        
        protected override void UnsubscribeChangeState()
        {
            if (NotificationModel)
                NotificationModel.OnChangedState -= OnChangedModelState;
        }

        protected override void UpdateView()
        {
            View.SetActive(IsVisible);
        }
    }
}