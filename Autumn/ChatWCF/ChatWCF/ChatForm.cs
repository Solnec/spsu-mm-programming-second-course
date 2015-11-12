using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatWCF
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
        }

        delegate void Proc();
        Network net = new Network();

        private void Entered_Click(object sender, EventArgs e)
        {
            if (Nick.Text == "")
            {
                MessageBox.Show("Введите свой ник!");
                return;
            }

            if (((IPFriend.Text == "") ^ (PortFriend.Text == "")) && (Port.Text != ""))
            {
                MessageBox.Show("Введите IP и порт друга или начните новую беседу");
                return;
            }

            string enter = net.Start(Port.Text, IPFriend.Text, PortFriend.Text, Nick.Text);

            if (enter == "!")
            {
                MessageBox.Show("Ошибка при попытке подключения, попробуйте еще раз!");
                return;
            }

            richTextBox1.Visible = true;
            richTextBox2.Visible = true;
            buttonSend.Visible = true;

            Thread thread1 = new Thread(delegate() { Reading(net.serv.Reset); });
            thread1.Start();
            thread1.IsBackground = true;

            MessageBox.Show(enter);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string Message = Nick.Text + ":" + richTextBox1.Text;
            net.SendMessage(Message);
            richTextBox2.Text += ("\n" + Message);
            richTextBox1.Text = "";
        }

        private void Reading(AutoResetEvent reset)
        {
            while (true)
            {
                reset.WaitOne();
                if (!string.IsNullOrEmpty(net.serv.MessageFrom))
                    if (net.serv.MessageFrom[0] == '!')
                    {
                        net.serv.MessageFrom = net.serv.MessageFrom.Remove(0, 1);
                        string AddressFriend = net.serv.MessageFrom;
                        net.DeleteFriend(AddressFriend);
                    }
                if (this.InvokeRequired)
                {
                    this.Invoke(new Proc(delegate() { richTextBox2.Text += ("\n" + net.serv.MessageFrom); }));
                }
                else
                    richTextBox2.Text += ("\n" + net.serv.MessageFrom);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread exit = new Thread(delegate() { net.Exit(Nick.Text, Port.Text); });
        }
    }
}
