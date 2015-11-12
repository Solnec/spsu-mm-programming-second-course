using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace ChatWCF
{
    class Network
    {
        WebServiceHost host;
        public MyService serv = new MyService();

        public string Start(string MyPort, string IPFriend, string PortFriend, string Nick)
        {
            try
            {
                host = new WebServiceHost(serv, new Uri("net.tcp://localhost:" + MyPort + "/"));
                host.AddDefaultEndpoints();
                host.Open();

                if ((IPFriend != "") && (PortFriend != ""))
                {
                    string AddressFriend = "net.tcp://" + IPFriend + ":" + PortFriend + "/";
                    ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), AddressFriend);
                    IMyService channel = cf.CreateChannel();
                    string MyAddress = channel.DefineAddress(MyPort + "/");
                    serv.Clients = channel.GetAllData();
                    ConnectWithAllClients(MyAddress, Nick);
                    channel.ConnectNewClient(MyAddress, Nick);
                    serv.Clients.Add(AddressFriend);
                    SendMessage(Nick + " в чате!");
                }
            }
            catch (Exception)
            {
                return "!";
            }
            return "Вы в чате!!!";
        }

        public void SendMessage(string Message)
        {
            for (int i = 0; i < serv.Clients.Count; i++)
            {
                bool err = true;
                while (err)
                {
                    try
                    {
                        ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), serv.Clients[i]);
                        IMyService channel = cf.CreateChannel();
                        channel.AddNewMessage(Message);
                        err = false;
                    }
                    catch (Exception)
                    {
                        err = true;
                        DeleteFriend(serv.Clients[i]);
                    } 
                }
            }
        }

        private void ConnectWithAllClients(string MyAddress, string Nick)
        {
            for (int i = 0; i < serv.Clients.Count; i++)
            {
                try
                {
                    ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), serv.Clients[i]);
                    IMyService channel = cf.CreateChannel();
                    channel.ConnectNewClient(MyAddress, Nick);
                }
                catch (Exception)
                {
                    DeleteFriend(serv.Clients[i]);
                }
            }
        }

        public void DeleteFriend(string Address)
        {
            serv.Clients.Remove(Address);
        }

        public void Exit(string Nick, string MyPort)
        {
            for (int i = 0; i < serv.Clients.Count; i++)
            {
                try
                {
                    ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), serv.Clients[i]);
                    IMyService channel = cf.CreateChannel();
                    channel.AddNewMessage("!" + channel.DefineAddress(MyPort + "/"));
                }
                catch (Exception)
                {
                    DeleteFriend(serv.Clients[i]);
                }
            }
            if (host != null)
                host.Close();
        }
    }
}
