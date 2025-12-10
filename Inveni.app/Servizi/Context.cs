using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.Xml;
using CoreGraphics;
using Palmipedo.Models;
using System.Threading.Tasks;

namespace Palmipedo.iOS.Core
{
    public enum Mode
    {
        BACKGROUND,
        FOREGROUND
    }

    public static class Context
    {
        private static object _lockForMode = new object();
        private static object _lockForUser = new object();

        public static NSString OnChangedModeNotification = new NSString("OnChangedMode");

        public static int? ServerUserId { get; set; }
        public static bool IsAdministrator { get; set; }
        public static string CurrentLangAbbr { get; private set; }
        public static string CurrentLangCulture { get; private set; }
        public static string CurrentLang { get; private set; }
        public static UIImage CurrentLangFlag { get; private set; }

        public static UIColor DefaultBlue = UIColor.FromRGB(66, 134, 244);
        public static UIColor LightBlue = UIColor.FromRGB(94, 161, 255);
        public static CGColor GradientStartColor = UIColor.Clear.CGColor;
        public static CGColor GradientEndColor = UIColor.Black.CGColor;

        public static Models.Comune CurrentCity { get; set; }

        public static Dictionary<string, string> LocalizationResourceEnUs { get; private set; }
        public static Dictionary<string, string> LocalizationResourceItIt { get; private set; }
        public static Dictionary<string, string> LocalizationResource { get; private set; }
        public static Mode Mode { get; private set; }
        public static bool IsInTreasureHuntMode { get; set; }
        public static int? IdGioco { get; set; }

        public static PlayerManager PlayerManager { get; private set; }
        public static LocationManager LocationManager { get; private set; }
        public static AccelerometerManager AccelerometerManager { get; private set; }
        public static NotificationManager NotificationManager { get; private set; }

        public static int CoordinateRefreshTimeoutInSeconds { get; set; }


<<<<<<< TODO: Modifica senza merge dal progetto 'Inveni.app (net8.0-windows10.0.19041.0)', Prima:
        public static Entities.User LoggedUser { get; private set; }
=======
        public static User LoggedUser { get; private set; }
>>>>>>> Dopo

<<<<<<< TODO: Modifica senza merge dal progetto 'Inveni.app (net8.0-windows10.0.19041.0)', Prima:
        public static Inveni.app.Modelli.User LoggedUser { get; private set; }
=======
        public static User LoggedUser { get; private set; }
>>>>>>> Dopo
        public static Inveni.app.Elementi.User LoggedUser { get; private set; }

        static Context()
        {
            CoordinateRefreshTimeoutInSeconds = 60;

            Mode = Mode.FOREGROUND;
#if __DEBUG__
            IsAdministrator = true;
#endif
            ServerUserId = Context.SendLog_InviaIdUtenteSync();
            Context.User_CheckLoginSync();

            LocalizationResourceEnUs = LoadLocalizationResources(Entities.Common.LANGUAGE_INGLESE_CULTURE);
            LocalizationResourceItIt = LoadLocalizationResources(Entities.Common.LANGUAGE_ITALIANO_CULTURE);
        }

        public static void InitManagers()
        {
            PlayerManager = PlayerManager.Instance;
            LocationManager = LocationManager.Instance;
            AccelerometerManager = AccelerometerManager.Instance;
            NotificationManager = NotificationManager.Instance;
        }

