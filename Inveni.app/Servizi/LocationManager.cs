using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using CoreLocation;
using System.Threading.Tasks;

namespace Palmipedo.iOS.Core
{
    public class LocationManager
    {
        private static object _lock = new object();
        private static object _lockHeading = new object();

        private Guid id;
        // event for the location changing
        //public event EventHandler<CLLocation> LocationUpdated = delegate { };
        public event EventHandler<CLHeading> UpdatedHeading = delegate { };
        public event EventHandler<Core.Entities.GeofenceCard> RegionEntered = delegate { };
        public event EventHandler<Core.Entities.GeofenceRegion> OnRegionStatusChanged = delegate { };
        public event EventHandler<CLAuthorizationChangedEventArgs> OnAuthorizationChanged = delegate { };
        public event EventHandler<CLLocation> OnLocationUpdated = delegate { };
        private CLLocation _latestMonitoredLocation;
        private List<Models.Scheda> _cards;
        private CLLocationManager _locationManager;
        private Dictionary<int, Models.Scheda> _regionEnteredCards;
        private CLLocation _currentLocation;
        private CLHeading _currentHeading;
        private List<Core.Entities.GeofenceCard> _currentCardsGeofence;
        private Entities.CustomLocation _lastSentLocation;

        private bool _isUpdatingLocationEnabled;
        private bool _isSignificantUpdatingLocationEnabled;
        private CLAuthorizationStatus? _authorizationStatus;

        public CLLocation CurrentLocation
        {
            get
            {
                if (_locationManager != null && CLLocationManager.LocationServicesEnabled)
                {
                    return _locationManager.Location;
                    
                }
                else
                {
                    return null;
                }
            }
        }

        public CLHeading CurrentHeading
        {
            get
            {
                if (_locationManager != null && CLLocationManager.LocationServicesEnabled)
                {
                    return _locationManager.Heading;
                }
                else
                {
                    return null;
                }
            }
        }

        private List<Core.Entities.GeofenceRegion> _geofenceRegions;

        private static LocationManager instance;

