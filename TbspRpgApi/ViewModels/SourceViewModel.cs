using System;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class SourceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public Guid AdventureId { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
        
        public SourceViewModel() {}

        public SourceViewModel(Guid key, string text)
        {
            Key = key;
            Text = text;
        }

        public SourceViewModel(Source source, string language)
        {
            Id = source.Id;
            Name = source.Name;
            Key = source.Key;
            AdventureId = source.AdventureId;
            Text = source.Text;
            Language = language;
        }

        public Source ToEntity()
        {
            return new Source()
            {
                Id = Id,
                Name = Name,
                Key = Key,
                AdventureId = AdventureId,
                Text = Text
            };
        }
    }
}