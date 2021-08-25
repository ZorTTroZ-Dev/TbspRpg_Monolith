using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class SourceViewModel
    {
        public string Text { get; }

        public SourceViewModel(string text)
        {
            Text = text;
        }
    }
}