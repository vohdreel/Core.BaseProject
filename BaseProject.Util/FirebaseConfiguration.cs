using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BaseProject.Util
{
    public static class FirebaseCloudMessageHelper
    {
        private static string ServerKeyFCM = "AAAAf0xA0Uc:APA91bFFNgdHTzNqIV-fUm5ql8ltmXltdZDeSzIG1X6bUF5YY7l0dlQf25MHXaSVee8RlFke85DARXCQdv-MnQjUiZHMuXje57e8e9fbTF1urZGE2s6MCel-tU2dnphxUyhVEskmqx-u";
        private static string SenderIdFCM = "546740162887";

        public static string GetServerKeyFCM()
        {
            return ServerKeyFCM;
        }

        public static string GetSenderIdFCM()
        {
            return SenderIdFCM;
        }

        public static bool PushNotificationAsync(string body, string title, string deviceToken, object data = null)
        {

            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";

                tRequest.Headers.Add(string.Format("Authorization: key={0}", GetServerKeyFCM()));

                tRequest.Headers.Add(string.Format("Sender: id={0}", GetSenderIdFCM()));
                tRequest.ContentType = "application/json";

                object payload = new
                {
                    to = deviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = body != "" ? body : "Você recebeu uma nova notificação", //Body,
                        title = !string.IsNullOrEmpty(title) ? title : "Nova Notificação", //Title,
                        badge = 1,
                        click_action = "FCM_PLUGIN_ACTIVITY"
                    },
                    data
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using WebResponse tResponse = tRequest.GetResponse();
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();

                            }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return true;
            }
        }

    }
}
