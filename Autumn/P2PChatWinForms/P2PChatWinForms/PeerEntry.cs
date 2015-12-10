using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.PeerToPeer;
using System.Runtime.Serialization;

namespace P2PChatWinForms
{
    [DataContract]
    public class PeerEntry
    {
        [DataMember]
        public PeerName PeerName { get; set; }
        [DataMember]
        public IP2PService ServiceProxy { get; set; }
        [DataMember]
        public string Nick { get; set; }
        [DataMember]
        public string stringUri;
        [DataMember]
        public bool IsConnected { get; set; }
        [DataMember]
        public Uri Uri;

        public Uri GetUri()
        {
            return Uri;
        }

        public PeerEntry(string ip, string port)
        {
            stringUri = string.Format("net.tcp://{0}:{1}/P2PService", ip, port);
            Uri = new Uri(stringUri);
        }

        public PeerEntry(string newUri)
        {
            stringUri = newUri;
            Uri = new Uri(stringUri);
        }

        public PeerEntry(PeerEntry Peer)
        {
            this.PeerName = Peer.PeerName;
            this.ServiceProxy = Peer.ServiceProxy;
            this.Nick = Peer.Nick;
            this.stringUri = Peer.stringUri;
            this.IsConnected = Peer.IsConnected;
            this.Uri = Peer.Uri;
        }

        public override string ToString()
        {
            return Nick + ": " + stringUri + ((IsConnected) ? " - connected" : " - not connected"); 
        }
    }
}