using UnityEngine;
using TMPro;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;


public class SimpleClient : MonoBehaviour
{

    public TMP_Text logText;

    public BoardManager board;


    TcpClient client;
    NetworkStream stream;


    Thread receiveThread;


    Queue<string> messages =
        new Queue<string>();



    void Start()
    {
        Connect();
    }



    void Connect()
    {

        string ip =
            "192.168.1.25";
        // coloque aqui o IP do servidor


        client =
            new TcpClient(
                ip,
                7777);



        stream =
            client.GetStream();



        AddMessage(
            "Conectado ao servidor"
        );


        receiveThread =
            new Thread(Receive);


        receiveThread.Start();

    }





    public void SendMove(int column)
    {

        string msg =
            "PLAY;" + column;



        byte[] data =
            Encoding.UTF8.GetBytes(msg);



        stream.Write(
            data,
            0,
            data.Length
        );

    }





    void Receive()
    {

        byte[] buffer =
            new byte[1024];


        while (true)
        {

            int size =
                stream.Read(
                    buffer,
                    0,
                    buffer.Length);



            string msg =
                Encoding.UTF8.GetString(
                    buffer,
                    0,
                    size);



            AddMessage(
                "Recebido: " + msg
            );



            if (msg.StartsWith("PLAY"))
            {

                string[] data =
                    msg.Split(';');


                int column =
                    int.Parse(data[1]);



                board.ReceiveMove(column);

            }

        }

    }





    void AddMessage(string msg)
    {

        lock (messages)
        {

            messages.Enqueue(msg);

        }

    }





    void Update()
    {

        lock (messages)
        {

            while (messages.Count > 0)
            {

                logText.text +=
                    "\n" +
                    messages.Dequeue();

            }

        }

    }

}
