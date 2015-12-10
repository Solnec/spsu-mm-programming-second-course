using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace P2PChatWinForms
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string content { get; set; }
        [DataMember]
        public string from { get; set; }
        [DataMember]
        public string to { get; set; }
        [DataMember]
        public int time { get; set; }

        public Message(Message old)
        {
            this.content = old.content;
            this.from = old.from;
            this.to = old.to;
            this.time = old.time;
        }

        public Message(string newContent, string from, string to, int time)
        {
            this.content = newContent;
            this.from = from;
            this.to = to;
            this.time = time;
        }

        public void Send(List<PeerEntry> Peers)
        {
            foreach (PeerEntry peerEntry in Peers)
            {
                if (peerEntry != null && peerEntry.ServiceProxy != null)
                {
                    peerEntry.ServiceProxy.Push(this);
                }
            }
        }

        public override string ToString()
        {
            return from +
                ((to != "") ? "( for " + to + ") " : "")
                + ": " + content + "\n";
        }

    }
}
