using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;


namespace Push_Notification_Center
{
    public partial class PushCenter : Form
    {
        string ToastPushXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<wp:Notification xmlns:wp=\"WPNotification\">" + "<wp:Toast>" + "<wp:Text1>{0}</wp:Text1>" + "<wp:Text2>{1}</wp:Text2>" + "</wp:Toast>" + "</wp:Notification>";

        public PushCenter()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtURL.Text == string.Empty)
            {
                MessageBox.Show("Please enter a url");
                return;
            }

            if (txtTitle.Text == string.Empty || txtText.Text == string.Empty)
            {
                MessageBox.Show("Please enter text and title to send");
                return;
            }

            try
            {
                string url = txtURL.Text;
                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(url);
                sendNotificationRequest.Method = "POST";
                sendNotificationRequest.Headers = new WebHeaderCollection();
                sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "toast");
                sendNotificationRequest.Headers.Add("X-NotificationClass", "2");
                string str = string.Format(ToastPushXML, txtTitle.Text, txtText.Text);
                byte[] strBytes = new UTF8Encoding().GetBytes(str);
                sendNotificationRequest.ContentLength = strBytes.Length;
                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(strBytes, 0, strBytes.Length);
                }
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];
                if (notificationStatus == "" && deviceConnectionStatus == "")
                    notificationStatus = deviceConnectionStatus = "ERROR";
                lblStatus.Text = "Status: " + notificationStatus + " : " + deviceConnectionStatus;
            }
            catch(Exception ex)
            {
                lblStatus.Text = "Status: " + ex.Message;
            }

        }

    }
}
