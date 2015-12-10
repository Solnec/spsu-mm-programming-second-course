using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
//using System.Windows.Controls;
using System.Threading;

namespace P2PChatWinForms
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class P2PService : IP2PService
    {
        private List<PeerEntry> Peers = new List<PeerEntry>();
        public PeerEntry Me;
        public AutoResetEvent Caught = new AutoResetEvent(false);
        public Queue<Message> MessageList = new Queue<Message>();


        public P2PService(PeerEntry Me)
        {
            this.Me = Me;
        }

        public void AddClient(PeerEntry client)
        {
            Peers.Add(client);
        }

        public void RemoveClient(PeerEntry client)
        {
            Peers.Remove(client);
        }

        public List<PeerEntry> GetClients()
        {
            return Peers;
        }

        public void Push(Message Msg)
        {
            MessageList.Enqueue(Msg);
            Caught.Set();
        }
    }
}