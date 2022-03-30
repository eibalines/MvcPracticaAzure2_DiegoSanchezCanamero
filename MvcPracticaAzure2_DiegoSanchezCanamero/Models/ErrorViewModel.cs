using System;

namespace MvcPracticaAzure2_DiegoSanchezCanamero.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
