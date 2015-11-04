using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Threading;

namespace ChatWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class MyService : IMyService
    {
        public static string MessageFrom;
        public static List<string> Clients = new List<string>();
        public static AutoResetEvent Reset = new AutoResetEvent(false);      

        public List<string> GetAllData()
        {
            return Clients;
        }

        public string DefineAddress(string MyPort)
        {
            OperationContext Context = OperationContext.Current;
            MessageProperties MessageProperties = Context.IncomingMessageProperties;
            RemoteEndpointMessageProperty EndpointProperty = MessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string IP = EndpointProperty.Address;
            return "net.tcp://" + IP + ":" + MyPort + "/";
        }

        public void ConnectNewClient(string AddressClient, string Nick)
        {
            Clients.Add(AddressClient);
        }

        public void AddNewMessage(string Message)
        {
            MessageFrom = Message;
            Reset.Set();
        }
    }
}