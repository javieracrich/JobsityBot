namespace JobsityBot.Services;

public class QueueMessage
{
    public QueueMessage(string text, int roomId)
    {
        Text = text;
        RoomId = roomId;
    }

    public string Text { get; set; }
    public int RoomId { get; set; }
}

