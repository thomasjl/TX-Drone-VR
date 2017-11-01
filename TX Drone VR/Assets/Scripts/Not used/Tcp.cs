using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class Tcp : MonoBehaviour {
    
    //read external data
    public string serverIP = "";
    public System.Int32 serverPort;

    TcpClient tcpClient;
    NetworkStream theStream;

    //static int bufferSize = 512;
    byte[] data = new byte[1024];
    string receiveMsg = "";

    bool ipconfiged = false;
    bool conReady = false;

    // Use this for initialization
    public void connectToServer ()
    {        
        readTCPInfo(); 
    }

    void readTCPInfo()
    {       
        

        //serverIP = "192.168.1.25";
        //serverPort = System.Int32.Parse("52432");

        ipconfiged = true;

        Debug.Log("server ip: " + serverIP + "    server port: " + serverPort);

        setupTCP();
    }

    public void setupTCP()
    {
        try
        {
            if(ipconfiged)
            {
                tcpClient = new TcpClient(serverIP, serverPort);
                //tcpClient.ReceiveTimeout = 5000;
                //tcpClient.SendTimeout = 5000;
                theStream = tcpClient.GetStream();

                Debug.Log("Successfully created TCP client and open the NetworkStream.");

                conReady = true;

                InvokeRepeating("receiveData", 0.001f, 0.5f);
            }
        }
        catch(Exception e)
        {
            Debug.Log("Unable to connect...");
            Debug.Log("Reason: " + e);
        }
    }


    public void receiveData()
    {
        if(!conReady)
        {
            Debug.Log("connection not ready...");
            return;
        }

        int numberOfBytesRead = 0;

        if(theStream.CanRead)
        {
            try
            {
                //data available always false?
                //Debug.Log("data availability:  " + theStream.DataAvailable);
                numberOfBytesRead = theStream.Read(data, 0, data.Length);  
                receiveMsg = System.Text.Encoding.ASCII.GetString(data, 0, numberOfBytesRead);

                Debug.Log("receive msg:  " + receiveMsg);
            }
            catch(Exception e)
            {
                Debug.Log("Error in NetworkStream: " + e);
            }
        }

        receiveMsg = "";
    }

    public void maintainConnection()
    {
        if(!theStream.CanRead)
        {
            setupTCP();
        }
    }
    public void closeConnection()
    {
        if(!conReady) return;

        theStream.Close();
        conReady = false;
    }




}