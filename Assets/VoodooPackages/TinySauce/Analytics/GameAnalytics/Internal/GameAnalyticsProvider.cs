using System.Collections.Generic;
using GameAnalyticsSDK;

namespace Voodoo.Sauce.Internal.Analytics
{
    internal class GameAnalyticsProvider : IAnalyticsProvider
    {
        private const string TAG = "GameAnalyticsProvider";
        
        internal GameAnalyticsProvider()
        {
            RegisterEvents();
        }

        public void Initialize(bool consent)
        {
            if (!GameAnalyticsWrapper.Initialize(consent)) {
                UnregisterEvents();
            }
        }

        private static void RegisterEvents()
        {
            AnalyticsManager.OnGameStartedEvent += OnGameStarted;
            AnalyticsManager.OnGameFinishedEvent += OnGameFinished;
            AnalyticsManager.OnTrackCustomEvent += TrackCustomEvent;
            AnalyticsManager.OnInterstitialShowEvent += OnInterstitialShowEvent;
            AnalyticsManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
            AnalyticsManager.OnRewardedShowEvent += OnRewardedShowEvent;
            AnalyticsManager.OnRewardedClickedEvent += OnRewardedClickedEvent;
        }

        private static void UnregisterEvents()
        {
            AnalyticsManager.OnGameStartedEvent -= OnGameStarted;
            AnalyticsManager.OnGameFinishedEvent -= OnGameFinished;
            AnalyticsManager.OnTrackCustomEvent -= TrackCustomEvent;
            AnalyticsManager.OnInterstitialShowEvent -= OnInterstitialShowEvent;
            AnalyticsManager.OnInterstitialClickedEvent -= OnInterstitialClickedEvent;
            AnalyticsManager.OnRewardedShowEvent -= OnRewardedShowEvent;
            AnalyticsManager.OnRewardedClickedEvent -= OnRewardedClickedEvent;
        }
        
        
        
        private static void OnGameStarted(GameStartedParameters parameters)
        {
            GameAnalyticsWrapper.TrackProgressEvent(GAProgressionStatus.Start, parameters.level, null);
        }

        private static void OnGameFinished(GameFinishedParameters parameters)
        {
            GameAnalyticsWrapper.TrackProgressEvent(parameters.status ? GAProgressionStatus.Complete : GAProgressionStatus.Fail, parameters.level, (int) parameters.score);
        }

        private static void TrackCustomEvent(string eventName,
                                             Dictionary<string, object> eventProperties,
                                             string type,
                                             List<TinySauce.AnalyticsProvider> analyticsProviders)
        {
            if (analyticsProviders.Contains(TinySauce.AnalyticsProvider.GameAnalytics))
            {
                GameAnalyticsWrapper.TrackDesignEvent(eventName,null);
            }
        }
        
        
        
        private static void OnInterstitialShowEvent(AdShownEventAnalyticsInfo adAnalyticsInfo)
        {
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, adAnalyticsInfo.AdNetworkName, adAnalyticsInfo.adPlacement);
        }

        private static void OnInterstitialClickedEvent(AdClickEventAnalyticsInfo adAnalyticsInfo)
        {
            GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Interstitial, adAnalyticsInfo.AdNetworkName, adAnalyticsInfo.adPlacement);
        }
        
        
        private static void OnRewardedShowEvent(AdShownEventAnalyticsInfo adAnalyticsInfo)
        {
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, adAnalyticsInfo.AdNetworkName, adAnalyticsInfo.adPlacement);
        }

        private static void OnRewardedClickedEvent(AdClickEventAnalyticsInfo adAnalyticsInfo)
        {
            GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.RewardedVideo, adAnalyticsInfo.AdNetworkName, adAnalyticsInfo.adPlacement);
        }
        
    }
}