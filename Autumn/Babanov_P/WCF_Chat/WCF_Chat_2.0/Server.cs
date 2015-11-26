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
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single)]
    class Server:IChat
    {
        
        public event AddingNewChater GettingNewAddress;
        public event SendStuffMessage ConnectedNewChaterToUs;
        public void SendUsersMessege(string s)
        {
            Console.WriteLine(s);
        }
        public void SendStuffMessege(string Chater)
        {
            GettingNewAddress(Chater);
        }
        public string[] Hello(string GettingAddress)
        {
            string[] ArrayOfChaters = GettingNewAddress(GettingAddress);
            ConnectedNewChaterToUs(GettingAddress);
            return ArrayOfChaters;
        }
    }

}
