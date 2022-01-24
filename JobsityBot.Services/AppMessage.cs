namespace Jobsity;

public class AppMessage
{
    public AppMessage(string stockCode, int roomId)
    {
        StockCode = stockCode;
        RoomId = roomId;
    }

    public string StockCode { get; }
    public int RoomId { get; set; }
}

