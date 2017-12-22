/*
    -----------------------
    UDP-Receive (send to)
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
public class udpReceive : MonoBehaviour
{
    protected static udpReceive Self = null;
    protected bool isActive = false;
    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // public string IP = "127.0.0.1"; default local
    public int iPort = 8051; // define > init

    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!

    // Use this for initialization
    void Start ()
    {
        if (Self == null) Self = this;
        init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Instance
    public static udpReceive Instance()
    {
        return Self;
    }
    public bool Active
    {
        get //get method for returning value
        {
            return isActive;
        }
        set // set method for storing value
        {
            isActive = value;
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

    // OnGUI
    void OnGUI()
    {
        
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Label(new Rect(10, 10, 240, 40), lastReceivedUDPPacket);
    }
    // init
    private void init()
    {
        //定義消息發送的終點。
        Debug.Log("udpReceive.init() port="+ iPort);
        // ----------------------------
        //聽著
        // ----------------------------
        //定義本地端點（接收消息的地方）。
        //創建一個新的線程來接收傳入的消息。
        isActive = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // receive thread
    private void ReceiveData()
    {
        client = new UdpClient(iPort);
        while (isActive)
        {
            try
            {
                //接收字節
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any,0);
                byte[] data = client.Receive(ref anyIP);

                //將UTF8編碼的字節編碼為文本格式。
                string text = Encoding.UTF8.GetString(data);

                //顯示檢索到的文本
                Debug.Log(">>"+text);

                //最新的UDPpacket
                lastReceivedUDPPacket = text;
                allReceivedUDPPackets = allReceivedUDPPackets + text;

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }

    // getLatestUDPPacket
    // cleans up the rest
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }
}