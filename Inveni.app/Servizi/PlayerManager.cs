using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using AVFoundation;
using System.Net;
using CoreGraphics;
using System.Threading.Tasks;
using Palmipedo.Models;
using MediaPlayer;

namespace Palmipedo.iOS.Core
{
    public enum AudioType
    {
        MP3,
        TTS
    }

    public class PlayerManager : NSObject
    {
        private static object _lock = new object();

        private AudioType _audioType;
        private string _currentDataAudio;
        private bool _isStoppableAudio;
        private Models.Scheda _currentScheda;
        private AVSpeechSynthesizer _speechSynthesizer;
        private AVSpeechUtterance _speechUtterance;

        private NSTimer _playerTimer;
        private AVPlayer _player;

        private NSObject _playToEndNotificationHandle;

        private UIButton _btnPlay;
        private UISlider _progressBar;
        private UILabel _lblCurrentTime;
        private UILabel _lblTotalTime;
        private UIView _containerView;

        private bool _uiControlsIsInitialized;
        private bool _playerIsInitialized;
        private bool _playerIsInPause;
        //private bool _playerIsReady;
        //private bool _playingFromLocationManager;
        private bool _trackEnded;
        private MPNowPlayingInfo _currentSong;

        public event EventHandler<Models.Scheda> OnFinishPlaying;
        //public event EventHandler<Models.Scheda> OnStartPlaying;

        private static PlayerManager instance;

        public bool IsInPause { get { return _playerIsInPause; } }

