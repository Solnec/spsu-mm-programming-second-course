using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace P2PChatWinForms
{
    public class Network
    {

        public bool Connect(PeerEntry Peer)
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                ChannelFactory<IP2PService> cf = new ChannelFactory<IP2PService>(binding, Peer.stringUri);
                Peer.ServiceProxy = cf.CreateChannel();

                return true;
            }
            catch (EndpointNotFoundException)
            {
                return false;
            }
        }

        public bool Disconnect(PeerEntry Peer)
        {
            Peer.ServiceProxy.RemoveClient(Peer);
            ((IClientChannel)Peer.ServiceProxy).Close();
            return true;
        }

        public bool Send(PeerEntry target, Message m)
        {
            try
            {
                target.ServiceProxy.Push(new Message(m));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
