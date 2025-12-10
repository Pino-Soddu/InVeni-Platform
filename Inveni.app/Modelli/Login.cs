using System;
namespace Palmipedo.Models
{
    public class IscrizioneUTRequest
    {
        public int IdUtente { get; set; }/// Identificativo App dell'Utente 
        public string UserUt { get; set; }/// User della Login (E-mail)
        public string PswUt { get; set; } /// Password della Login
    }

    public class IscrizioneUTResponse
    {
        public int EsitoLogin { get; set; }    /// Codice Esito Login
        public string DescrEsito { get; set; }/// Descrizione Esito Login 
        public string UserUt { get; set; }/// User della Login (E-mail) 
        public string PswUt { get; set; }/// Password della Login

        //EsitoLogin, DescrEsito
        // 0 = OK (Email inviata, Attesa verifica)
        // 1 = Password non conforme
        // 2 = User già registrato
        // 3 = IdUtente già registrato
        // 4 = Errore generico
    }


    public class LoginUTRequest
    {
        public int IdUtente { get; set; }
        public string UserUt { get; set; }
        public string PswUt { get; set; }
    }

    public class LoginUTResponse
    {
        public int EsitoLogin { get; set; }
        public string DescrEsito { get; set; }

        //EsitoLogin, DescrEsito
        // 0 = OK
        // 1 = Password errata
        // 2 = User errato
        // 3 = User o Password
        // 4 = Errore generico
        // 5 = Validazione Email in corso
    }


    public class CheckLoginRequest
    {
        public int IdUtente { get; set; }
    }

    public class CheckLoginResponse
    {
        public int EsitoLogin { get; set; }
        public string DescrEsito { get; set; }
        public string UserUt { get; set; }
        public string PswUt { get; set; }

        //EsitoLogin, DescrEsito
        // 0 = Login Attiva
        // 1 = Login Scaduta
        // 2 = Utente non registrato
        // 4 = Errore Generico
        // 5 = Validazione Email in corso
    }

    public class CancellaIscrizioneRequest
    {
        public int IdUtente { get; set; }
    }

    public class CancellaIscrizioneResponse
    {
        public int EsitoLogin { get; set; }
        public string DescrEsito { get; set; }
        //EsitoLogin, DescrEsito
        // 0 = OK
        // 4 = Errore generico
    }

    public class RicordaEmailRequest
    {
        public int IdUtente { get; set; }
    }

    public class RicordaEmailResponse
    {
        public int Esito { get; set; }
        public string DescrEsito { get; set; }
        public string UserUt { get; set; }
        public string PswUt { get; set; }
        //EsitoLogin, DescrEsito
        // 0 = OK (Email inviata)
        // 4 = Errore generico
    }


    public class LogoutRequest
    {
        public int IdUtente { get; set; }
    }

    public class LogoutResponse
    {
        public int Esito { get; set; }
        public string DescrEsito { get; set; }
        //EsitoLogin, DescrEsito
        // 0 = OK (Email inviata)
        // 4 = Errore generico
    }
}
