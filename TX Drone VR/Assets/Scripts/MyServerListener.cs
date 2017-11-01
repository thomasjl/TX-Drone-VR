using UnityEngine; 
using System.Threading; 
using System.Net.Sockets; 
using System.IO;
using System;
using System.Net;
using System.Text;


public class MyServerListener : MonoBehaviour { 

    //public string serverIP = "";
    //public System.Int32 serverPort;

    private bool mRunning;
    string msg = "";
    Thread mThread;
    TcpListener tcp_Listener = null;
    public TcpClient androidClient;
    public NetworkStream ns;
   
    private Socket socket;

    void Start()
    {       
        androidClient = null;
        mRunning = true;
        ThreadStart ts = new ThreadStart(SayHello);
        mThread = new Thread(ts);
        mThread.Start();
        print("Thread done...");
    }
    public void stopListening()
    {
        mRunning = false;
    }


    void SayHello()
    {


        try
        {
            tcp_Listener = new TcpListener(52432);
            tcp_Listener.Start();
            print("Server Start");

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            while (mRunning)
            {               

                // check if new connections are pending, if not, be nice and sleep 100ms
                    
                if (!tcp_Listener.Pending())
                {
                    Thread.Sleep(100);
                }
                else
                {
                    TcpClient client = tcp_Listener.AcceptTcpClient();
                   
                    androidClient = client;
                    print("Connected!");

                    data=null;

                    androidClient = client;

                     //Get a stream object for reading and writing
                    ns = client.GetStream();


                    int i;
                    // Loop to receive all the data sent by the client.

                    while((i = ns.Read(bytes, 0, bytes.Length))!=0) 
                    {   
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        print("Received: " + data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        ns.Write(msg, 0, msg.Length);
                        print("Sent: " +  data);            
                    }
                }
                                         

            }
        }
        catch (ThreadAbortException)
        {
            print("exception");
        }
        finally
        {
            // Stop listening for new clients.
            mRunning = false;
            print("Stop listening for new clients. The Stop method does not close any accepted connections. You are responsible for closing these separately.");
            //tcp_Listener.Stop();
        }



    }
    void OnApplicationQuit()
    {
        // stop listening thread
        stopListening();
        // wait fpr listening thread to terminate (max. 500ms)
        mThread.Join(500);
    }



} 