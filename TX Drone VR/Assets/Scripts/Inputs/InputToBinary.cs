using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToBinary : MonoBehaviour {

    public GameObject server;

    private float roll;
    private float pitch;
    private float yaw;
    private float gaz; 

    private bool rollToZero;
    private bool pitchToZero;
    private bool yawToZero;
    private bool gazToZero;

    private bool takeOff;
    private bool landing; 

    private bool alreadyTakeOff;
    private bool alreadyLanding;


    void Start ()
    {
        rollToZero = true;
        pitchToZero = true;
        yawToZero = true;
        gazToZero = true;

        takeOff = false;
        landing = false;

        alreadyTakeOff = false;
        alreadyLanding = false;
       
    }


    void Update()
    {
        // Piloting
        roll = Input.GetAxis("Axis 1");
        if (roll != 0)
        {
            rollToZero = false;
            sendAnglesToClient(1, roll);                                  
        }
        else
        {
            if (!rollToZero)
            {
                sendAnglesToClient(1, 0);     
                rollToZero = true;
            }
        }

        pitch = Input.GetAxis("Axis 2");
        if (pitch != 0)
        {
            pitchToZero = false;
            sendAnglesToClient(2 , pitch);     
        }
        else
        {
            if (!pitchToZero)
            {
                sendAnglesToClient(2, 0);     
                pitchToZero = true;
            }
        }

        yaw = Input.GetAxis("Axis 4");
        if (yaw != 0)
        {
            yawToZero = false;
            sendAnglesToClient(3 , yaw);  
        }
        else
        {
            if (!yawToZero)
            {
                sendAnglesToClient(3, 0);     
                yawToZero = true;
            }
        }

        gaz = Input.GetAxis("Axis 5");
        if (gaz != 0)
        {
            gazToZero = false;
            sendAnglesToClient(4 , gaz);  
        }
        else
        {
            if (!gazToZero)
            {
                sendAnglesToClient(4, 0);     
                gazToZero = true;
            }
        }


        // take off
        takeOff = Input.GetKey(KeyCode.JoystickButton7);
        if (takeOff && !alreadyTakeOff)
        {            
            sendButtonToClient(5);   
            alreadyTakeOff = true;
            alreadyLanding = false;
        }
        /*
        else
        {
            if (!takeOffToFalse)
            {
                takeOffToFalse = true;
                //sendButtonToClient(5, false);   
            }
        }
        */


        //landing
        landing = Input.GetKey(KeyCode.JoystickButton6);
        if (landing && !alreadyLanding)
        {            
            sendButtonToClient(6);   
            alreadyLanding = true;
            alreadyTakeOff = false;
        }
        /*
        else
        {
            if (!landingToFalse)
            {
                landingToFalse = true;
                //sendButtonToClient(6, false);  
            }
        }
        */

               


    }

    byte convertFloatInBinary(float number)
    {
        //number, nombre à virgule allant de -1 à 1. On multiplie par 100 pour avoir un entier
        int intNumber = (int)(number * 100);


        //si le nombre est positif, on lui rajoute 100

        if (intNumber > 0)
        {
            intNumber += 100;
        }
        else if (intNumber < 0)
        {
            // si le nombre est negatif, on fait la difference avec 100
            intNumber = 100 + intNumber +1; //+1 pour ne pas avoir 0 quand on a le max negatif
        }


        byte intInByte = Convert.ToByte(intNumber);

        //int[] intArray = new BitArray(new[] { intNumber });

        //int[] intArray = new int[8];

        //bitArray.CopyTo(intArray, 8);

        Debug.Log("number (" + number + "), intNumber (" + intNumber + "), binary (" + intInByte +")");

        return intInByte;
    }



    void sendAnglesToClient(int type, float _roll)
    {
        byte typeByte=0;

        switch (type)
        {
            case (1):
                typeByte = 0;
                break;
            case (2):
                typeByte = 1;
                break;
            case (3):
                typeByte = 2;
                break;
            case(4):
                typeByte = 3;
                break;        
        }

        byte valueByte = convertFloatInBinary(_roll);

        byte[] bitArray = new byte[] {typeByte, valueByte };

        //Array.Copy(typeArray, 0, bitArray, 0, 1);
        //Array.Copy(valueArray, 0, bitArray, 1, 1);    

        Debug.Log("message : " + Convert.ToString(bitArray[0],2) + " ** " + Convert.ToString(bitArray[1],2));

        server.GetComponent<SendMessages>().sendMessageToClient(bitArray);
    }

    void sendButtonToClient(int type)
    {
        byte typeByte=0;

        switch (type)
        {
            case(5):
                typeByte = 4;
                break;
            case(6):
                typeByte = 5;
                break;
        }

        byte valueByte = 1;

        byte[] bitArray = new byte[] {typeByte, valueByte };

        server.GetComponent<SendMessages>().sendMessageToClient(bitArray);
    }



}
