using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace WCF_Chat_2._0
{
    [ServiceContract]
    public interface IChat
    {

        [OperationContract]
        void SendStuffMessege(string chaters);

        [OperationContract]
        void SendUsersMessege(string s);

        [OperationContract]
        string[] Hello(string gettingAddress);
    }


    public delegate void SendStuffMessage(string Chater);
    public delegate string[] AddingNewChater(string Chater);
    public delegate void SendUsersMessage(string message);
    public delegate void EndWork();
}