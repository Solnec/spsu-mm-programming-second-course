using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Net;

namespace WCF_Chat_2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Server MyServer = new Server();
            Client MyClient = new Client();
            Interface MyInterface = new Interface();
            MyServer.GettingNewAddress += MyClient.GetNewAddress;
            MyServer.ConnectedNewChaterToUs += MyClient.SendingStuffMessage;
            MyServer.GettingUsersMessage += MyInterface.WriteToConsole;
            MyInterface.EnteredNewMessage += MyClient.SendUsersMessage;
            Console.WriteLine("Enter port for getting messege");
            string MyPort = Console.ReadLine();
            String strHostName = Dns.GetHostName(); // узнаем имя компьютера
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //узнаем IP адрес
            IPAddress[] addr = ipEntry.AddressList;
            string MyIP = addr[0].ToString();
            string MyAdress = "http://" + MyIP + ":" + MyPort;
            MyClient.Chaters[0] = MyAdress;
            MyClient.MyAddress = MyAdress;
            WebServiceHost host = new WebServiceHost(MyServer, new Uri(MyAdress));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IChat), new WebHttpBinding(), "");
            host.Open();
            Console.WriteLine("Enter IP adress your companion");
            string IP = Console.ReadLine();
            Console.WriteLine("Enter port your companion");
            string Port = Console.ReadLine();
            Console.WriteLine("Enter your name");
            string Name = Console.ReadLine();
            MyInterface.Name = Name;
            string Adress = "http://" + IP + ":" + Port;
            MyClient.Start(Adress, Name);
            MyInterface.Start();
            host.Close();
        }
    }
}
