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
        [WebInvoke]
        void SendStuffMessege(string Chaters);

        [OperationContract]
        [WebInvoke]
        void SendUsersMessege(string s);

        [OperationContract]
        [WebInvoke]
        string[] Hello(string GettingAddress);   
    }
    
    
    public delegate void SendStuffMessage(string Chater);
    public delegate string[] AddingNewChater (string Chater);
}
