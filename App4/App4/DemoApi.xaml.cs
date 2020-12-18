using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DemoApi : ContentPage
    {
        HttpClient httpcliente;

        public DemoApi()
        {
            InitializeComponent();
            httpcliente = new HttpClient();
        }       

        private async void Button_Clicked(object sender, EventArgs e)
        {
            listView.ItemsSource = await GetNotesAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await SaveTodoNoteAsync(new Note { ID = "XXXX", Name = "XXXXX", Done = true, Notes = "xxxxx" });
            listView.ItemsSource = await GetNotesAsync();
        }
        public async Task SaveTodoNoteAsync(Note note)
        {

            try
            {

                var json = JsonConvert.SerializeObject(note);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                var uri = new Uri("http://192.168.1.7/TodoApi/api/todoitems/create");

                response = await httpcliente.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tTodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task<List<Note>> GetNotesAsync()
        {
            List<Note> notes = new List<Note>();

            HttpResponseMessage response = null;
            Uri uri = new Uri("http://192.168.1.7/TodoApi/api/todoitems/list");

            response = await httpcliente.GetAsync(uri);
            //JSON

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                notes = JsonConvert.DeserializeObject<List<Note>>(content);
            }

            return notes;

        }
    }
}