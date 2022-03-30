using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcPracticaAzure2_DiegoSanchezCanamero.Models;
using MvcPracticaAzure2_DiegoSanchezCanamero.Services;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using System.IO;

namespace MvcPracticaAzure2_DiegoSanchezCanamero.Controllers
{
    public class TicketController : Controller
    {
        private ServiceTickets service;
        private BlobServiceClient blobClient;
        public TicketController
            (ServiceTickets service, BlobServiceClient blobClient)
        {
            this.service = service;
            this.blobClient = blobClient;
        }
        
        //La idea era tener un authorize, pero como no me da tiempo
        //voy a buscar a los tickets de los usuarios por el numero 
        // de usuario simplemente
        
        public IActionResult GetTicketsUsuario()
        {
            return View();
        }
        [HttpPost]    
        public async Task<IActionResult> GetTicketsUsuario(int idusuario)
        {
            List<Ticket> tickets =
                await this.service.GetTicketsUsuario(idusuario);
            return View(tickets);
        }
        public IActionResult FindTicket()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FindTicket(int idticket)
        {
            Ticket ticket =
                await this.service.FindTickets(idticket);
            return View(ticket);
        }

        public IActionResult CrearTicket()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearTicket(Ticket ticket, IFormFile file)
        {
           await this.service.CrearTicket(ticket.IdUsuario, ticket.Importe, ticket.Producto, ticket.FileName,"/tickets");
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.SubirBlob(file.FileName, stream);
            }

            ViewData["MENSAJE"] = "El ticket: '" + ticket.IdTicket + "' se creó correctament";
            return View();
        }
    }
}
