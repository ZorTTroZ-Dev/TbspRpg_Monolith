using System;

namespace TbspRpgApi.RequestModels
{
    public class SourceFilterRequest
    {
        public Guid? Key { get; set; }
        public string Language { get; set; }
    }
}