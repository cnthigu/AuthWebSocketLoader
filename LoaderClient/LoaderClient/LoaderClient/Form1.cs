using System;
using System.Windows.Forms;
using WebSocketSharp;
using Newtonsoft.Json;

namespace LoaderClient
{
    public partial class Form1 : Form
    {
        private WebSocket ws;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (ws == null || ws.ReadyState != WebSocketState.Open)
            {
                ws = new WebSocket("ws://localhost:8080/auth");

                ws.OnMessage += Ws_OnMessage;
                ws.OnOpen += (s, ev) => Log("Conectado ao servidor WebSocket.");
                ws.OnClose += (s, ev) => Log("Conexão encerrada.");
                ws.OnError += (s, ev) => Log("Erro: " + ev.Message);

                ws.Connect();
            }

            string hwidReal = HardwareHelper.GetMachineHWID();

            var dadosLogin = new
            {
                username = txtUsername.Text.Trim(),
                password = txtPassword.Text.Trim(),
                hwid = hwidReal
            };

            string json = JsonConvert.SerializeObject(dadosLogin);
            ws.Send(json);
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            dynamic resposta = JsonConvert.DeserializeObject(e.Data);

            if (resposta.success == true)
            {
                Log("Login aceito!");
            }
            else
            {
                Log("Login falhou: " + resposta.error);
            }
        }

        private void Log(string texto)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() =>
                {
                    txtLog.AppendText(texto + Environment.NewLine);
                }));
            }
            else
            {
                txtLog.AppendText(texto + Environment.NewLine);
            }
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
