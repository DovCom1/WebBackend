using System.Text.Json.Serialization;

namespace WebBackend.Model.Dto.Calls
{
    public enum SignalingMessageType
    {
        [JsonPropertyName("room_join")]
        RoomJoin,

        [JsonPropertyName("room_leave")]
        RoomLeave,

        [JsonPropertyName("room_invite")]
        RoomInvite,

        [JsonPropertyName("participant_joined")]
        ParticipantJoined,

        [JsonPropertyName("participant_left")]
        ParticipantLeft,

        [JsonPropertyName("participant_state_update")]
        ParticipantStateUpdate,

        [JsonPropertyName("webrtc_offer")]
        WebRtcOffer,

        [JsonPropertyName("webrtc_answer")]
        WebRtcAnswer,

        [JsonPropertyName("webrtc_ice_candidate")]
        WebRtcIceCandidate,

        [JsonPropertyName("emotion_send")]
        EmotionSend,

        [JsonPropertyName("error")]
        Error
    }
}
