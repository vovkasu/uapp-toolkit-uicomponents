using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UAppToolkit.UIComponents.RedPointNotification
{
    [CreateAssetMenu(menuName = "UAppToolkit/UIComponents/CompositeNotificationModel", fileName = "CompositeNotificationModel", order = 2603)]
    public class CompositeNotificationModel : RedPointNotificationModelBase
    {
        public List<RedPointNotificationModelBase> NotificationModels;

        protected override bool IsVisibleInternal => NotificationModels.Any(m => m.IsVisible);

        protected override void InitInternal()
        {
            foreach (var notificationModel in NotificationModels)
            {
                notificationModel.OnChangedState -= StateChanged;
                notificationModel.OnChangedState += StateChanged;
            }
        }
    }
}