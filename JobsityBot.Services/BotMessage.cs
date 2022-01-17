namespace Jobsity;

public class BotMessage
{
    public BotMessage(string text, int roomId)
    {
        Text = text;
        RoomId = roomId;
    }

    public string Text { get; set; }
    public int RoomId { get; set; }
}

