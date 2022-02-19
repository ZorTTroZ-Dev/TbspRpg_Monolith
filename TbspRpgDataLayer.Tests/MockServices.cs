using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgDataLayer.Tests
{
    public static class MockServices
    {
        public static IUsersService MockDataLayerUsersService(ICollection<User> users)
        {
            var usersService = new Mock<IUsersService>();
            
            usersService.Setup(service =>
                service.Authenticate(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync((string userName, string password) =>
            {
                return users.FirstOrDefault(user => user.Email == userName && user.Password == password);
            });
            
            usersService.Setup(service =>
                service.GetById(It.IsAny<Guid>())
            ).ReturnsAsync((Guid userId) =>
            {
                return users.FirstOrDefault(user => user.Id == userId);
            });

            usersService.Setup(service =>
                service.GetUserByEmail(It.IsAny<string>())
            ).ReturnsAsync((string email) =>
            {
                return users.FirstOrDefault(user => user.Email == email);
            });

            usersService.Setup(service =>
                service.AddUser(It.IsAny<User>())
            ).Callback((User user) =>
            {
                user.Id = Guid.NewGuid();
                users.Add(user);
            });

            return usersService.Object;
        }

        public static IAdventuresService MockDataLayerAdventuresService(ICollection<Adventure> adventures)
        {
            var adventuresService = new Mock<IAdventuresService>();

            adventuresService.Setup(service =>
                    service.GetAllAdventures(It.IsAny<AdventureFilter>()))
                .ReturnsAsync(adventures.ToList());
            
            adventuresService.Setup(service =>
                    service.GetPublishedAdventures(It.IsAny<AdventureFilter>()))
                .ReturnsAsync(adventures.Where(adventure => adventure.PublishDate <= DateTime.UtcNow).ToList());
            
            adventuresService.Setup(service =>
                    service.GetAdventureByName(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                    adventures.FirstOrDefault(a => a.Name == name));
            
            adventuresService.Setup(service =>
                    service.GetAdventureById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid Id) =>
                    adventures.FirstOrDefault(a => a.Id == Id));

            adventuresService.Setup(service =>
                    service.AddAdventure(It.IsAny<Adventure>()))
                .Callback((Adventure adventure) =>
                {
                    adventure.Id = Guid.NewGuid();
                    adventures.Add(adventure);
                });

            return adventuresService.Object;
        }
        
        public static IGamesService MockDataLayerGamesService(ICollection<Game> games)
        {
            var gamesService = new Mock<IGamesService>();
            
            gamesService.Setup(service =>
                    service.GetGameByAdventureIdAndUserId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId, Guid userId) =>
                    games.FirstOrDefault(g => g.AdventureId == adventureId && g.UserId == userId));
            
            gamesService.Setup(service =>
                    service.AddGame(It.IsAny<Game>()))
                .Callback((Game game) => games.Add(game));

            gamesService.Setup(service =>
                    service.GetGameByIdIncludeLocation(It.IsAny<Guid>()))
                .ReturnsAsync((Guid gameId) => games.FirstOrDefault(g => g.Id == gameId));
            
            gamesService.Setup(service =>
                    service.GetGameById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid gameId) => games.FirstOrDefault(g => g.Id == gameId));

            // doesn't do any actual filtering
            gamesService.Setup(service =>
                    service.GetGames(It.IsAny<GameFilter>()))
                .ReturnsAsync((GameFilter filter) => games.ToList());

            gamesService.Setup(service =>
                    service.RemoveGame(It.IsAny<Game>()))
                .Callback((Game game) => games.Remove(game));

            return gamesService.Object;
        }

        public static ILocationsService MockDataLayerLocationsService(ICollection<Location> locations)
        {
            var locationsService = new Mock<ILocationsService>();
            
            locationsService.Setup(service =>
                    service.GetInitialLocationForAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId) =>
                    locations.FirstOrDefault(l => l.AdventureId == adventureId && l.Initial));
            
            locationsService.Setup(service =>
                    service.GetLocationsForAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId) =>
                    locations.Where(l => l.AdventureId == adventureId).ToList());

            locationsService.Setup(service =>
                    service.GetLocationById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid locationId) => locations.FirstOrDefault(l => l.Id == locationId));
            
            locationsService.Setup(service =>
                    service.AddLocation(It.IsAny<Location>()))
                .Callback((Location location) =>
                {
                    location.Id = Guid.NewGuid();
                    locations.Add(location);
                });

            return locationsService.Object;
        }

        public static IRoutesService MockDataLayerRoutesService(ICollection<Route> routes)
        {
            var routesService = new Mock<IRoutesService>();

            routesService.Setup(service =>
                    service.GetRoutesForLocation(It.IsAny<Guid>()))
                .ReturnsAsync((Guid locationId) => routes.Where(r => r.LocationId == locationId).ToList());

            routesService.Setup(service =>
                    service.GetRouteById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid routeId) => routes.FirstOrDefault(r => r.Id == routeId));

            routesService.Setup(service =>
                    service.GetRoutes(It.IsAny<RouteFilter>()))
                .ReturnsAsync((RouteFilter filter) =>
                {
                    if (filter.DestinationLocationId != null)
                        return routes.Where(r => r.DestinationLocationId == filter.DestinationLocationId).ToList();
                    if (filter.LocationId != null)
                        return routes.Where(r => r.LocationId == filter.LocationId).ToList();
                    return routes.ToList();
                });

            routesService.Setup(service =>
                    service.AddRoute(It.IsAny<Route>()))
                .Callback((Route route) =>
                {
                    route.Id = Guid.NewGuid();
                    routes.Add(route);
                });
            
            routesService.Setup(service =>
                    service.RemoveRoute(It.IsAny<Route>()))
                .Callback((Route route) => routes.Remove(route));

            return routesService.Object;
        }

        public static ISourcesService MockDataLayerSourcesService(ICollection<En> sources)
        {
            var sourcesService = new Mock<ISourcesService>();

            sourcesService.Setup(service =>
                    service.GetSourceTextForKey(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync((Guid key, string language) =>
                {
                    var source = sources.FirstOrDefault(s => s.Key == key);
                    return source != null ? source.Text : null;
                });
            
            sourcesService.Setup(service =>
                    service.GetSourceForKey(It.IsAny<Guid>(), It.IsAny<Guid>(),It.IsAny<string>()))
                .ReturnsAsync((Guid key, Guid adventureId, string language) =>
                {
                    En source = null;
                    if (key == Guid.Empty)
                        source = sources.FirstOrDefault(s => s.Key == key);
                    else
                        source = sources.FirstOrDefault(s => s.Key == key && s.AdventureId == adventureId);
                    return source;
                });
            
            sourcesService.Setup(service =>
                    service.AddSource(It.IsAny<Source>(), It.IsAny<string>()))
                .Callback((Source source, string language) => sources.Add(new En()
                {
                    Id = Guid.NewGuid(),
                    Key = source.Key,
                    AdventureId = source.AdventureId,
                    Name = source.Name,
                    Text = source.Text
                }));

            return sourcesService.Object;
        }

        public static IContentsService MockDataLayerContentsService(ICollection<Content> contents)
        {
            var contentsService = new Mock<IContentsService>();
            
            contentsService.Setup(service =>
                    service.GetContentForGameAtPosition(It.IsAny<Guid>(), It.IsAny<ulong>()))
                .ReturnsAsync((Guid gameId, ulong position) =>
                    contents.FirstOrDefault(c => c.GameId == gameId && c.Position == position));
            
            contentsService.Setup(service =>
                    service.AddContent(It.IsAny<Content>()))
                .Callback((Content content) => contents.Add(content));
            
            contentsService.Setup(service =>
                    service.GetAllContentForGame(It.IsAny<Guid>()))
                .ReturnsAsync((Guid gameId) => 
                    contents.
                        OrderBy(c => c.Position).
                        Where(c => c.GameId == gameId).ToList());
            
            contentsService.Setup(service =>
                    service.GetLatestForGame(It.IsAny<Guid>()))
                .ReturnsAsync((Guid gameId) => 
                    contents.
                        OrderByDescending(c => c.Position).
                        FirstOrDefault(c => c.GameId == gameId));
            
            contentsService.Setup(service =>
                    service.GetContentForGameAfterPosition(It.IsAny<Guid>(), It.IsAny<ulong>()))
                .ReturnsAsync((Guid gameId, ulong position) => 
                    contents.
                        OrderBy(c => c.Position).
                        Where(c => c.GameId == gameId && c.Position > position).ToList());

            contentsService.Setup(service =>
                    service.RemoveContents(It.IsAny<IEnumerable<Content>>()))
                .Callback((IEnumerable<Content> contentsToRemove) =>
                {
                    foreach (var ctr in contentsToRemove)
                    {
                        contents.Remove(ctr);
                    }
                });

            contentsService.Setup(service =>
                    service.GetPartialContentForGame(It.IsAny<Guid>(), It.IsAny<ContentFilterRequest>()))
                .ReturnsAsync((Guid gameId, ContentFilterRequest cfr) =>
                {
                    var start = 0;
                    var count = 0;
                    if (cfr.Start != null)
                        start = (int) cfr.Start.GetValueOrDefault();
                    if (cfr.Count != null)
                        count = (int) cfr.Count.GetValueOrDefault();
                    cfr.Direction ??= "f";
                    
                    if (cfr.IsBackward())
                    {
                        return contents.
                            OrderByDescending(c => c.Position).
                            Skip(start).
                            Take(count).
                            Where(c => c.GameId == gameId).ToList();
                    }

                    if (cfr.IsForward())
                    {
                        return contents.
                            OrderBy(c => c.Position).
                            Skip(start).
                            Take(count).
                            Where(c => c.GameId == gameId).ToList();
                    }

                    throw new ArgumentException("invalid direction argument");
                });

            return contentsService.Object;
        }
    }
}