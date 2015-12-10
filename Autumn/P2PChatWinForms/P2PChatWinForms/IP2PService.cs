using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
//using System.Windows.Controls;

namespace P2PChatWinForms
{
    [ServiceContract]
    public interface IP2PService
    {
        [OperationContract]
        void AddClient(PeerEntry client);
        
        [OperationContract]
        void RemoveClient(PeerEntry client);

        [OperationContract]
        List<PeerEntry> GetClients();

        [OperationContract]
        void Push(P2PChatWinForms.Message Msg);
    }
}