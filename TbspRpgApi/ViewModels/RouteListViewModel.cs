using System.Collections.Generic;

namespace TbspRpgApi.ViewModels;

public class RouteListViewModel
{
    public List<RouteViewModel> Routes { get; set; }
    public LocationViewModel Location { get; set; }
}