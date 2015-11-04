using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ChatWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string DefineAddress(string MyPort);

        [OperationContract]
        void ConnectNewClient(string AddressClient, string Nick);

        [OperationContract]
        List<string> GetAllData();

        [OperationContract]
        void AddNewMessage(string message);
    }
}

