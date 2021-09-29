using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class SourceViewModel
    {
        public Guid Id { get; }
        public Guid Key { get; }
        public Guid AdventureId { get; }
        public string Text { get; }

        public SourceViewModel(string text)
        {
            Text = text;
        }

        public SourceViewModel(Source source)
        {
            Id = source.Id;
            Key = source.Key;
            AdventureId = source.AdventureId;
            Text = source.Text;
        }
    }
}