/*
  -----------------------
  UDP-Send
  -----------------------
  // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]  
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
/*
 * simple udp implementation (send/read)
 */
public class udpSend : MonoBehaviour
{
    protected static udpSend Self = null;
    protected udpEventJson jsonData;//temp
    // prefs
    public string szIP = "127.0.0.1";  // define in init
    public int iPort = 8051;  // define in init    
    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;
    // 
    protected string strMessage = "";
    // Use this for initialization
    void Start ()
    {
        if (Self == null) Self = this;
        jsonData = new udpEventJson();
        init();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Instance
    public static udpSend Instance()
    {
        return Self;
    }
    //Creating property
    public string IP
    {
        get //get method for returning value
        {
            return szIP;
        }
        set // set method for storing value
        {
            szIP = value;

        }
    }
    public int Port
    {
        get //get method for returning value
        {
            return iPort;
        }
        set // set method for storing value
        { 
            iPort = value;
        }
    }
    // init
    public void init()
    {
        // ----------------------------
        // 發送
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(szIP), iPort);
        client = new UdpClient();

        // status
        Debug.Log("Sending to " + IP + " : " + Port);
    }   

    // sendData
    private void sendString(string message)
    {
        try
        {
            //用UTF8編碼將數據編碼為二進制格式。
            byte[] data = Encoding.UTF8.GetBytes(message);

            //將消息發送到遠程客戶端。
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
    //
    public void SendStartMessage()
    {
        DateTime currentDate = DateTime.Now;
        String sFileDate = currentDate.ToString("yyyy-MM-dd-HH-mm-ss");
        jsonData.eventType = (int)EventStatus.EVENT_START;
        jsonData.eventData = "Start Event " + sFileDate ;
        string jsonString = JsonUtility.ToJson(jsonData);
        sendString(jsonString);
    }
    public void SendEndMessage()
    {
        DateTime currentDate = DateTime.Now;
        String sFileDate = currentDate.ToString("yyyy-MM-dd-HH-mm-ss");
        jsonData.eventType = (int)EventStatus.EVENT_END;
        jsonData.eventData = "End Event " + sFileDate;
        string jsonString = JsonUtility.ToJson(jsonData);
        sendString(jsonString);
    }
}

/*
 *  simple event date for udp send , json format
 */
enum EventStatus
{
    EVENT_READY = 0, // 準備
    EVENT_START = 1, // 開始
    EVENT_END   = 2  // 結束
}

[System.Serializable]
public class udpEventJson
{
    public int eventType;
    public string eventData;
}

