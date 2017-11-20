using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCityParkServices.Utils
{

    public class AzureHubUtils
    {
        private static NotificationHubClient hub;
     

        public static async void SendNotificationAsync(string message, System.Collections.Generic.IEnumerable<string> tags)
        {
            hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://cityparkhub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=YbiuJRmp5LmeYq+ayAXBupwcimqHPtL8JzXcScEh50c=", "CityPark");
            var notif = ("{\"data\":{\"message\":\"" + message + "\"}}");
            await hub.SendGcmNativeNotificationAsync(notif, tags);
        }
    }
}