using TbspRpgApi.Entities;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Adapters
{
    public static class LocationAdapter
    {
        public static Location ToEntity(LocationViewModel viewModel)
        {
            return new Location()
            {
                Id = viewModel.Id,
                AdventureId = viewModel.AdventureId,
                Initial = viewModel.Initial,
                Name = viewModel.Name,
                SourceKey = viewModel.SourceKey
            };
        }
    }
}