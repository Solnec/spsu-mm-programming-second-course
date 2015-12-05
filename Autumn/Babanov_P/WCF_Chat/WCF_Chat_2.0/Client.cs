using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.Threading;

namespace WCF_Chat_2._0
{
    class Client
    {
        public string[] Chaters = new string[1];
        public string MyAddress;

        public void Start(string address, string Name)
        {
            if (Chaters.GetLength(0) == 1)
            {
                AddChater(address);
                using (ChannelFactory<IChat> FirstFactory = new ChannelFactory<IChat>(new WebHttpBinding(), Chaters[1]))
                {
                    FirstFactory.Endpoint.Behaviors.Add(new WebHttpBehavior());
                    IChat FirstChannel = FirstFactory.CreateChannel();
                    Chaters = GetAddressesArray(FirstChannel.Hello(Chaters[0]));
                    SortArrayOfChaters();
                }
            }
            else
            {
                SortArrayOfChaters();
            }
        }
        public void SendUsersMessage(string s)
        {
            List<int> deleteChaters = new List<int>();
            for (int i = 1; i < Chaters.GetLength(0); i++)
            {
                try
                {
                    using (ChannelFactory<IChat> cf = new ChannelFactory<IChat>(new WebHttpBinding(), Chaters[i]))
                    {
                        cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                        IChat channel = cf.CreateChannel();
                        channel.SendUsersMessege(s);
                    }
                }
                catch (Exception)
                {
                    deleteChaters.Add(i);
                }
            }
            foreach(int i in deleteChaters)
            {
                DeleteChater(i);
            }
        }
        string[] GetAddressesArray(string[] SendingArray) //при первом подключении получаем массив адресов
        {
            if (SendingArray == null)
            {
                return Chaters;
            }
            string[] NewArray = SendingArray;
            return NewArray;
        }
        void DeleteChater(int i) //удаляем неответившего клиента
        {
            for (int j = i; j < Chaters.GetLength(0) - 1; j++)
            {
                Chaters[j] = Chaters[j + 1];
            }
            Array.Resize<string>(ref Chaters, Chaters.GetLength(0) - 1);
        }
        public void AddChater(string NewChater) //добавляем нового участника
        {

            Array.Resize<string>(ref Chaters, Chaters.GetLength(0) + 1);
            Chaters[Chaters.GetLength(0) - 1] = NewChater;
        }
        public void SendingStuffMessage(string Chater) //рассылаем служебное сообщение при подключении нового клиента
        {
            List<int> deleteChaters = new List<int>();
            for (int i = 1; i < Chaters.GetLength(0); i++)
            {
                if (Chaters[i] != Chater)
                {
                    try
                    {
                        using (ChannelFactory<IChat> cf = new ChannelFactory<IChat>(new WebHttpBinding(), Chaters[i]))
                        {
                            cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                            IChat channel = cf.CreateChannel();
                            channel.SendStuffMessege(Chater);
                        }
                    }
                    catch (Exception)
                    {
                        deleteChaters.Add(i);
                    }

                }
            }
            foreach(int i in deleteChaters)
            {
                DeleteChater(i);
            }
        }
        public string[] GetNewAddress(string NewAddress) //К нам подключился новый клиент или мы получили сообщение с адресом подключившегося клиента
        {
            AddChater(NewAddress);
            return this.Chaters;
        }
        void SortArrayOfChaters()
        {
            for (int i = 1; i < Chaters.GetLength(0); i++)
            {
                if (Chaters[i] == MyAddress)
                {
                    Chaters[i] = Chaters[0];
                    Chaters[0] = MyAddress;
                    break;
                }
            }
        }
    }
}
