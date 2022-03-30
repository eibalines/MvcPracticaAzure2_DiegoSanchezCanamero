using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MvcPracticaAzure2_DiegoSanchezCanamero.Models;
using Newtonsoft.Json;

namespace MvcPracticaAzure2_DiegoSanchezCanamero.Services
{
    public class ServiceTickets
    {
        private MediaTypeWithQualityHeaderValue Header;
        private BlobServiceClient blobClient;
        private Uri uriApi;


        public ServiceTickets(BlobServiceClient blobClient)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.blobClient = blobClient;
            this.uriApi = new Uri("https://apipractica2azurediegosanchezcanamero.azurewebsites.net");
        }
        

        public async Task<List<Ticket>> GetTicketsUsuario(int idusuario)
        {
            
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Empresa/TicketUsuario/" + idusuario;
                client.BaseAddress = this.uriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                List<Ticket> tickets = await response.Content.ReadAsAsync<List<Ticket>>();
                return tickets;

            }
        }

        public async Task<Ticket> FindTickets(int idticket)
        {

            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Empresa/FindTicket/" + idticket;
                client.BaseAddress = this.uriApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                Ticket tickets = await response.Content.ReadAsAsync<Ticket>();
                return tickets;

            }
        }

        public async Task CrearTicket
            (int idusuario, string importe, string producto, string filename, string storagepath )
        {
            string urlCrearTicket =
                "https://prod-147.westeurope.logic.azure.com:443/workflows/ebf3147ded1146b6ac0787168fece23e/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=ZRd_JEi9C5Zk3UVmTUvVT7AoGz0vEBDLamNt74scEPw";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Ticket ticket = new Ticket
                {
                    IdUsuario = idusuario,
                    Fecha = DateTime.Now,
                    Importe = importe,
                    Producto = producto,
                    FileName = filename,
                    StoragePath = storagepath

                };
             
                string json = JsonConvert.SerializeObject(ticket);
               
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(urlCrearTicket, content);
            }
        }
    
        public async Task SubirBlob(string filename, Stream stream)
        {
            BlobContainerClient containerClient =
              this.blobClient.GetBlobContainerClient("tickets");
           await containerClient.UploadBlobAsync(filename, stream);
        }
    }
}
