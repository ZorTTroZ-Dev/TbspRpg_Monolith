using System;
using TbspRpgApi.Entities;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Adapters
{
    public static class RouteAdapter
    {
        public static Route ToEntity(RouteViewModel viewModel)
        {
            return new Route()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                SourceKey = viewModel.SourceKey,
                SuccessSourceKey = viewModel.SuccessSourceKey,
                LocationId = viewModel.LocationId,
                DestinationLocationId = viewModel.DestinationLocationId
            };
        }
    }
}