        public static LocationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocationManager();
                }
                return instance;
            }
        }

        private LocationManager()
        {
            id = Guid.NewGuid();
            Context.SetMode(Mode.FOREGROUND);

            _locationManager = new CLLocationManager();
            _locationManager.AuthorizationChanged += AuthorizationChanged;

            _locationManager.PausesLocationUpdatesAutomatically = false;
            _locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
            //_locationManager.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;
            //_locationManager.DistanceFilter = 10;
            //_locationManager.ActivityType = CLActivityType.OtherNavigation;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // works in background
                _locationManager.RequestAlwaysAuthorization();
                // only in foreground
                //locMgr.RequestWhenInUseAuthorization ();
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                _locationManager.AllowsBackgroundLocationUpdates = true;
            }

            _regionEnteredCards = new Dictionary<int, Models.Scheda>();

            if (CLLocationManager.SignificantLocationChangeMonitoringAvailable)
            {
                _locationManager.StartMonitoringSignificantLocationChanges();
                _locationManager.LocationsUpdated += BackgroundLocationsUpdated;
                _isSignificantUpdatingLocationEnabled = true;
            }

            _geofenceRegions = new List<Entities.GeofenceRegion>();
            _locationManager.ShouldDisplayHeadingCalibration += (CoreLocation.CLLocationManager manager) => { return false; };
        }

        private void AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            if (!_authorizationStatus.HasValue || e.Status != _authorizationStatus)
            {
                _authorizationStatus = e.Status;
                OnAuthorizationChanged?.Invoke(sender, e);
            }
        }

        //private void _locationManager_MonitoringFailed(object sender, CLRegionErrorEventArgs e)
        //{
        //    _isSignificantUpdatingLocationEnabled = false;
        //}

        //private void _locationManager_Failed(object sender, NSErrorEventArgs e)
        //{
        //    _isSignificantUpdatingLocationEnabled = false;
        //}

        public void StartTacking(List<Models.Scheda> cards)
        {
            if (cards == null || cards.Count <= 0)
                return;

            if (!CLLocationManager.LocationServicesEnabled)
                return;

            StopTracking();

            lock (_lock)
            {
                _currentLocation = null;
                _cards = cards;

                if (!_isUpdatingLocationEnabled)
                {
                    _locationManager.StartUpdatingLocation();
                    _locationManager.StartUpdatingHeading();
                    _isUpdatingLocationEnabled = true;
                }
            }

            _locationManager.LocationsUpdated += ForegroundLocationsUpdated;
            _locationManager.UpdatedHeading += LocationManager_UpdatedHeading;
            Context.PlayerManager.OnFinishPlaying += PlayerManager_OnFinishPlaying;

            //_timer = NSTimer.CreateRepeatingScheduledTimer(20, (NSTimer s) =>
            //{
            //    _locationManager.RequestLocation();
            //});
        }

        private void LocationManager_UpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
        {
            lock (_lockHeading)
            {
                _currentHeading = e.NewHeading;
            }
            UpdatedHeading?.Invoke(sender, _currentHeading);
        }

        public void StartTreasureHuntTraking(List<Models.Scheda> tresureHuntCards)
        {
            lock (_lock)
            {
                if (_currentCardsGeofence == null)
                    _currentCardsGeofence = new List<Core.Entities.GeofenceCard>();

                foreach (var card in tresureHuntCards)
                {
                    if (card == null) continue;

                    Entities.GeofenceCard item = new Entities.GeofenceCard();
                    item.CenterLat = card.lat;
                    item.CenterLng = card.lon;
                    item.Radius = card.radius;
                    item.Name = card.name;
                    item.Card = card;
                    item.StartPlayingMode = Mode.FOREGROUND;
                    _currentCardsGeofence.Add(item);
                }
                _latestMonitoredLocation = null;
            }

            MonitorLocation();
        }

        private void ForegroundLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            OnLocationUpdated?.Invoke(sender, e.Locations?.LastOrDefault());

            if (Context.Mode != Mode.FOREGROUND)
                return;

            if (e.Locations == null || e.Locations[0] == null)
            {
                return;
            }

#if __DEBUG__
            Console.WriteLine("ForegroundLocationsUpdated " + e.Locations.Last().Coordinate.Latitude + " " + e.Locations.Last().Coordinate.Longitude);
#endif

            if (_currentLocation != null && (DateTime.Now - _currentLocation.Timestamp.ToDateTime()).TotalSeconds < 5) return;

            lock (_lock)
            {
                _currentLocation = e.Locations.Last();

                if (_lastSentLocation == null)
                    _lastSentLocation = new Entities.CustomLocation(_currentLocation);

                if ((DateTime.Now - _lastSentLocation.Date).TotalSeconds >= Context.CoordinateRefreshTimeoutInSeconds)
                {
                    _lastSentLocation = new Entities.CustomLocation(_currentLocation);
                    var request = new Models.InviaPosizioneRequest();
                    request.IdUtente = Context.ServerUserId.Value;
                    request.Latitudine = _lastSentLocation.Location.Coordinate.Latitude;
                    request.Longitudine = _lastSentLocation.Location.Coordinate.Longitude;
                    request.Precisione = _lastSentLocation.Location.HorizontalAccuracy;
                    if (Context.IdGioco.HasValue)
                        request.IdGioco = Context.IdGioco.Value;
                    else
                        request.IdGioco = 0;
                    try
                    {
                        ApiManager.Log_InviaPosizione(request);
                    }
                    catch (Exception)
                    {

                    }
                }
            }


            MonitorLocation();
        }

        private void BackgroundLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            if (Context.Mode != Mode.BACKGROUND)
                return;

            if (e.Locations == null || e.Locations[0] == null)
            {
                return;
            }

#if __DEBUG__
            Console.WriteLine("BackgroundLocationsUpdated " + e.Locations[0].Coordinate.Latitude + " " + e.Locations[0].Coordinate.Longitude);
