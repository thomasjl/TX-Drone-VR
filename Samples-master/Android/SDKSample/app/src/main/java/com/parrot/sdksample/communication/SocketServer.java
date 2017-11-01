package com.parrot.sdksample.communication;

import android.os.AsyncTask;
import android.util.Log;

import com.parrot.sdksample.drone.MiniDrone;

import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.ServerSocket;
import java.net.Socket;

/**
 * Created by thomas on 23/10/2017.
 */

public class SocketServer extends AsyncTask<Void, Void, Void> {

    int dstPort;
    private MiniDrone mMiniDrone;

    public SocketServer(int port, MiniDrone dron) {

        dstPort = port;
        mMiniDrone = dron;
    }

    @Override
    protected Void doInBackground(Void... arg0) {

        Socket clientSocket = null;

        try {
            ServerSocket serverSocket = new ServerSocket(dstPort);
            Log.v("SocketServer", "creation de la socket cient, ecoute port " + dstPort);
            clientSocket = serverSocket.accept();//attend qu'au moins un client se connecte

            Log.v("SocketServer", "client connecte !");
            /*
            PrintWriter out =  new PrintWriter(clientSocket.getOutputStream(), true);
            BufferedReader bw = new BufferedReader(  new InputStreamReader(clientSocket.getInputStream()));
            */

            OutputStream os = clientSocket.getOutputStream();
            OutputStreamWriter osw = new OutputStreamWriter(os);
            BufferedWriter bw = new BufferedWriter(osw);

            String sendMessage = "message de la part du serveur\n";
            bw.write(sendMessage);
            bw.flush();
            Log.v("SocketServer","Message sent to the client : "+sendMessage);

            InputStream is = clientSocket.getInputStream();
            int bytesToRead;
            byte[] bbb = new byte[2];
            String message;
            while(true)
            {
                is.read(bbb);
                message = convert(bbb);
                Log.v("SocketClient","Message de la part du client : "+ message);

                /*
                if (message.equals("A1")) {
                    mMiniDrone.takeOff();
                }
                else {

                    Log.v("SocketClient", "message " + message +" different de A1");
                }
                */

            }


        } catch (IOException e) {
            e.printStackTrace();
        }



            /*
                String inputLine, outputLine;

                // Initiate conversation with client
                KnockKnockProtocol kkp = new KnockKnockProtocol();
                outputLine = kkp.processInput(null);
                out.println(outputLine);

                while ((inputLine = in.readLine()) != null) {
                    outputLine = kkp.processInput(inputLine);
                    out.println(outputLine);
                    if (outputLine.equals("Bye."))
                        break;
                }
            }
            */


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

}
