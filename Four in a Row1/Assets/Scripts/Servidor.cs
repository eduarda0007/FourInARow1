using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class SimpleServer : MonoBehaviour
{
    public TMP_Text logText;

    TcpListener server;
    TcpClient client;
    NetworkStream stream;

    Thread serverThread;

    Queue<string> messages =
        new Queue<string>();


    public BoardManager board;


    void Start()
    {
        AddMessage("Servidor iniciado");

        serverThread =
            new Thread(Listen);

        serverThread.Start();
    }


    void Listen()
    {
        server =
            new TcpListener(
                IPAddress.Any,
                7777);

        server.Start();


        AddMessage(
            "Esperando cliente..."
        );


        client =
            server.AcceptTcpClient();


        stream =
            client.GetStream();


        AddMessage(
            "Cliente conectado"
        );


        byte[] buffer =
            new byte[1024];


        while (true)
        {
            int size =
                stream.Read(
                    buffer,
                    0,
                    buffer.Length
                );


            string msg =
                Encoding.UTF8.GetString(
                    buffer,
                    0,
                    size
                );


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


                Send(msg);
            }
        }
    }



    public void Send(string msg)
    {
        if (stream == null)
            return;


        byte[] data =
            Encoding.UTF8.GetBytes(msg);


        stream.Write(
            data,
            0,
            data.Length
        );
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