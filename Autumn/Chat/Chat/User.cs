using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    public class User
    {
        public string Name { get; private set; }

        public IChat Channel;
        public IChat Host;
        private readonly AutoResetEvent _stopFlag = new AutoResetEvent(false);
        private DuplexChannelFactory<IChat> _factory;

        public User(string name)
        {
            Name = name;
            IsStarted = false;
        }

        public void StartService()
        {
            var binding = new NetPeerTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            var endpoint = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IChat)),
                binding,
                new EndpointAddress("net.p2p://ChatP2P"));

            Host = new Chat();

            _factory = new DuplexChannelFactory<IChat>(
                new InstanceContext(Host),
                endpoint);

            var channel = _factory.CreateChannel();

            ((ICommunicationObject)channel).Open();

            Channel = channel;
        }

        public void StopService()
        {
            (Channel as ICommunicationObject).Close();
            if (_factory != null)
                _factory.Close();
        }

        public bool IsStarted
        {
            get;
            private set;
        }
        public void Run()
        {
            Console.WriteLine("[ Starting Service... ]");
            StartService();

            Console.WriteLine("[ Service Started ]");
            IsStarted = true;
            _stopFlag.WaitOne();


            IsStarted = false;
            Console.WriteLine("[ Stopping Service... ]");
            StopService();

            Console.WriteLine("[ Service Stopped ]");
        }

        public void Stop()
        {
            _stopFlag.Set();
        }
    }
}
