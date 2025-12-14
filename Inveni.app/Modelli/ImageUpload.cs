using System;
using System.Collections.Generic;
using System.Text;

namespace Inveni.App.Modelli
{
    public class ImageUploadSendRequest
    {
        public int IdUtente { get; set; }
        public int IdGioco { get; set; }
        public int CodiceCategoria { get; set; }
        public int CodiceServizio { get; set; }
        public int CodiceScheda { get; set; }
        public int NumeroTentativo { get; set; }
    }

    public class ImageUploadSendResponse
    {
        public string? Messaggio { get; set; }
    }
}
