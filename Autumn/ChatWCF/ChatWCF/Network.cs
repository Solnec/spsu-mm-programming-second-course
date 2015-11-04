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

        public string Start(string MyPort, string IPFriend, string PortFriend, string Nick)
        {
            host = new WebServiceHost(typeof(MyService), new Uri("net.tcp://localhost:" + MyPort + "/"));
            host.AddDefaultEndpoints();
            host.Open();

            if ((IPFriend != "") && (PortFriend != ""))
            {
                string AddressFriend = "net.tcp://" + IPFriend + ":" + PortFriend + "/";
                ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), AddressFriend);
                IMyService channel = cf.CreateChannel();
                string MyAddress = channel.DefineAddress(MyPort + "/");
                MyService.Clients = channel.GetAllData();
                ConnectWithAllClients(MyAddress, Nick);
                channel.ConnectNewClient(MyAddress, Nick);
                MyService.Clients.Add(AddressFriend);
                SendMessage(Nick + " в чате!");
            }
            return "Вы в чате!!!";
        }

        public void SendMessage(string Message)
        {
            for (int i = 0; i < MyService.Clients.Count; i++)
            {
                try
                {
                    ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), MyService.Clients[i]);
                    IMyService channel = cf.CreateChannel();
                    channel.AddNewMessage(Message);
                }
                catch (Exception)
                {
                    DeleteFriend(MyService.Clients[i]);
                }
            }
        }

        private void ConnectWithAllClients(string MyAddress, string Nick)
        {
            if (MyService.Clients == null)
                return;

            for (int i = 0; i < MyService.Clients.Count; i++)
            {
                try
                {
                    ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), MyService.Clients[i]);
                    IMyService channel = cf.CreateChannel();
                    channel.ConnectNewClient(MyAddress, Nick);
                }
                catch (Exception)
                {
                    DeleteFriend(MyService.Clients[i]);
                }
            }
        }

        public void DeleteFriend(string Address)
        {
            MyService.Clients.Remove(Address);
        }

        public void Exit(string Nick, string MyPort)
        {
            for (int i = 0; i < MyService.Clients.Count; i++)
            {
                try
                {
                    ChannelFactory<IMyService> cf = new ChannelFactory<IMyService>(new NetTcpBinding(), MyService.Clients[i]);
                    IMyService channel = cf.CreateChannel();
                    channel.AddNewMessage("!" + channel.DefineAddress(MyPort + "/") + "*" + Nick);
                }
                catch (Exception)
                {
                    DeleteFriend(MyService.Clients[i]);
                }
            }
            if (host != null)
                host.Close();
        }
    }
}
