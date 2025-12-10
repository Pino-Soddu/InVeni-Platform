using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using UserNotifications;

namespace Palmipedo.iOS.Core.Entities
{
    public class NotificationRequest
    {
        public string Id { get; private set; }
        public DateTime CreationDate { get; set; }
        public DateTime TriggeredDate { get; set; }
        public Notification Notification { get; set; }
        public UNNotificationTrigger Trigger { get; set; }
        private UNMutableNotificationContent NotificationContent { get; set; }

        public NotificationRequest(Notification notification)
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            Notification = notification;

            NotificationContent = new UNMutableNotificationContent();

            if (!string.IsNullOrEmpty(notification.Title))
                NotificationContent.Title = Notification.Title;

            if (!string.IsNullOrEmpty(notification.Subtitle))
                NotificationContent.Subtitle = Notification.Subtitle;

            if (!string.IsNullOrEmpty(notification.Body))
                NotificationContent.Body = Notification.Body;

            if (Notification.Badge > 0)
                NotificationContent.Badge = Notification.Badge;

            Trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

            CreationDate = DateTime.Now;
        }

        public UNNotificationRequest GetRequest()
        {
            return UNNotificationRequest.FromIdentifier(Id, NotificationContent, Trigger);
        }
    }
}