using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ServiceModel;
using System.Diagnostics;

namespace P2PChatWinForms
{
    public partial class ChatForm : Form
    {
        private P2PService localService;
        public int time = 0;
        private ServiceHost host;
        protected Network Net;
        protected PeerEntry Me;
        public bool HasParent;
        public List<Process> Children = new List<Process>();

        public ChatForm(string s)
        {
            InitializeComponent();
            HasParent = ((s == "1") ? true : false);
            if (HasParent)
            {
                ForkButton.Enabled = false;
                ForkButton.Visible = false;
                this.Text += "- Child " + Process.GetCurrentProcess().Id.ToString();
            }
            else this.Text += " - Parent";

        }

        private void ChatButton_Click(object sender, EventArgs e)
        {
            if (AddressTextBox.Text == "" || NickTextBox.Text == "")
            {
                MessageBox.Show("Please fill in all the fields to enter the chat!");
                return;
            }

            // Registration and start WCF
            Me = new PeerEntry("localhost", AddressTextBox.Text);
            Me.Nick = NickTextBox.Text;
            localService = new P2PService(Me);
            host = new ServiceHost(localService, Me.Uri);
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IP2PService), binding, string.Format("net.tcp://localhost:{0}/P2PService", AddressTextBox.Text));

            try
            {
                host.Open();
                ChatTextBox.Text += "system: Host created successfully at " + AddressTextBox.Text + ".\n";
                MessageTextBox.Focus();
                ChatButton.Enabled = false;
                Net = new Network();

                Thread Waiter = new Thread(new ThreadStart(
                   delegate
                   {
                       while (true)
                       {
                           localService.Caught.WaitOne();
                           Message Got = localService.MessageList.Dequeue();

                           if (this.PreParseMessage(ref Got))
                           {
                               if (Got.time >= time) time = Got.time + 1;
                               else continue;
                               ChatTextBox.Invoke(new Action(delegate() { ChatTextBox.Text += Got.ToString(); }));
                               foreach (PeerEntry Peer in localService.GetClients())
                               {
                                   if (Peer.Nick == Got.from) continue;
                                   Net.Send(Peer, Got);
                               }
                           }
                           else
                           {
                               if (Got.time >= time) time = Got.time + 1;
                           }
                       }
                   }
               ));
                Waiter.IsBackground = true;
                Waiter.Start();
                ChatTextBox.Text += "Now waiting for messages.\n";
                ChatTextBox.Text += "\nYou have some commands:\n";
                ChatTextBox.Text += "   /connect -ip -port for connect.\n";
                ChatTextBox.Text += "   /ls for monitoring all users.\n";
                ChatTextBox.Text += "   /whoami for your ip and port.\n";
                ChatTextBox.Text += "   /time for number of message.\n\n";
                MessageTextBox.Text = "";
            }
            catch (AddressAlreadyInUseException)
            {
                ChatTextBox.Text += "system: This port is busy. Try another.\n";
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            Message newMessage = new Message(MessageTextBox.Text, Me.Nick, "", time);
            MessageTextBox.Text = "";
            ChatTextBox.Text += newMessage.ToString();
            if (PreParseMessage(ref newMessage))
            {
                if (!localService.GetClients().Any())
                {
                    ChatTextBox.Text += "There isn't anyone you're connected to. Type /connect -ip -port to do it.\n";
                    MessageTextBox.Focus();
                    return;
                }
                PeerEntry fail = null;
                foreach (PeerEntry target in localService.GetClients())
                {
                    try
                    {
                        newMessage.Send(localService.GetClients());
                    }
                    catch (Exception)
                    {
                        fail = new PeerEntry(target);
                    }
                }
                localService.GetClients().Remove(fail);
                time++;
            }
            MessageTextBox.Focus();
        }

        private void ForkButton_Click(object sender, EventArgs e)
        {
            Process newProc = new Process();
            newProc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "P2PChatWinForms.exe";
            newProc.StartInfo.Arguments = "1";
            Children.Add(newProc);

            Thread newThread = new Thread(new ThreadStart(delegate
            {
                newProc.Start();
                ChatTextBox.Invoke(new Action(delegate { ChatTextBox.Text += newProc.Id.ToString() + " forked.\n"; }));
                newProc.WaitForExit();
                ChatTextBox.Invoke(new Action(delegate { ChatTextBox.Text += newProc.Id.ToString() + " exited.\n"; }));
                Children.Remove(newProc);
            }));
            newThread.IsBackground = true;
            newThread.Start();
        }