        public static void SetCurrentLang(string culture)
        {
            if (culture.Replace("_", "-").ToLower() == Core.Entities.Common.LANGUAGE_ITALIANO_CULTURE.ToLower())
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_ITALIANO_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_ITALIANO_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_ITALIANO;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_ITALIANO_FLAG);
            }
            else if (culture.Replace("_", "-").ToLower() == Core.Entities.Common.LANGUAGE_INGLESE_CULTURE.ToLower())
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_INGLESE_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_INGLESE_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_INGLESE;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_INGLESE_FLAG);
            }
            else if (culture.Replace("_", "-").ToLower() == Core.Entities.Common.LANGUAGE_FRANCESE_CULTURE.ToLower())
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_FRANCESE_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_FRANCESE_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_FRANCESE;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_FRANCESE_FLAG);
            }
            else if (culture.Replace("_", "-").ToLower() == Core.Entities.Common.LANGUAGE_SPAGNOLO_CULTURE.ToLower())
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_SPAGNOLO_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_SPAGNOLO_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_SPAGNOLO;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_SPAGNOLO_FLAG);
            }
            else if (culture.Replace("_", "-").ToLower() == Core.Entities.Common.LANGUAGE_TEDESCO_CULTURE.ToLower())
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_TEDESCO_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_TEDESCO_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_TEDESCO;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_TEDESCO_FLAG);
            }
            else if (culture.Replace("_", "-").ToLower() == Core.Entities.Common.LANGUAGE_RUSSO_CULTURE.ToLower())
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_RUSSO_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_RUSSO_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_RUSSO;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_RUSSO_FLAG);
            }
            else
            {
                CurrentLangCulture = Entities.Common.LANGUAGE_INGLESE_CULTURE;
                CurrentLangAbbr = Entities.Common.LANGUAGE_INGLESE_ABBR;
                CurrentLang = Entities.Common.LANGUAGE_INGLESE;
                CurrentLangFlag = UIImage.FromBundle(Core.Entities.Common.LANGUAGE_INGLESE_FLAG);
            }
        }

        public static UIImage GetFlagImageFromNativeName(string nativeLanguageName)
        {
            if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_ITALIANO_NATIVE.ToLower())
            {
                return UIImage.FromBundle(Core.Entities.Common.LANGUAGE_ITALIANO_FLAG);
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_INGLESE_NATIVE.ToLower())
            {
                return UIImage.FromBundle(Core.Entities.Common.LANGUAGE_INGLESE_FLAG);
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_FRANCESE_NATIVE.ToLower())
            {
                return UIImage.FromBundle(Core.Entities.Common.LANGUAGE_FRANCESE_FLAG);
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_SPAGNOLO_NATIVE.ToLower())
            {
                return UIImage.FromBundle(Core.Entities.Common.LANGUAGE_SPAGNOLO_FLAG);
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_TEDESCO_NATIVE.ToLower())
            {
                return UIImage.FromBundle(Core.Entities.Common.LANGUAGE_TEDESCO_FLAG);
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_RUSSO_NATIVE.ToLower())
            {
                return UIImage.FromBundle(Core.Entities.Common.LANGUAGE_RUSSO_FLAG);
            }
            else
            {
                return null;
            }
        }

        public static string GetCultureFromNativeName(string nativeLanguageName)
        {
            if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_ITALIANO_NATIVE.ToLower())
            {
                return Entities.Common.LANGUAGE_ITALIANO_CULTURE;
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_INGLESE_NATIVE.ToLower())
            {
                return Entities.Common.LANGUAGE_INGLESE_CULTURE;
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_FRANCESE_NATIVE.ToLower())
            {
                return Entities.Common.LANGUAGE_FRANCESE_CULTURE;
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_SPAGNOLO_NATIVE.ToLower())
            {
                return Entities.Common.LANGUAGE_SPAGNOLO_CULTURE;
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_TEDESCO_NATIVE.ToLower())
            {
                return Entities.Common.LANGUAGE_TEDESCO_CULTURE;
            }
            else if (nativeLanguageName.ToLower() == Core.Entities.Common.LANGUAGE_RUSSO_NATIVE.ToLower())
            {
                return Entities.Common.LANGUAGE_RUSSO_CULTURE;
            }
            else
            {
                return null;
            }
        }

        public static string GetCurrentLangCultureFromLang(string lang)
        {
            if (lang.ToLower() == Entities.Common.LANGUAGE_ITALIANO.ToLower())
            {
                return Entities.Common.LANGUAGE_ITALIANO_CULTURE;
            }
            else if (lang.ToLower() == Entities.Common.LANGUAGE_INGLESE.ToLower())
            {
                return Entities.Common.LANGUAGE_INGLESE_CULTURE;
            }
            else if (lang.ToLower() == Entities.Common.LANGUAGE_FRANCESE.ToLower())
            {
                return Entities.Common.LANGUAGE_FRANCESE_CULTURE;
            }
            else if (lang.ToLower() == Entities.Common.LANGUAGE_TEDESCO.ToLower())
            {
                return Entities.Common.LANGUAGE_TEDESCO_CULTURE;
            }
            else if (lang.ToLower() == Entities.Common.LANGUAGE_SPAGNOLO.ToLower())
            {
                return Entities.Common.LANGUAGE_SPAGNOLO_CULTURE;
            }
            else if (lang.ToLower() == Entities.Common.LANGUAGE_RUSSO.ToLower())
            {
                return Entities.Common.LANGUAGE_RUSSO_CULTURE;
            }
            else
            {
                return Entities.Common.LANGUAGE_INGLESE_CULTURE;
            }
        }

        public static string GetLoacalizedValueByName(string name)
        {
            if (LocalizationResource.ContainsKey(name))
            {
                return LocalizationResource[name];
            }
            else if (LocalizationResourceEnUs.ContainsKey(name))
            {
                return LocalizationResourceEnUs[name];
            }
            else if (LocalizationResourceItIt.ContainsKey(name))
            {
                return LocalizationResourceItIt[name];
            }
            else
            {
                return name;
            }
        }

        public static void LoadCurrentLocalizationResources()
        {
            if (CurrentLangCulture == Entities.Common.LANGUAGE_INGLESE_CULTURE)
            {
                LocalizationResource = LocalizationResourceEnUs;
            }
            else if (CurrentLangCulture == Entities.Common.LANGUAGE_ITALIANO_CULTURE)
            {
                LocalizationResource = LocalizationResourceItIt;
            }
            else
            {
                LocalizationResource = LoadLocalizationResources(CurrentLangCulture);
            }
        }

        private static Dictionary<string, string> LoadLocalizationResources(string culture)
        {
            Dictionary<string, string> localizationResource = new Dictionary<string, string>();

            var text = System.IO.File.ReadAllText(string.Format("Localizations/{0}/strings.xml", culture));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            XmlNodeList nodes = doc.SelectNodes("resources/string");

            foreach (XmlNode node in nodes)
            {
                localizationResource.Add(node.Attributes["name"].Value, node.InnerText);
            }

            return localizationResource;
        }

        public static void SetMode(Mode mode)
        {
            lock (_lockForMode)
            {
                Mode = mode;

                NSNotificationCenter.DefaultCenter.PostNotificationName("OnChangedMode", null, null);
            }
        }

        public static async Task SendLog_InviaIdUtente(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
                return;

            Models.IdUtenteRequest request = new Models.IdUtenteRequest();
            request.liveID = "LiveID";
            request.deviceID = UIDevice.CurrentDevice.IdentifierForVendor.ToString();
            request.linguaSelezionata = Context.CurrentLangAbbr;
            request.linguaSistema = NSLocale.CurrentLocale.LocaleIdentifier;
            request.nomeApp = "Palmipedo iOS";
            request.versionePRG = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"] + " - Build: " + NSBundle.MainBundle.InfoDictionary["CFBundleVersion"];
            request.terminaleTipo = UIDevice.CurrentDevice.Name;
            request.terminaleFirmware = UIDevice.CurrentDevice.SystemVersion;
            request.comune = cityName;
            request.utenteTrial = false;

            try
            {
                Models.IdUtenteResponse response = await ApiManager.Log_InviaIdUtente(request);
#if __RELEASE__
                IsAdministrator = response.isAdministrator;
#endif
            }
            catch (Exception ex)
            {
                Context.Trace_TraceError(ex);
            }
        }

        public static int SendLog_InviaIdUtenteSync()
        {
            if (Context.ServerUserId.HasValue)
                return Context.ServerUserId.Value;

            Models.IdUtenteRequest request = new Models.IdUtenteRequest();
            request.liveID = "LiveID";
            request.deviceID = UIDevice.CurrentDevice.IdentifierForVendor.ToString();
            request.linguaSelezionata = Context.CurrentLangAbbr;
            request.linguaSistema = NSLocale.CurrentLocale.LocaleIdentifier;
            request.nomeApp = "Palmipedo iOS";
            request.versionePRG = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"] + " - Build: " + NSBundle.MainBundle.InfoDictionary["CFBundleVersion"];
            request.terminaleTipo = UIDevice.CurrentDevice.Name;
            request.terminaleFirmware = UIDevice.CurrentDevice.SystemVersion;
            request.comune = "";
            request.utenteTrial = false;

            try
            {
                Models.IdUtenteResponse response = ApiManager.Log_InviaIdUtenteSync(request);
#if __RELEASE__
                IsAdministrator = response.isAdministrator;
#endif
                response.isAdministrator = response.isAdministrator;
                return response.id;
            }
            catch (Exception ex)
            {
                Context.Trace_TraceError(ex);
                return 0;
            }
        }

        public static async void SendLog_InviaLogSchede(string city, int? catagoryCode, int? serviceCode, int? cardCode, string fileName, string operation)
        {
            InviaLogSchedeRequest request = new InviaLogSchedeRequest();
            request.IdUtente = Context.ServerUserId.HasValue ? Context.ServerUserId.Value : -1;
            request.Latitudine = LocationManager.CurrentLocation != null ? LocationManager.CurrentLocation.Coordinate.Latitude : 0;
            request.Longitudine = LocationManager.CurrentLocation != null ? LocationManager.CurrentLocation.Coordinate.Longitude : 0;
            request.Precisione = LocationManager.CurrentLocation != null ? LocationManager.CurrentLocation.HorizontalAccuracy : 0;
            request.Comune = city != null ? city : "";  /// Comune selezionato/scelto
            request.CodiceCategoria = catagoryCode.HasValue ? catagoryCode.Value : -1; /// Categoria scheda (se disponibile)
            request.CodiceServizio = serviceCode.HasValue ? serviceCode.Value : -1; /// Servizio scheda (se disponibile)
            request.CodiceScheda = cardCode.HasValue ? cardCode.Value : -1; /// ID Scheda (se disponibile)
            request.NomeFileMP3 = fileName != null ? string.Format("infoFile={0}", fileName) : ""; /// Nome file utilizzato (Testo, audio, ecc..)
            request.Operazione = operation; /// Operazione effettuata

            try
            {
                await ApiManager.Log_InviaLogSchede(request);
            }
            catch (Exception) { }
        }

        public static async void SendLog_InviaLogGiochi(int idGioco, string city, int? catagoryCode, int? serviceCode, int? cardCode, string fileName, string operation)
        {
            InviaLogGiochiRequest request = new InviaLogGiochiRequest();
            request.IdUtente = Context.ServerUserId.HasValue ? Context.ServerUserId.Value : -1;
            request.Latitudine = LocationManager.CurrentLocation != null ? LocationManager.CurrentLocation.Coordinate.Latitude : 0;
            request.Longitudine = LocationManager.CurrentLocation != null ? LocationManager.CurrentLocation.Coordinate.Longitude : 0;
            //request.Precisione = _locationManager.CurrentLocation != null ? LocationManager.CurrentLocation.HorizontalAccuracy : 0;
            request.Comune = city != null ? city : "";  /// Comune selezionato/scelto
            request.CodiceCategoria = catagoryCode.HasValue ? catagoryCode.Value : -1; /// Categoria scheda (se disponibile)
            request.CodiceServizio = serviceCode.HasValue ? serviceCode.Value : -1; /// Servizio scheda (se disponibile)
            request.CodiceScheda = cardCode.HasValue ? cardCode.Value : -1; /// ID Scheda (se disponibile)
            request.NomeFileMP3 = fileName != null ? string.Format("infoFile={0}", fileName) : ""; /// Nome file utilizzato (Testo, audio, ecc..)
            request.IdGioco = idGioco;
            request.Operazione = operation; /// Operazione effettuata

            try
            {
                await ApiManager.Log_InviaLogGiochi(request);
            }
            catch (Exception) { }
        }

        public static void Trace_TraceError(Exception ex, bool managed = false)
        {
            TraceErrorRequest request = new TraceErrorRequest();
            request.DeviceId = UIDevice.CurrentDevice.IdentifierForVendor.ToString();
            request.CurrentLang = Context.CurrentLangAbbr;
            request.SystemLang = NSLocale.CurrentLocale.LocaleIdentifier;
            request.App = "Palmipedo iOS";
            request.Version = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"] + " - Build: " + NSBundle.MainBundle.InfoDictionary["CFBundleVersion"];
            request.DeviceName = UIDevice.CurrentDevice.Name;
            request.DeviceSystemVersion = UIDevice.CurrentDevice.SystemVersion;
            request.ErrorMessage = (managed ? "MANAGED " : "") + ex.Message;
            request.StackTrace = ex.ToString();

            try
            {
                ApiManager.Trace_TraceError(request);
            }
            catch (Exception)
            {

            }
        }

        public static async Task<LoginUTResponse> User_Login(string username, string password)
        {
            var result = await ApiManager.Accesso_LoginUT(new LoginUTRequest { IdUtente = ServerUserId.Value, UserUt = username.Trim(), PswUt = password.Trim() });

            if (result.EsitoLogin == 0)
            {
                lock (_lockForUser)
                {
                    LoggedUser = new Entities.User();
                    LoggedUser.Username = username.Trim();
                }
            }

            return result;
        }

        public static async Task<CheckLoginResponse> User_CheckLogin()
        {
            var result = await ApiManager.Accesso_CheckLogin(new CheckLoginRequest { IdUtente = ServerUserId.Value });

            if (result.EsitoLogin == 0)
            {
                lock (_lockForUser)
                {
                    LoggedUser = new Entities.User();
                    LoggedUser.Username = result.UserUt.Trim();
                }
            }

            return result;
        }

        public static void User_CheckLoginSync()
        {
            var result = ApiManager.Accesso_CheckLoginSync(new CheckLoginRequest { IdUtente = ServerUserId.Value });

            if (result.EsitoLogin == 0)
            {
                lock (_lockForUser)
                {
                    LoggedUser = new Entities.User();
                    LoggedUser.Username = result.UserUt.Trim();
                }
            }
        }


        public static async Task<CancellaIscrizioneResponse> User_DeleteSubscription()
        {
            var result = await ApiManager.Accesso_CancellaIscrizione(new CancellaIscrizioneRequest { IdUtente = ServerUserId.Value });

            if (result.EsitoLogin == 0)
            {
                lock (_lockForUser)
                {
                    LoggedUser = null;
                }
            }

            return result;
        }

        public static async Task<LogoutResponse> User_Logout()
        {
            var result = await ApiManager.Accesso_Logout(new LogoutRequest { IdUtente = ServerUserId.Value });

            if (result.Esito == 0)
            {
                lock (_lockForUser)
                {
                    LoggedUser = null;
                }
            }

            return result;
        }
    }
}