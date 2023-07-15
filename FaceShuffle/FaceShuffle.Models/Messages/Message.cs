using FaceShuffle.Models.Session;

namespace FaceShuffle.Models.Messages;
public class Message
{
    public MessageId Id { get; set; }
    public UserSessionGuid Sender { get; set; }
    public UserSessionGuid Receiver { get; set; }
    public required MessageTextContent MessageTextContent { get; init; }
    public DateTime SentDate { get; init; }
    public DateTime? ViewedDate { get; init; }
}
