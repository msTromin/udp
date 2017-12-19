using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class udp : MonoBehaviour
{
    protected int iCount = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // OnGUI
    void OnGUI()
    {     
        if (GUI.Button(new Rect(500, 480, 100, 40), "send"))
        {
            sendMessage();
        }
        if (GUI.Button(new Rect(650, 480, 100, 40), "recive"))
        {
            reciveMessage();
        }
    }
    //發送端程式碼如下：
    public void sendMessage()
    {
        IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse("192.168.1.104"), 1688); //可自行定義廣播區域跟Port
        Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //定義發送的格式及有效區域
        Server.EnableBroadcast = true;
        Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

        byte[] pushdata = new byte[1024]; //定義要送出的封包大小


        string s = "" + iCount;
        pushdata = System.Text.Encoding.UTF8.GetBytes(s); //把要送出的資料轉成byte型態
        Server.SendTo(pushdata, remoteIP); //送出的資料跟目的
         
//      System.Threading.Thread.Sleep(1000); //每秒發送一次
        iCount += 1000;    
    }
    //接收端程式碼如下：
    public void reciveMessage()
    {
        IPEndPoint IPEnd = new IPEndPoint(IPAddress.Any, 1688);
        Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Client.Bind(IPEnd);
        EndPoint IP = (EndPoint)IPEnd; //我真的不知道為何一定要這行才能成功= =，誰能解釋一下
        byte[] getdata = new byte[1024]; //要接收的封包大小
        string input;
        int recv;
        while (true)
        {
            recv = Client.ReceiveFrom(getdata, ref IP); //把接收的封包放進getdata且傳回大小存入recv
            input = System.Text.Encoding.UTF8.GetString(getdata, 0, recv); //把接收的byte資料轉回string型態
            if (true == string.IsNullOrEmpty(input)) continue;
            System.Console.WriteLine("received: {0} from: {1}", input, IP.ToString());
            break;
        }
    }
}