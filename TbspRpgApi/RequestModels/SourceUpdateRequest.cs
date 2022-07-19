using System;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels;

public class SourceUpdateRequest
{
    public SourceViewModel Source { get; set; }
}