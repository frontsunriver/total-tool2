using System;
using DT.Xamarin.Agora;

namespace WoWonder.Activities.Live.Rtc
{
    public interface IEventHandler
    { 
        void OnFirstLocalVideoFrame(int width, int height, int elapsed);
        void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed);

        void OnLeaveChannel(IRtcEngineEventHandler.RtcStats stats);

        void OnJoinChannelSuccess(String channel, int uid, int elapsed);

        void OnUserOffline(int uid, int reason);

        void OnUserJoined(int uid, int elapsed);

        void OnLastmileQuality(int quality);

        void OnLastmileProbeResult(IRtcEngineEventHandler.LastmileProbeResult result);

        void OnLocalVideoStats(IRtcEngineEventHandler.LocalVideoStats stats);

        void OnRtcStats(IRtcEngineEventHandler.RtcStats stats);

        void OnNetworkQuality(int uid, int txQuality, int rxQuality);

        void OnRemoteVideoStats(IRtcEngineEventHandler.RemoteVideoStats stats);

        void OnRemoteAudioStats(IRtcEngineEventHandler.RemoteAudioStats stats);

    }
}