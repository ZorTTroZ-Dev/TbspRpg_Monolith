namespace TbspRpgDataLayer.ArgumentModels
{
    public class ContentFilterRequest
    {
        public string Direction { get; set; }
        public long? Start { get; set; }
        public long? Count { get; set; }

        public bool IsForward() {
            return !string.IsNullOrEmpty(Direction) && Direction.ToLower()[0] == 'f';
        }
        
        public bool IsBackward() {
            return !string.IsNullOrEmpty(Direction) && Direction.ToLower()[0] == 'b';
        }
    }
}