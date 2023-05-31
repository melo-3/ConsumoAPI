using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;

namespace ConsumoAPI
{
    public partial class Form1 : Form
    {
        private readonly HttpClient httpClient;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();

            httpClient= new HttpClient();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //executa o codigo se der erro vai pro catch

                string linkapi = "http://localhost:5126/api/Personagens";
                HttpResponseMessage response = await httpClient.GetAsync(linkapi);

                if(response.IsSuccessStatusCode)
                {
                    string conteudo = await response.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<Personagem>>(conteudo);
                    CriarEstrutura(lista);
                }
                else
                {
                    MessageBox.Show($"Erro ao consumir API: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
                //executa outro codigo 

            }
        }

        private void CriarEstrutura(List<Personagem> listaObj)
        {
            //dataGridView1
            dataTable= new DataTable();
            dataTable.Columns.Add("Nome", typeof(string));
            dataTable.Columns.Add("Sobrenome",typeof(string));
            dataTable.Columns.Add("Fantasia",typeof(string));
            dataTable.Columns.Add("Local",typeof(string));

            foreach (var item in listaObj)
            {
                dataTable.Rows.Add(item.Nome, item.Sobrenome, item.Fantasia, item.Local);
            }
            dataGridView1.DataSource = dataTable;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public class Personagem
        {
            public int Id { get; set;}
            public string Nome { get; set;} 
            public string Sobrenome { get; set;}
            public string Fantasia { get; set;}
            public string Local { get; set;}
        }

        private async void btnAddP_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;
            string sobrenome = txtSobrenome.Text;
            string fantasia = txtFantasia.Text;
            string local = txtLocal.Text;

            string urlApi = "http://localhost:5126/api/Personagens";

            var parametros = new Dictionary<string, string>
            {
                {"Id","0" }, //problema pq id é int -> solução: lista
                { "Nome", nome },
                { "Sobrenome", sobrenome },
                { "Fantasia", fantasia },
                { "Local", local }
            };

            var content = new FormUrlEncodedContent(parametros);
            var response = await httpClient.PostAsync(urlApi, content);

            if (response.IsSuccessStatusCode)
            {
                txtNome.Text = "";
                txtSobrenome.Text = "";
                txtFantasia.Text = "";
                txtLocal.Text = "";

            }
        }
    }
}
