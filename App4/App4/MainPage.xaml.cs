using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
    public partial class MainPage : ContentPage
    {
        HttpClient client;
        
        string TodoItemsUrl = "http://192.168.1.7/todoapi/api/todoitems/{0}";
        public   MainPage()
        {
            InitializeComponent();        
            client = new HttpClient();
            

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                listView.ItemsSource = await RefreshDataAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }       
        private async void Button_Clicked(object sender, EventArgs e)
        {
           await SaveTodoItemAsync(new TodoItem {ID="yyy", Name = "Hugo", Done=true, Notes="Notas" });
          listView.ItemsSource =  await RefreshDataAsync();
        }

        public async Task<List<TodoItem>> RefreshDataAsync()
        {
            List<TodoItem> Items = new List<TodoItem>(); ;
            Uri uri = new Uri(string.Format(TodoItemsUrl, "Get"));
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Items = JsonConvert.DeserializeObject<List<TodoItem>>(content);
            }
            return Items;
        }

        public async Task SaveTodoItemAsync(TodoItem item)
        {



            try
            {

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                var uri = new Uri(string.Format(TodoItemsUrl, "Create"));
                response = await client.PostAsync(uri, content);

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
    }
    public class TodoItem
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public bool Done { get; set; }
    }
}