        protected bool PreParseMessage(ref Message Msg)
        {
            string what = Msg.content;
            if (what != "")
            {
                if (what[0] == '/')
                {
                    //we got some command
                    string[] CommandArgs = what.Remove(0, 1).Split(' ');
                    switch (CommandArgs[0])
                    {
                        case "connect":
                            {
                                PeerEntry target;
                                if (CommandArgs.Length == 2)
                                {
                                    // /connect uri

                                    foreach (PeerEntry Client in localService.GetClients())
                                    {
                                        if (Client.stringUri == CommandArgs[1])
                                        {
                                            Client.Nick = Msg.from;
                                            if (Msg.from == Me.Nick)
                                                this.Invoke(new Action(delegate { ChatTextBox.Text += "system: Already connected to " + Client.Nick + "\n"; }));
                                            return false;
                                        }
                                    }
                                    target = new PeerEntry(CommandArgs[1]);
                                }
                                else
                                {
                                    // connect ip port

                                    foreach (PeerEntry Client in localService.GetClients())
                                    {
                                        if (Client.stringUri == string.Format("net.tcp://{0}:{1}/P2PService", CommandArgs[1], CommandArgs[2]))
                                        {
                                            if (Msg.from == Me.Nick)
                                                this.Invoke(new Action(delegate { ChatTextBox.Text += "system: Already connected to " + Client.Nick + "\n"; }));
                                            return false;
                                        }
                                    }
                                    target = new PeerEntry(CommandArgs[1], CommandArgs[2]);
                                }

                                if (Msg.from != Me.Nick) target.Nick = Msg.from;

                                if (Net.Connect(target))
                                {
                                    this.Invoke(new Action(delegate
                                    {
                                        localService.AddClient(target);
                                        target.IsConnected = true;
                                        AddressTextBox.ReadOnly = true;
                                        NickTextBox.ReadOnly = true;
                                        MessageTextBox.ReadOnly = false;
                                        MessageTextBox.Text = "";
                                        MessageTextBox.Focus();
                                    }));

                                    //preparing system message to push
                                    Message systemMsg = new Message("/connect " + Me.stringUri, Me.Nick, "", time++);
                                    try
                                    {
                                        Net.Send(target, systemMsg);
                                        if (localService.GetClients().Count > 1 && target.Nick != null)
                                        {
                                            systemMsg.content = target.Nick + " is now online!\n";
                                            foreach (PeerEntry Peer in localService.GetClients())
                                            {
                                                if (Peer.stringUri == target.stringUri) continue;
                                                Net.Send(Peer, systemMsg);
                                            }
                                        }
                                        if (target.Nick != null) Invoke(new Action(delegate { ChatTextBox.Text += target.Nick + " is now online!\n"; }));
                                        else Invoke(new Action(delegate { ChatTextBox.Text += target.Nick + " system: Connected.\n"; }));
                                    }
                                    catch (EndpointNotFoundException)
                                    {
                                        Invoke(new Action(delegate { ChatTextBox.Text += "system: Error.\n"; }));
                                    }

                                }
                                else this.Invoke(new Action(delegate { ChatTextBox.Text += "system: Connection error."; }));

                                return false;
                            }
                        case "whoami":
                            {
                                ChatTextBox.Text += "You are " + Me.Nick + "(" + Me.stringUri + ").\n";
                                return false;
                            }
                        case "ls":
                            {
                                if (!localService.GetClients().Any()) { Invoke(new Action(delegate { ChatTextBox.Text += "Empty!\n"; })); break; }
                                Invoke(new Action(delegate { ChatTextBox.Text += "List of neighbours: \n"; }));
                                foreach (PeerEntry Peer in localService.GetClients())
                                {
                                    Invoke(new Action(delegate { ChatTextBox.Text += Peer.ToString() + "\n"; }));
                                }
                                return false;
                            }
                        case "time":
                            {
                                ChatTextBox.Text += "Time is " + time + ".\n";
                                return false;
                            }
                        default:
                            {
                                Msg.content = "system: got undefined command " + CommandArgs[0] + ". Message ignored." + ".\n";
                                break;
                            }
                    }
                }
            }

            return true;
        }
    }
}
