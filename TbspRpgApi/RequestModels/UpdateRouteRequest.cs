using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class UpdateRouteRequest
    {
        public RouteViewModel route { get; set; }
        public SourceViewModel source { get; set; }
        public SourceViewModel successSource { get; set; }
        public string newDestinationLocationName { get; set; }

        public RouteUpdateModel ToRouteUpdateModel()
        {
            return new RouteUpdateModel()
            {
                route = route.ToEntity(),
                source = source.ToEntity(),
                successSource = successSource.ToEntity(),
                newDestinationLocationName = newDestinationLocationName,
                language = source.Language
            };
        }
    }
}

// {
//     "route": {
//         "id": "8fdf762b-2896-4520-8eb8-a0e2c0c245b6",
//         "name": "demo_start_routeone",
//         "sourceKey": "00000000-0000-0000-0000-000000000000",
//         "successSourceKey": "00000000-0000-0000-0000-000000000000",
//         "locationId": "fd84b608-72de-4fe0-b725-4e46ae0bb72e",
//         "destinationLocationId": "9786b18d-f7cf-42f1-b681-466663ae51da",
//         "newDestinationLocationName": "xxx"
//     },
//     "source": {
//         "id": "09a184ef-1215-431e-9a59-6ab722e35bfa",
//         "key": "00000000-0000-0000-0000-000000000000",
//         "adventureId": "00000000-0000-0000-0000-000000000000",
//         "text": "Empty Source",
//         "language": ""
//     },
//     "successSource": {
//         "id": "09a184ef-1215-431e-9a59-6ab722e35bfa",
//         "key": "00000000-0000-0000-0000-000000000000",
//         "adventureId": "00000000-0000-0000-0000-000000000000",
//         "text": "Empty Source",
//         "language": ""
//     }
// }