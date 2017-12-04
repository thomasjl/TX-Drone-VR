using UnityEngine; 
using System.Threading; 
using System.Net.Sockets; 
using System.IO;
using System;
using System.Net;
using System.Text;

public class SendMessages : MonoBehaviour {

    public GameObject server;
    private TcpClient androidClient;
    private NetworkStream networkStream;
    private int cpt;

    void Start()
    {
        androidClient = null;
        cpt = 0;
    }

    void Update()
    {
        if (androidClient == null && server.GetComponent<MyServerListener>().androidClient != null)
        {
            androidClient = server.GetComponent<MyServerListener>().androidClient;

            networkStream = server.GetComponent<MyServerListener>().ns;

        }
    }


    public void sendMessageToClient (byte[] message)
    {
        if (androidClient != null)
        {
            if (networkStream.CanWrite){
                print("networkstream can write ");

                //byte[] myWriteBuffer = message;
                networkStream.Write(message, 0, 2);


            }
            else{
                print("Sorry.  You cannot write to this NetworkStream.");  
            }

        }
    }
}
