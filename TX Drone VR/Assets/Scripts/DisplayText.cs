using UnityEngine; 
using System.Threading; 
using System.Net.Sockets; 
using System.IO;
using System;
using System.Net;
using System.Text;


public class DisplayText : MonoBehaviour {

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


    public void SendText()
    {
        if (androidClient != null)
        {
            int i;
            //Byte[] bytes = new Byte[256];

            if (networkStream.CanWrite){
                print("networkstream can write ");

                byte[] myWriteBuffer = Encoding.ASCII.GetBytes("A1");
                networkStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

                /*
                while((i = networkStream.Read(bytes, 0, bytes.Length))!=0) 
                {   
                    // Translate data bytes to a ASCII string.
                    String data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    print("Received: " + data);

                    String message = "message numero " + cpt + "provenant du serveur"; 
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);

                    cpt++;

                    // Send back a response.
                    networkStream.Write(msg, 0, msg.Length);
                    print("Sent: " +  message);        

                }
                */
            }
            else{
                print("Sorry.  You cannot write to this NetworkStream.");  
            }




            /////////////
              
        }
       
    }
}