#endif

            if (_currentLocation != null && (DateTime.Now - _currentLocation.Timestamp.ToDateTime()).TotalSeconds < 5) return;

            lock (_lock)
            {
                _currentLocation = e.Locations.Last();
            }

            //LocationUpdated?.Invoke(sender, _currentLocation);

            MonitorLocation();
        }

        private void PlayerManager_OnFinishPlaying(object sender, Models.Scheda e)
        {
            MonitorLocation();
            //if (_timer != null)
            //    _timer.Fire();
        }

        private void MonitorLocation()
        {
#if __DEBUG__
            Console.WriteLine("MonitorLocation");
#endif
            if (_cards == null)
            {
#if __DEBUG__
                Console.WriteLine("MonitorLocation _cards null");
#endif
                return;
            }

            if (!Context.PlayerManager.CanPlay())
            {
#if __DEBUG__
                Console.WriteLine("MonitorLocation !CanPlay");
#endif
                return;
            }

            if (_currentCardsGeofence != null && _currentCardsGeofence.FirstOrDefault(x => x.Playing) != null && !Context.IsInTreasureHuntMode)
            {
#if __DEBUG__
                Console.WriteLine("MonitorLocation _currentCardsGeofence playing");
#endif
                return;
            }

            lock (_lock)
            {
                if (_currentLocation == null)
                {
#if __DEBUG__
                    Console.WriteLine("MonitorLocation _currentLocation null");
#endif
                    return;
                }

                if(_latestMonitoredLocation != null && _latestMonitoredLocation == _currentLocation)
                {
                    return;
                }

                _latestMonitoredLocation = _currentLocation;

                //preservare le treasurehunt se esistono
                List<Entities.GeofenceCard> treasureHuntGeofences = new List<Entities.GeofenceCard>();
                if (_currentCardsGeofence != null)
                    treasureHuntGeofences.AddRange(_currentCardsGeofence.Where(x => x.Card.IsTreasureHuntItem && x.Card.serviceId != 1 && (x.Card.ItemItinerario == null || !x.Card.ItemItinerario.DataCaccia.HasValue))); //escludo quelle che sono gia state risolte

                _currentCardsGeofence = Core.GeoUtils.GetTop20NearestCards(_cards, _currentLocation.Coordinate.Latitude, _currentLocation.Coordinate.Longitude);

                if (treasureHuntGeofences.Count > 0)
                    _currentCardsGeofence.AddRange(treasureHuntGeofences);

                if (_currentCardsGeofence == null)
                {
#if __DEBUG__
                    Console.WriteLine("MonitorLocation _currentCardsGeofence ");
#endif
                    return;
                }
                else
                {
#if __DEBUG__
                    Console.WriteLine("MonitorLocation _currentCardsGeofence " + _currentCardsGeofence.Count());
#endif
                }

                //List<Entities.GeofenceRegion> 
                List<CLCircularRegion> regions = new List<CLCircularRegion>();

                foreach (var item in _currentCardsGeofence)
                {
                    var existingGeofenceRegion = _geofenceRegions.FirstOrDefault(x => x.GeofenceCard.Card._id == item.Card._id);

                    CLCircularRegion region = new CLCircularRegion(new CLLocationCoordinate2D(item.CenterLat, item.CenterLng), item.Radius, item.Card._id.ToString());
                    if (region.ContainsCoordinate(_currentLocation.Coordinate))
                    {
                        regions.Add(region);

                        if (existingGeofenceRegion != null)
                        {
                            if (existingGeofenceRegion.Status != Entities.GeofenceRegionStatus.IN /*|| !existingGeofenceRegion.Handled*/)
                            {
                                existingGeofenceRegion.SetIn();
                            }
                        }
                        else
                        {
                            Core.Entities.GeofenceRegion geofenceRegion = new Entities.GeofenceRegion();
                            geofenceRegion.CircularRegion = region;
                            geofenceRegion.GeofenceCard = item;

                            geofenceRegion.OnStatusChanged += GeofenceRegion_OnStatusChanged;
                            
                            _geofenceRegions.Add(geofenceRegion);

                            geofenceRegion.SetIn();
                        }
                    }
                    else
                    {
                        if (existingGeofenceRegion != null)
                        {
                            if (existingGeofenceRegion.Status != Entities.GeofenceRegionStatus.OUT /*|| !existingGeofenceRegion.Handled*/)
                            {
                                existingGeofenceRegion.SetOut();
                            }
                        }
                    }
                }

                foreach (var geofenceRegion in _geofenceRegions)
                {
                    if (geofenceRegion.Status == Entities.GeofenceRegionStatus.IN
                        && !geofenceRegion.CircularRegion.ContainsCoordinate(_currentLocation.Coordinate))
                    {
                        geofenceRegion.SetOut();
                    }
                }

                _geofenceRegions.RemoveAll(x => x.Status == Entities.GeofenceRegionStatus.OUT && x.LastUpdate.AddMinutes(1) <= DateTime.Now);

#if __DEBUG__
                Console.WriteLine("MonitorLocation regions: " + regions.Count());
#endif
                if (regions.Count > 0 && !Context.IsInTreasureHuntMode)
                {
                    regions = regions.OrderBy(x => x.Radius).ToList();

                    foreach (var region in regions)
                    {
                        Entities.GeofenceCard currentCardGeofence = _currentCardsGeofence.FirstOrDefault(x => x.Card._id == int.Parse(region.Identifier));
                        if (currentCardGeofence != null && !currentCardGeofence.Card.IsTreasureHuntItem && CanRaiseRegionEntered(currentCardGeofence))
                        {
                            currentCardGeofence.Card.LastListeningDate = DateTime.Now;
                            if (!_regionEnteredCards.ContainsKey(currentCardGeofence.Card._id))
                                _regionEnteredCards.Add(currentCardGeofence.Card._id, currentCardGeofence.Card);

                            currentCardGeofence.Playing = true;
#if __DEBUG__
                            Console.WriteLine("MonitorLocation founded ");
#endif
                            if (Context.Mode == Mode.FOREGROUND)
                            {
                                currentCardGeofence.StartPlayingMode = Mode.FOREGROUND;
                                RegionEntered(this, currentCardGeofence);
                            }
                            else if (Context.Mode == Mode.BACKGROUND)
                            {
#if __DEBUG__
                                Console.WriteLine("MonitorLocation PlayInBackgroundMode ");
#endif
                                currentCardGeofence.StartPlayingMode = Mode.BACKGROUND;
                                PlayInBackgroundMode(currentCardGeofence);
                            }

                            break;
                        }
                        else
                        {
                            if (currentCardGeofence == null)
                            {

                            }
                        }
                    }
                }
            }
        }

        private void GeofenceRegion_OnStatusChanged(object sender, Entities.GeofenceRegion e)
        {
            OnRegionStatusChanged(this, e);
        }

        private void PlayInBackgroundMode(Entities.GeofenceCard currentCardGeofence)
        {
            Context.PlayerManager.TryToPlayInBackground(currentCardGeofence.Card);
        }

        private bool CanRaiseRegionEntered(Entities.GeofenceCard currentCardGeofence)
        {
            if (_regionEnteredCards.ContainsKey(currentCardGeofence.Card._id))
            {
                var tmpCard = _regionEnteredCards[currentCardGeofence.Card._id];
                if (tmpCard.LastListeningDate.HasValue)
                {
                    if (tmpCard.LastListeningDate.Value.Date == DateTime.Today)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void SetForegroundMode()
        {
            //_locationManager.LocationsUpdated -= BackgroundLocationsUpdated;
            lock (_lock)
            {
                if (_currentCardsGeofence != null)
                {
                    var currentCardGeofence = _currentCardsGeofence.FirstOrDefault(x => x.Playing);
                    if (currentCardGeofence != null)
                    {
                        currentCardGeofence.StartPlayingMode = Mode.FOREGROUND;
                        RegionEntered(this, currentCardGeofence);
                    }
                }
            }

            StartTacking(_cards);
        }

        public void SetBackgroundMode()
        {
#if __DEBUG__
            Console.WriteLine("SetBackgroundMode");
#endif
            lock (_lock)
            {
                if (_isUpdatingLocationEnabled && !Context.IsInTreasureHuntMode)
                {
                    _locationManager.StopUpdatingLocation();
                    _isUpdatingLocationEnabled = false;
                }

                if (_currentCardsGeofence != null)
                {
                    var currentCardGeofence = _currentCardsGeofence.FirstOrDefault(x => x.Playing);
                    if (currentCardGeofence != null)
                    {
                        currentCardGeofence.StartPlayingMode = Mode.BACKGROUND;
                    }
                }
            }
            //Context.PlayerManager.OnFinishPlaying -= PlayerManager_OnFinishPlaying;
        }

        public void SetCardToPlayed(int cardId)
        {
            lock (_lock)
            {
                if (_currentCardsGeofence != null)
                {
                    //var tmp = _currentCardsGeofence.FirstOrDefault(x => x.Card._id == cardId);
                    //if (tmp != null)
                    //    tmp.Playing = false;

                    //per sicurezza le cancello tutte
                    foreach (var item in _currentCardsGeofence.Where(x => x.Card._id == cardId))
                    {
                        item.Playing = false;
                    }
                }
            }
        }

        public void StopTracking()
        {
            lock (_lock)
            {
                _currentLocation = null;
                _currentHeading = null;
                if (_currentCardsGeofence != null)
                    _currentCardsGeofence.RemoveAll(x => !x.Card.IsTreasureHuntItem);
                _cards = null;

                if (_isUpdatingLocationEnabled)
                {
                    _locationManager.StopUpdatingLocation();
                    _locationManager.StopUpdatingHeading();
                    _isUpdatingLocationEnabled = false;
                }
            }

            _locationManager.LocationsUpdated -= ForegroundLocationsUpdated;
            _locationManager.UpdatedHeading -= LocationManager_UpdatedHeading;
            Context.PlayerManager.OnFinishPlaying -= PlayerManager_OnFinishPlaying;
        }

        public void StopTreasureHuntTracking()
        {
            lock (_lock)
            {
                if (_currentCardsGeofence != null)
                    _currentCardsGeofence.RemoveAll(x => x.Card.IsTreasureHuntItem);

                if (_geofenceRegions != null)
                    _geofenceRegions.RemoveAll(x => x.GeofenceCard.Card.IsTreasureHuntItem);
            }
        }

        public void Terminate()
        {
            //if (_timer != null)
            //    _timer.Dispose();
            lock (_lock)
            {
                _currentCardsGeofence = null;
                _cards = null;

                if (_isUpdatingLocationEnabled)
                {
                    _locationManager.StopUpdatingLocation();
                    _locationManager.StopUpdatingHeading();
                    _isUpdatingLocationEnabled = false;
                }

                if (_isSignificantUpdatingLocationEnabled)
                {
                    _locationManager.StopMonitoringSignificantLocationChanges();
                    _isSignificantUpdatingLocationEnabled = false;
                }

                _locationManager.LocationsUpdated -= ForegroundLocationsUpdated;
                _locationManager.LocationsUpdated -= BackgroundLocationsUpdated;
                _locationManager.UpdatedHeading -= LocationManager_UpdatedHeading;
                Context.PlayerManager.OnFinishPlaying -= PlayerManager_OnFinishPlaying;
            }
        }

        public void Clear()
        {
            _regionEnteredCards.Clear();
        }
    }

    public class LocationUpdatedEventArgs : EventArgs
    {
        CLLocation location;

        public CLLocation Location
        {
            get { return location; }
        }

        public LocationUpdatedEventArgs(CLLocation location)
        {
            this.location = location;
        }
    }
}
