using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Palmipedo.iOS.Core.Entities;
using UIKit;
using UserNotifications;

namespace Palmipedo.iOS.Core
{
    public class NotificationManager
    {
        private static object _lock = new object();

        public event EventHandler<bool> OnAuthorizationChanged = delegate { };

        private Dictionary<string, NotificationRequest> _dict;

        private static NotificationManager instance;
        public static NotificationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotificationManager();
                }
                return instance;
            }
        }

        public event NotificationManagerOnErrorEventHandler OnError;
        public event NotificationManagerOnSentEventHandler OnSent;

        private NotificationManager()
        {
            _dict = new Dictionary<string, NotificationRequest>();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request Permissions
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
                {
                    OnAuthorizationChanged?.Invoke(new object(), granted);
                });
            }

            // Watch for notifications while the app is active
            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            //else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            //{
            //    var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
            //    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);

            //    app.RegisterUserNotificationSettings(notificationSettings);
            //}
        }

        private static void Check()
        {
            // Get current notification settings
            UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
            {
                var alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);
            });
        }

        public NotificationRequest SendLocalNotification(Notification notification)
        {
            NotificationRequest notificationRequest = new NotificationRequest(notification);

            lock (_lock)
            {
                UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
                _dict.Add(notificationRequest.Id, notificationRequest);
            }

            UNUserNotificationCenter.Current.AddNotificationRequest(notificationRequest.GetRequest(), (err) =>
            {
                notificationRequest.TriggeredDate = DateTime.Now;

                if (err != null)
                {
                    if (OnError != null)
                    {
                        NotificationManagerOnErrorEventArgs ev = new NotificationManagerOnErrorEventArgs();
                        ev.NotificationRequest = notificationRequest;
                        ev.Error = err.ToString();
                        OnError.Invoke(this, ev);
                    }
                }
                else
                {
                    if (OnSent != null)
                    {
                        NotificationManagerOnSentEventArgs ev = new NotificationManagerOnSentEventArgs();
                        ev.NotificationRequest = notificationRequest;
                        OnSent.Invoke(this, ev);
                    }
                }
            });

            return notificationRequest;
        }

        public void RemoveNotificationRequest(string id)
        {
            lock (_lock)
            {
                if (_dict.ContainsKey(id))
                {
                    _dict.Remove(id);
                }
            }
        }
    }

    public class NotificationManagerOnErrorEventArgs : EventArgs
    {
        public NotificationRequest NotificationRequest { get; set; }
        public string Error { get; set; }
    }

    public delegate void NotificationManagerOnErrorEventHandler(object sender, NotificationManagerOnErrorEventArgs ev);

    public class NotificationManagerOnSentEventArgs : EventArgs
    {
        public NotificationRequest NotificationRequest { get; set; }
    }

    public delegate void NotificationManagerOnSentEventHandler(object sender, NotificationManagerOnSentEventArgs ev);

    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        #region Constructors
        public UserNotificationCenterDelegate()
        {
        }
        #endregion

        #region Override Methods
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
        #endregion
    }
}