        //private static FLAnimatedImageView _loadingImageView;

        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerManager();
                }
                return instance;
            }
        }

        private PlayerManager()
        {
            //_audioQueue = new Queue<Models.Scheda>();

            //FLAnimatedImage loadingImage = FLAnimatedImage.AnimatedImageWithGIFData(NSData.FromUrl(NSUrl.FromString("http://www.luca.evaroni.name/Rolling.gif")));
            //_loadingImageView = new FLAnimatedImageView();
            //_loadingImageView.AnimatedImage = loadingImage;

            _speechSynthesizer = new AVSpeechSynthesizer();

            AddRemoteCommand();
        }

        public void InitUIControls(UIButton btnPlay, UISlider progressBar, UILabel lblCurrentTime, UILabel lblTotalTime, UIView view)
        {
            _btnPlay = btnPlay;
            _progressBar = progressBar;
            _lblCurrentTime = lblCurrentTime;
            _lblTotalTime = lblTotalTime;
            _containerView = view;

            _progressBar.Enabled = false;
            _lblCurrentTime.Enabled = false;
            _lblTotalTime.Enabled = false;

            _uiControlsIsInitialized = true;
        }

        public void Start(string data, AudioType type, Models.Scheda card)
        {
            if (_currentDataAudio != data && !string.IsNullOrEmpty(_currentDataAudio))
            {
                ResetPlayer();
            }

            //_playingFromLocationManager = false;
            _audioType = type;

            _isStoppableAudio = false;

            switch (type)
            {
                case AudioType.MP3:
                    StartMp3(data, card);
                    break;
                case AudioType.TTS:
                    StartTTS(data, card);
                    break;
                default:
                    break;
            }
        }

        private void StartMp3(string url, Models.Scheda card, bool isAuto = false)
        {
            if (!_uiControlsIsInitialized) throw new Exception("UIControls is not initialized");

            if (string.IsNullOrEmpty(_currentDataAudio) || url != _currentDataAudio)
            {
                //_playerIsReady = false;

                if (_player != null) ResetPlayerMP3();

                NSError error = AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                AVAudioSession.SharedInstance().SetActive(true);
                if (error == null)
                {
                    NSUrl soundUrl = new NSUrl(url);

                    if (!CheckUrl(url)) return;

                    _currentDataAudio = url;
                    _currentScheda = card;

                    AVAsset asset = AVAsset.FromUrl(soundUrl);
                    AVPlayerItem playerItem = new AVPlayerItem(asset);
                    AVPlayerLayer l = new AVPlayerLayer();
                    _player = new AVPlayer(playerItem);
                    _player.Volume = 1;
                    _player.Muted = false;
                    _btnPlay.SetImage(null, UIControlState.Normal);

                    SetPlayingInfo(card, 0);

                    //_loadingImageView.Frame = new CGRect(_btnPlay.Frame.X + 5, _btnPlay.Frame.Y + 5, 40, 40);
                    //_containerView.Add(_loadingImageView);

                    //_btnPlay.SetTitle("L", UIControlState.Normal);

                    _player.AddObserver(this, "status", NSKeyValueObservingOptions.New, IntPtr.Zero);

                    UnregisterPlayToEndNotification();
                    _playToEndNotificationHandle = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, DidPlayToEndTimeNotification);

                    _playerIsInPause = false;
                    _lblCurrentTime.Enabled = true;
                    _lblTotalTime.Enabled = true;
                    _progressBar.MaxValue = (float)_player.CurrentItem.Asset.Duration.Seconds;
                    _progressBar.Value = (float)_player.CurrentTime.Seconds;
                    _lblCurrentTime.Text = "0:00";
                    _lblTotalTime.Text = DateTime.Today.AddSeconds(_player.CurrentItem.Asset.Duration.Seconds).ToString("m:ss");

                    _progressBar.Enabled = false;
                    _progressBar.ValueChanged -= OnProgressBar_ValueChanged;
                    _progressBar.ValueChanged += OnProgressBar_ValueChanged;
                }
                else
                {
                    UIAlertView alert = new UIAlertView();
                    alert.Title = Context.GetLoacalizedValueByName("audio_error_title");
                    alert.AddButton("Ok");
                    alert.Message = error.LocalizedDescription != null ? error.LocalizedDescription : error.Description;
                    alert.Show();
                }

                Context.SendLog_InviaLogSchede(card.comune, card.catId, card.serviceId, card._id, url, !isAuto ? Models.InviaLogSchedeRequest.OPERAZIONE_BRANO_AUDIO : Models.InviaLogSchedeRequest.OPERAZIONE_BRANO_SCHEDA_AUTO);
            }
            else
            {
                PauseOrPlayMP3();
            }

            _playerIsInitialized = true;
        }

        private void OnProgressBar_ValueChanged(object sender1, EventArgs e1)
        {
            try
            {
                _playerTimer.Dispose();
                _player.Seek(CoreMedia.CMTime.FromSeconds(_progressBar.Value, 1));
                AddPlayerWatcher();
                _currentSong.ElapsedPlaybackTime = _player.CurrentTime.Seconds;

                if (_trackEnded && !_playerIsInPause)
                {
                    _player.Play();
                    SetPlayingInfo(_currentScheda, 1);
                }
                else if (_trackEnded)
                {
                    SetPlayingInfo(_currentScheda, 0);
                }

                if (!_playerIsInPause)
                {
                    SetPlayingInfo(_currentScheda, 1);
                }
                else
                {
                    SetPlayingInfo(_currentScheda, 0);
                }
            }
            catch (Exception) { }
        }

        private void StartTTS(string text, Models.Scheda card, bool isAuto = false)
        {
            if (!_uiControlsIsInitialized) throw new Exception("UIControls is not initialized");

            if (string.IsNullOrEmpty(text) || text != _currentDataAudio)
            {
                AVAudioSession audioSession = AVAudioSession.SharedInstance();
                NSError error = audioSession.SetCategory(AVAudioSessionCategory.Playback);
                if (error == null)
                {
                    _currentDataAudio = text;
                    _currentScheda = card;

                    _speechUtterance = new AVSpeechUtterance(text)
                    {
                        Rate = AVSpeechUtterance.DefaultSpeechRate,
                        Voice = AVSpeechSynthesisVoice.FromLanguage(Context.GetCurrentLangCultureFromLang(card.pathText)), //Context.CurrentLangCulture
                        Volume = 1,//AVAudioSession.SharedInstance().OutputVolume,
                        PitchMultiplier = 1.0f
                    };

                    _progressBar.Value = 0;
                    _progressBar.MaxValue = 100;
                    _progressBar.Enabled = false;
                    _lblCurrentTime.Enabled = false;
                    _lblTotalTime.Enabled = false;
                    _lblCurrentTime.Text = "0:00";
                    _lblTotalTime.Text = "0:00";

                    _playerIsInPause = false;

                    SetPlayingInfo(card, 1);

                    _speechSynthesizer.SpeakUtterance(_speechUtterance);
                    _btnPlay.SetImage(UIImage.FromBundle("Player_Pause"), UIControlState.Normal);
                    _speechSynthesizer.DidFinishSpeechUtterance += SpeechSynthesizer_DidFinishSpeechUtterance;

                    string textUrl = string.Format(IndexViewController.URL_BASE_CARD_TEXT, Uri.EscapeUriString(card.comune), card.pathText, Uri.EscapeUriString(card.text));
                    Context.SendLog_InviaLogSchede(card.comune, card.catId, card.serviceId, card._id, textUrl, !isAuto ? Models.InviaLogSchedeRequest.OPERAZIONE_BRANO_AUDIO : Models.InviaLogSchedeRequest.OPERAZIONE_BRANO_SCHEDA_AUTO);
                }
                else
                {
                    UIAlertView alert = new UIAlertView();
                    alert.Title = Context.GetLoacalizedValueByName("audio_error_title");
                    alert.AddButton("Ok");
                    alert.Message = error.LocalizedDescription != null ? error.LocalizedDescription : error.Description;
                    alert.Show();
                }
            }
            else
            {
                PauseOrPlayTTS();
            }

            _playerIsInitialized = true;
        }

        private void SpeechSynthesizer_DidFinishSpeechUtterance(object sender, AVSpeechSynthesizerUteranceEventArgs e)
        {
            OnFinishPlayingAudio();
        }

        public void PauseOrPlay()
        {
            switch (_audioType)
            {
                case AudioType.MP3:
                    PauseOrPlayMP3();
                    break;
                case AudioType.TTS:
                    PauseOrPlayTTS();
                    break;
                default:
                    break;
            }
        }

        private void PauseOrPlayMP3()
        {
            if (_player == null)
                return;

            if (_playerIsInPause)
            {
                this.InvokeOnMainThread(() =>
                {
                    _btnPlay.SetImage(UIImage.FromBundle("Player_Pause"), UIControlState.Normal);

                    if (_trackEnded)
                    {
                        var diff = (int)_progressBar.Value - (int)_player.CurrentItem.Asset.Duration.Seconds;
                        diff = Math.Abs(diff);

                        if (diff <= 1)
                        {
                            _player.Seek(CoreMedia.CMTime.FromSeconds(0, 1));
                            AddPlayerWatcher();
                        }
                        else
                        {
                            _player.Seek(CoreMedia.CMTime.FromSeconds(_progressBar.Value, 1));
                        }
                        _trackEnded = false;
                    }

                    _player.Play();
                    SetPlayingInfo(_currentScheda, 1);
                    _playerIsInPause = false;

                });
            }
            else
            {
                if (string.IsNullOrEmpty(_currentDataAudio) && !CheckUrl(_currentDataAudio)) return;

                this.InvokeOnMainThread(() =>
                {
                    _btnPlay.SetImage(UIImage.FromBundle("Player_Play"), UIControlState.Normal);
                    _player.Pause();
                    _playerIsInPause = true;
                    SetPlayingInfo(_currentScheda, 0);
                });
            }

        }

        private void PauseOrPlayTTS()
        {
            if (_speechSynthesizer == null || _speechUtterance == null)
                return;

            if (_playerIsInPause)
            {
                this.InvokeOnMainThread(() =>
                {
                    _btnPlay.SetImage(UIImage.FromBundle("Player_Pause"), UIControlState.Normal);
                    _speechSynthesizer.ContinueSpeaking();
                    _playerIsInPause = false;
                    SetPlayingInfo(_currentScheda, 1);
                });
            }
            else
            {
                if (string.IsNullOrEmpty(_currentDataAudio)) return;

                this.InvokeOnMainThread(() =>
                {
                    _btnPlay.SetImage(UIImage.FromBundle("Player_Play"), UIControlState.Normal);
                    _speechSynthesizer.PauseSpeaking(AVSpeechBoundary.Immediate);
                    _playerIsInPause = true;
                    SetPlayingInfo(_currentScheda, 0);
                });
            }
        }

        public void ResetPlayer()
        {
            switch (_audioType)
            {
                case AudioType.MP3:
                    ResetPlayerMP3();
                    break;
                case AudioType.TTS:
                    ResetPlayerTTS();
                    break;
                default:
                    break;
            }

            if (_currentScheda != null)
            {
                Context.LocationManager.SetCardToPlayed(_currentScheda._id);
            }

            if (_playerIsInitialized)
            {
                _btnPlay.SetImage(UIImage.FromBundle("Player_Play"), UIControlState.Normal);
                _lblTotalTime.Text = "0:00";
                _lblCurrentTime.Text = "0:00";
                _progressBar.Value = 0;
                _progressBar.ValueChanged -= OnProgressBar_ValueChanged;
            }

            var tmpScheda = _currentScheda;

            _playerIsInPause = false;
            //_playerIsReady = false;
            _trackEnded = false;
            //_playingFromLocationManager = false;
            _currentDataAudio = null;
            _currentScheda = null;
            _isStoppableAudio = false;

            SetPlayingInfo(null, 0);

            if (OnFinishPlaying != null)
            {
                OnFinishPlaying.Invoke(new object(), tmpScheda);
            }
        }

        private void ResetPlayerMP3()
        {
            if (_playerTimer != null)
            {
                _playerTimer.Dispose();
            }

            if (_player != null)
            {
                try
                {
                    _player.RemoveObserver(this, "status");
                }
                catch (Exception)
                {

                }

                _player.Pause();
                _player = null;
            }

            UnregisterPlayToEndNotification();
        }

        private void ResetPlayerTTS()
        {
            if (_speechSynthesizer != null)
            {
                _speechSynthesizer.StopSpeaking(AVSpeechBoundary.Immediate);
                _speechSynthesizer.DidFinishSpeechUtterance -= SpeechSynthesizer_DidFinishSpeechUtterance;
            }

            if (_speechUtterance != null)
            {
                _speechUtterance.Dispose();
                _speechUtterance = null;
            }
        }

        private void UnregisterPlayToEndNotification()
        {
            try
            {
                if (_playToEndNotificationHandle != null)
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_playToEndNotificationHandle);
            }
            catch (Exception) { }
        }

        private void DidPlayToEndTimeNotification(NSNotification notification)
        {
            OnFinishPlayingAudio();
        }

        private void OnFinishPlayingAudio()
        {
            try
            {
                SetPlayingInfo(null, 0);

                _trackEnded = true;

                if (_playerTimer != null)
                    _playerTimer.Dispose();

                if (_audioType == AudioType.MP3)
                {
                    if (_player != null)
                    {
                        if (_lblCurrentTime != null)
                            _lblCurrentTime.Text = DateTime.Today.AddSeconds(_player.CurrentItem.Asset.Duration.Seconds).ToString("m:ss");
                    }
                    else
                    {
                        if (_lblCurrentTime != null)
                            _lblCurrentTime.Text = "0:00";
                    }
                }
                else if (_audioType == AudioType.TTS)
                {
                    if (_lblCurrentTime != null)
                        _lblCurrentTime.Text = "0:00";
                }

                if (_btnPlay != null)
                {
                    _btnPlay.SetImage(UIImage.FromBundle("Player_Play"), UIControlState.Normal);
                }

                if (_progressBar != null)
                {
                    _progressBar.ValueChanged -= OnProgressBar_ValueChanged;
                }

                if (_currentScheda != null)
                {
                    Context.LocationManager.SetCardToPlayed(_currentScheda._id);
                }

                UnregisterPlayToEndNotification();

                if (_player != null)
                {
                    try
                    {
                        _player.RemoveObserver(this, "status", IntPtr.Zero);
                    }
                    catch (Exception)
                    {

                    }
                }

                var tmpScheda = _currentScheda;
                _currentDataAudio = null;
                _currentScheda = null;
                //_playingFromLocationManager = false;
                _currentSong = null;
                _player = null;
                _speechUtterance = null;

                if (OnFinishPlaying != null)
                {
                    OnFinishPlaying.Invoke(new object(), tmpScheda);
                }
            }
            catch (Exception ex)
            {


            }
        }

        private bool CheckUrl(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                return true;
            }
            catch (WebException)
            {
                ToastIOS.Toast.MakeText(Context.GetLoacalizedValueByName("audio_not_found"))
                          .SetDuration((int)ToastIOS.ToastDuration.Normal)
                          .SetGravity(ToastIOS.ToastGravity.Bottom)
                          .Show();

                return false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        private void AddPlayerWatcher()
        {
            if (_playerTimer != null)
                _playerTimer.Dispose();

            _playerTimer = NSTimer.CreateRepeatingScheduledTimer(0.5, (NSTimer s) =>
            {
                if (_player != null)
                {
                    _progressBar.Value = (float)_player.CurrentTime.Seconds;
                    _lblCurrentTime.Text = DateTime.Today.AddSeconds(_player.CurrentTime.Seconds).ToString("m:ss");
                    if (_player.CurrentTime.Seconds <= 3)
                    {
                        if (!_playerIsInPause)
                        {
                            if (_player.CurrentTime.Seconds > 1)
                            {
                                SetPlayingInfo(_currentScheda, 1);
                            }
                        }
                        else
                            SetPlayingInfo(_currentScheda, 0);
                    }
                }
                else
                {
                    s.Dispose();
                }
            });
        }

        private void SetPlayingInfo(Models.Scheda card, double playbackRate)
        {
            if (card == null)
            {
                MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = null;
                return;
            }


            try
            {
                _currentSong = new MPNowPlayingInfo();

                if (_player != null)
                {
                    _currentSong.PlaybackDuration = _player.CurrentItem.Asset.Duration.Seconds;
                    _currentSong.ElapsedPlaybackTime = _player.CurrentItem.CurrentTime.Seconds;
                }
                else if (_speechUtterance != null)
                {
                    _currentSong.PlaybackDuration = null;
                    _currentSong.ElapsedPlaybackTime = null;
                }

                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    InvokeOnMainThread(async () =>
                    {
                        try
                        {
                            UIImage image = await Utils.GetUIImageFromUrlAsync(string.Format(IndexViewController.URL_BASE_PREVIEW_CARD, Uri.EscapeUriString(card.comune), Uri.EscapeUriString(card.photo1)));

                            _currentSong.PlaybackRate = playbackRate;

                            if (image != null)
                                _currentSong.Artwork = new MPMediaItemArtwork(image);

                            _currentSong.Artist = card.comune;
                            _currentSong.Title = card.name;
                            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = _currentSong;
                        }
                        catch (Exception ex)
                        {
                            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = null;
                            _currentSong = null;
                            Core.Context.Trace_TraceError(ex, true);
                        }
                    });
                })).Start();
            }
            catch (Exception ex)
            {
                MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = null;
                _currentSong = null;
                Core.Context.Trace_TraceError(ex, true);
            }
        }

        private void AddRemoteCommand()
        {
            NSError error;
            AVAudioSession.SharedInstance().SetMode(AVAudioSession.ModeDefault, out error);
            if (error == null)
            {
                MPRemoteCommandCenter.Shared.PauseCommand.Enabled = true;
                MPRemoteCommandCenter.Shared.PlayCommand.Enabled = true;
                MPRemoteCommandCenter.Shared.ChangePlaybackPositionCommand.Enabled = true;
                MPRemoteCommandCenter.Shared.PlayCommand.AddTarget(ToggledPlayButton);
                MPRemoteCommandCenter.Shared.PauseCommand.AddTarget(ToggledPauseButton);
                MPRemoteCommandCenter.Shared.ChangePlaybackPositionCommand.AddTarget(ChangePlaybackPositionCommand);
                UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();
            }
            else
            {
                //TODO
            }
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (Equals(ofObject, _player) && keyPath.Equals((NSString)"status"))
            {
                if (_player.Status == AVPlayerStatus.ReadyToPlay)
                {
                    _progressBar.Enabled = true;
                    //_playerIsReady = true;

                    //_loadingImageView.RemoveFromSuperview();

                    _btnPlay.SetImage(UIImage.FromBundle("Player_Pause"), UIControlState.Normal);

                    this.InvokeOnMainThread(() =>
                    {
                        _player.Play();
                        _lblCurrentTime.Text = DateTime.Today.AddSeconds(_player.CurrentTime.Seconds).ToString("m:ss");
                        AddPlayerWatcher();
                    });
                }
            }
        }

        public void Stop(Scheda card)
        {
            if (!_uiControlsIsInitialized) { return; }

            if (card == null) { return; }

            if (string.IsNullOrEmpty(_currentDataAudio)) { return; }

            if (_currentScheda._id == card._id)
            {
                ResetPlayer();
            }
        }

        public bool TryToPlay(Scheda card)
        {
            if (!_uiControlsIsInitialized) { return false; }

            if (card == null) { return false; }

            if (!string.IsNullOrEmpty(_currentDataAudio)) { return false; }

            //_playingFromLocationManager = true;
            _isStoppableAudio = false;

            if (!string.IsNullOrEmpty(card.audio))
            {
                string url = string.Format(IndexViewController.URL_BASE_AUDIO, Uri.EscapeUriString(card.comune), card.pathText, Uri.EscapeUriString(card.audio));

                if (CheckUrl(url))
                {
                    _audioType = AudioType.MP3;
                    StartMp3(url, card, true);
                }
                else
                {
                    Context.LocationManager.SetCardToPlayed(card._id);
                    OnFinishPlayingAudio();
                }
            }
            else
            {
                string textUrl = string.Format(IndexViewController.URL_BASE_CARD_TEXT, Uri.EscapeUriString(card.comune), card.pathText, Uri.EscapeUriString(card.text));
                string textSummary = Utils.GetStringFromUrlAsync(textUrl).Result;

                if (!string.IsNullOrEmpty(textSummary))
                {
                    _audioType = AudioType.TTS;
                    StartTTS(textSummary, card, true);
                }
                else
                {
                    Context.LocationManager.SetCardToPlayed(card._id);
                    OnFinishPlayingAudio();
                }
            }

            return true;
        }

        public bool TryToPlayHuntArea(Models.Scheda card)
        {
            if (!_uiControlsIsInitialized) { return false; }

            if (card == null) { return false; }

            if (!string.IsNullOrEmpty(_currentDataAudio)) { ResetPlayer(); }

            _isStoppableAudio = true;
            //_playingFromLocationManager = true;

            string audio = null;

            if (card.HuntType == HuntType.ADVANCED || card.HuntType == HuntType.INTERMEDIATE || card.EnigmaType == EnigmaType.INDIZIO)
            {
                audio = card.audio;
            }
            else if (card.EnigmaType == EnigmaType.ENIGMA)
            {
                audio = card.AreaEnigmaMp3;
            }

            if (!string.IsNullOrEmpty(audio))
            {
                string url = string.Format(IndexViewController.URL_BASE_AUDIO, Uri.EscapeUriString(card.comune), card.pathText, Uri.EscapeUriString(audio));

                if (CheckUrl(url))
                {
                    _audioType = AudioType.MP3;
                    StartMp3(url, card, true);
                }
                else
                {
                    Context.LocationManager.SetCardToPlayed(card._id);
                    //OnFinishPlayingAudio();
                }
            }

            return true;
        }

        public bool TryToPlayInBackground(Scheda card)
        {
            if (card == null) { return false; }

            if (!string.IsNullOrEmpty(_currentDataAudio)) { return false; }

            //_playingFromLocationManager = true;

            if (!string.IsNullOrEmpty(card.audio))
            {
                string url = string.Format(IndexViewController.URL_BASE_AUDIO, Uri.EscapeUriString(card.comune), card.pathText, Uri.EscapeUriString(card.audio));

                if (CheckUrl(url))
                {
                    _audioType = AudioType.MP3;

                    NSError error = AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                    AVAudioSession.SharedInstance().SetActive(true);
                    if (error == null)
                    {
                        NSUrl soundUrl = new NSUrl(url);

                        _currentDataAudio = url;
                        _currentScheda = card;

                        AVAsset asset = AVAsset.FromUrl(soundUrl);
                        AVPlayerItem playerItem = new AVPlayerItem(asset);
                        AVPlayerLayer l = new AVPlayerLayer();
                        _player = new AVPlayer(playerItem);
                        _player.Volume = 1;
                        _player.Muted = false;

                        UnregisterPlayToEndNotification();
                        _playToEndNotificationHandle = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, DidPlayToEndTimeNotification);

                        _player.Play();
                        SetPlayingInfo(card, 1);

                        Context.SendLog_InviaLogSchede(card.comune, card.catId, card.serviceId, card._id, url, Models.InviaLogSchedeRequest.OPERAZIONE_BRANO_SCHEDA_AUTO);
                    }
                    else
                    {
                        UIAlertView alert = new UIAlertView();
                        alert.Title = Context.GetLoacalizedValueByName("audio_error_title");
                        alert.AddButton("Ok");
                        alert.Message = error.LocalizedDescription != null ? error.LocalizedDescription : error.Description;
                        alert.Show();
                    }
                }
                else
                {
                    Context.LocationManager.SetCardToPlayed(card._id);
                    OnFinishPlayingAudio();
                }
            }
            else
            {
                string textUrl = string.Format(IndexViewController.URL_BASE_CARD_TEXT, Uri.EscapeUriString(card.comune), card.pathText, Uri.EscapeUriString(card.text));
                string text = Utils.GetStringFromUrlAsync(textUrl).Result;

                if (!string.IsNullOrEmpty(text))
                {
                    _audioType = AudioType.TTS;

                    NSError error = AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                    AVAudioSession.SharedInstance().SetActive(true);
                    if (error == null)
                    {
                        _currentDataAudio = text;
                        _currentScheda = card;

                        _speechUtterance = new AVSpeechUtterance(text)
                        {
                            Rate = AVSpeechUtterance.DefaultSpeechRate,
                            Voice = AVSpeechSynthesisVoice.FromLanguage(Context.GetCurrentLangCultureFromLang(card.pathText)), //Context.CurrentLangCulture
                            Volume = 1,//AVAudioSession.SharedInstance().OutputVolume,
                            PitchMultiplier = 1.0f
                        };

                        _playerIsInPause = false;

                        _speechSynthesizer.SpeakUtterance(_speechUtterance);
                        SetPlayingInfo(card, 1);

                        _speechSynthesizer.DidFinishSpeechUtterance += SpeechSynthesizer_DidFinishSpeechUtterance;

                        Context.SendLog_InviaLogSchede(card.comune, card.catId, card.serviceId, card._id, textUrl, Models.InviaLogSchedeRequest.OPERAZIONE_BRANO_SCHEDA_AUTO);
                    }
                    else
                    {
                        UIAlertView alert = new UIAlertView();
                        alert.Title = Context.GetLoacalizedValueByName("audio_error_title");
                        alert.AddButton("Ok");
                        alert.Message = error.LocalizedDescription != null ? error.LocalizedDescription : error.Description;
                        alert.Show();
                    }
                }
                else
                {
                    Context.LocationManager.SetCardToPlayed(card._id);
                    OnFinishPlayingAudio();
                }
            }

            return true;
        }

        public void SwitchPlayer()
        {
            if (_player != null && _currentSong != null && _uiControlsIsInitialized)
            {
                if (_audioType == AudioType.MP3)
                {
                    _lblCurrentTime.Enabled = true;
                    _lblTotalTime.Enabled = true;
                    _progressBar.Enabled = true;

                    _progressBar.MaxValue = (float)_player.CurrentItem.Asset.Duration.Seconds;
                    _progressBar.Value = (float)_player.CurrentTime.Seconds;
                    _lblCurrentTime.Text = DateTime.Today.AddSeconds(_player.CurrentTime.Seconds).ToString("m:ss");
                    _lblTotalTime.Text = DateTime.Today.AddSeconds(_player.CurrentItem.Asset.Duration.Seconds).ToString("m:ss");

                    _progressBar.ValueChanged -= OnProgressBar_ValueChanged;
                    _progressBar.ValueChanged += OnProgressBar_ValueChanged;

                    if (!_playerIsInPause)
                    {
                        _btnPlay.SetImage(UIImage.FromBundle("Player_Pause"), UIControlState.Normal);
                    }
                    else
                    {
                        _btnPlay.SetImage(UIImage.FromBundle("Player_Play"), UIControlState.Normal);
                    }
                }
            }
        }

        public bool CanPlay()
        {
            if (_isStoppableAudio) { return true; }

            if (!string.IsNullOrEmpty(_currentDataAudio))
            {
                return false;
            }

            return true;
        }

        [Export("ToggledPlayButton:")]
        public MPRemoteCommandHandlerStatus ToggledPlayButton(MPRemoteCommandEvent ev)
        {
            if (_currentSong == null) return MPRemoteCommandHandlerStatus.NoActionableNowPlayingItem;

            if (_player != null)
            {
                _player.Play();
                _currentSong.ElapsedPlaybackTime = _player.CurrentTime.Seconds;
            }
            else if (_speechSynthesizer != null)
            {
                try
                {
                    _speechSynthesizer.ContinueSpeaking();
                }
                catch (Exception) { }
            }

            _currentSong.PlaybackRate = 1.0;
            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = _currentSong;

            if (_btnPlay != null)
            {
                _btnPlay.SetImage(UIImage.FromBundle("Player_Pause"), UIControlState.Normal);
                _playerIsInPause = false;
            }

            //try
            //{
            //    AVAudioSession.SharedInstance().SetActive(true);
            //}
            //catch (Exception) { }

            return MPRemoteCommandHandlerStatus.Success;
        }

        [Export("ToggledPauseButton:")]
        public MPRemoteCommandHandlerStatus ToggledPauseButton(MPRemoteCommandEvent ev)
        {
            if (_currentSong == null) return MPRemoteCommandHandlerStatus.NoActionableNowPlayingItem;

            if (_player != null)
            {
                _player.Pause();
                _currentSong.ElapsedPlaybackTime = _player.CurrentTime.Seconds;
            }
            else if (_speechSynthesizer != null)
            {
                _speechSynthesizer.PauseSpeaking(AVSpeechBoundary.Immediate);
            }

            _currentSong.PlaybackRate = 0.0;
            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = _currentSong;

            if (_btnPlay != null)
            {
                _btnPlay.SetImage(UIImage.FromBundle("Player_Play"), UIControlState.Normal);
                _playerIsInPause = true;
            }

            //try
            //{
            //    AVAudioSession.SharedInstance().SetActive(false);
            //}
            //catch (Exception) { }

            return MPRemoteCommandHandlerStatus.Success;
        }

        [Export("ChangePlaybackPositionCommand:")]
        public MPRemoteCommandHandlerStatus ChangePlaybackPositionCommand(MPRemoteCommandEvent ev)
        {
            if (_player == null || _currentSong == null) return MPRemoteCommandHandlerStatus.NoActionableNowPlayingItem;

            var eventPlayback = (MPChangePlaybackPositionCommandEvent)ev;
            _player.Seek(CoreMedia.CMTime.FromSeconds(eventPlayback.PositionTime, 1));
            _currentSong.ElapsedPlaybackTime = eventPlayback.PositionTime;
            if (_progressBar != null)
            {
                _progressBar.Value = (float)_player.CurrentTime.Seconds;
            }
            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = _currentSong;

            return MPRemoteCommandHandlerStatus.Success;
        }
    }
}