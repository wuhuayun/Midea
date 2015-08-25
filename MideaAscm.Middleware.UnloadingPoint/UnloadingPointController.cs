using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//添加用于Socket的类
using System.Net;
using System.Net.Sockets;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Server
{
    public class UnloadingPointController
    {
        public List<AscmUnloadingPoint> listAscmUnloadingPoint = new List<AscmUnloadingPoint>();
        public bool canUser = false;
        public AscmUnloadingPointController ascmUnloadingPointController = new AscmUnloadingPointController();

        public bool Connected = true;

        public int[] ReadStatus()
        {
            int[] status = new int[8];
            try
            {
                IPAddress ip = IPAddress.Parse(ascmUnloadingPointController.ip);
                IPEndPoint ipe = new IPEndPoint(ip, ascmUnloadingPointController.port);//把ip和端口转化IPEndPoint实例
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
                socket.SendTimeout = 5000;
                socket.ReceiveTimeout = 5000;
                socket.Connect(ipe);//连接服务器
                //modulebus协议读取DIO状态命令
                byte[] sendBytes = new byte[] { 0x01, 0x02, 0x00, 0x00, 0x00, 0x06, 0x01, 0x01, 0x00, 0x00, 0x00, 0x08 };
                socket.Send(sendBytes, sendBytes.Length, 0);//发送信息
                byte[] recvBytes = new byte[1024];
                int bytes = socket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
                if (bytes <= 0)
                {
                    //break;
                }
                //协议中卸货点状态对应字节位，有车为0，无车为1
                byte statusByte = recvBytes[9];

                for (int i = 6; i >= 0; i--)
                {
                    status[i] = statusByte / (int)Math.Pow(2, i) % 2;
                    //对车位状态对应位取反后读取相应车位状态
                    status[i] = status[i] == 1 ? 0 : 1;
                }
                socket.Close();
            }
            catch (SocketException e)
            {
                this.Connected = false;
            }
            catch (Exception e)
            {
                throw e;
            }
            return status;
        }
    }
}
