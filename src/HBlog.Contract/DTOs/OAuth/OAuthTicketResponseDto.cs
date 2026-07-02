using System.Text.Json.Serialization;

namespace HBlog.Contract.DTOs.OAuth;

public class OAuthTicketResponseDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "needs_profile";

    [JsonPropertyName("oauthTicket")]
    public string OAuthTicket { get; set; } = string.Empty;
}
