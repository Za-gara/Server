﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();

        static void FlushRoom()
        {
            Room.Push(() => Room.Flush());
            JobTimer.Instance.Push(FlushRoom, 250);
        }

        static void Main(string[] args)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress Adrs = host.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(Adrs, 7777);

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generator(); });

            FlushRoom();
            // JobTimer.Instance.Push(FlushRoom);
            while (true) //(임시) 프로그램 종료 방지
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}
