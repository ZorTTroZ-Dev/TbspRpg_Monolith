using TbspRpgApi.Entities;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Adapters
{
    public static class SourceAdapter
    {
        public static Source ToEntity(SourceViewModel viewModel)
        {
            return new Source()
            {
                Id = viewModel.Id,
                Key = viewModel.Key,
                AdventureId = viewModel.AdventureId,
                Text = viewModel.Text
            };
        }
    }
}