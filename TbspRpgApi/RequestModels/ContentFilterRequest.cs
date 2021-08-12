namespace TbspRpgApi.RequestModels
{
    public class ContentFilterRequest
    {
        public string Direction { get; set; }

        public long? Start { get; set; }

        public long? Count { get; set; }
    }
}