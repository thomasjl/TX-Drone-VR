package com.parrot.sdksample.communication;

import android.os.AsyncTask;
import android.util.Log;
import android.widget.TextView;

import com.parrot.sdksample.drone.MiniDrone;

import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.Socket;
import java.net.UnknownHostException;

public class SocketClient extends AsyncTask<Void, Void, Void> {

    String dstAddress;
    int dstPort;
    String response = "";
    TextView textResponse;
    private MiniDrone mMiniDrone;

    public SocketClient(String addr, int port, MiniDrone dron) {
        dstAddress = addr;
        dstPort = port;
        mMiniDrone = dron;
    }

    @Override
    protected Void doInBackground(Void... arg0) {

        Socket socket = null;

        try {

            //creation socket
            socket = new Socket(dstAddress, dstPort);

            //Send the message to the server
            OutputStream os = socket.getOutputStream();
            OutputStreamWriter osw = new OutputStreamWriter(os);
            BufferedWriter bw = new BufferedWriter(osw);

            String number = "2";

            String sendMessage = number + "message de la part du client\n";
            bw.write(sendMessage);
            bw.flush();
            Log.v("SocketClient","Message sent to the server : "+sendMessage);

            //Get the return message from the server
            InputStream is = socket.getInputStream();
            int bytesToRead;
            byte[] bbb = new byte[2];
            String message;
            while(true)
            {
                is.read(bbb);

                //conversion des ocets en entier
                int typeByte = bbb[0];
                int valueByte = bbb[1];

                int oldValueByte = valueByte;

                // TODO - Voir avec zero ?

                if (valueByte > 0) {

                    if (valueByte < 100)
                    {
                        valueByte = valueByte - 100;
                    }
                    else if (valueByte > 100) {

                        valueByte = valueByte - 100;
                    }
                }
                else if (valueByte < 0) {

                    valueByte = 27 + valueByte +127;
                }
                /*
                else if (valueByte == 0) {
                    valueByte = -100;
                }
                */

                Log.v("SocketClient", "Message de la part du serveur : "+ typeByte +" ( "+Integer.toBinaryString((typeByte+256)%256)  +" )**" +valueByte + "( " + Integer.toBinaryString((oldValueByte+256)%256) +")");


                switch(typeByte)
                {
                    case (0) :
                        //roll
                        mMiniDrone.setFlag((byte) 1);
                        mMiniDrone.setRoll((byte) valueByte);
                        break;

                    case (1) :
                        //pitch

                        mMiniDrone.setFlag((byte) 1);
                        mMiniDrone.setPitch((byte) valueByte);

                        break;

                    case (2) :
                        //yaw

                        mMiniDrone.setYaw((byte) valueByte);
                        break;

                    case (3) :
                        //gaz

                        mMiniDrone.setGaz((byte) valueByte);
                        break;

                    case (4) :
                        //take off

                        mMiniDrone.takeOff();
                        break;

                    case (5) :
                        //landing

                        mMiniDrone.land();
                        break;


                }




                //message = convert(bbb);
                //Log.v("SocketClient","Message de la part du serveur : "+ bbb);

                /*
                if (message.equals("A1")) {
                    mMiniDrone.takeOff();
                }
                else {

                    Log.v("SocketClient", "message " + message +" different de A1");
                }
                */

            }


        } catch (UnknownHostException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            response = "UnknownHostException: " + e.toString();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            response = "IOException: " + e.toString();
        } finally {
            if (socket != null) {
                try {
                    socket.close();
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
            }
        }
        return null;
    }

    String convert(byte[] data) {
        StringBuilder sb = new StringBuilder(data.length);
        for (int i = 0; i < data.length; ++ i) {
            if (data[i] < 0) throw new IllegalArgumentException();
            sb.append((char) data[i]);
        }
        return sb.toString();
    }


    @Override
    protected void onPostExecute(Void result) {
        textResponse.setText(response);
        super.onPostExecute(result);
    }


}