using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Migrations;
using TbspRpgDataLayer.Services;
using TbspRpgSettings.Settings;

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
                    service.GetAdventureByIdIncludeAssociatedObjects(It.IsAny<Guid>()))
                .ReturnsAsync((Guid Id) =>
                    adventures.FirstOrDefault(a => a.Id == Id));

            adventuresService.Setup(service =>
                    service.AddAdventure(It.IsAny<Adventure>()))
                .Callback((Adventure adventure) =>
                {
                    adventure.Id = Guid.NewGuid();
                    adventures.Add(adventure);
                });
            
            adventuresService.Setup(service =>
                    service.RemoveAdventure(It.IsAny<Adventure>()))
                .Callback((Adventure adventure) => adventures.Remove(adventure));
            
            adventuresService.Setup(service =>
                    service.RemoveScriptFromAdventures(It.IsAny<Guid>()))
                .Callback((Guid scriptId) =>
                {
                    foreach (var adventure in adventures)
                    {
                        if (adventure.InitializationScriptId == scriptId)
                        {
                            adventure.InitializationScriptId = null;
                            adventure.InitializationScript = null;
                        }
                        if (adventure.TerminationScriptId == scriptId)
                        {
                            adventure.TerminationScriptId = null;
                            adventure.TerminationScript = null;
                        }
                    }
                });

            adventuresService.Setup(service =>
                    service.DoesAdventureUseSource(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId, Guid sourceKey) =>
                {
                    var advs = adventures.Where(a => a.Id == adventureId && (
                        a.DescriptionSourceKey == sourceKey || a.InitialSourceKey == sourceKey));
                    return advs.Any();
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
            
            gamesService.Setup(service =>
                    service.GetGameByIdIncludeAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid gameId) => games.FirstOrDefault(g => g.Id == gameId));

            // doesn't do any actual filtering
            gamesService.Setup(service =>
                    service.GetGames(It.IsAny<GameFilter>()))
                .ReturnsAsync((GameFilter filter) => games.ToList());
            
            gamesService.Setup(service =>
                    service.GetGamesIncludeUsers(It.IsAny<GameFilter>()))
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
                    service.GetLocationByIdWithRoutes(It.IsAny<Guid>()))
                .ReturnsAsync((Guid locationId) => locations.FirstOrDefault(l => l.Id == locationId));
            
            locationsService.Setup(service =>
                    service.GetLocationByIdWithObjects(It.IsAny<Guid>()))
                .ReturnsAsync((Guid locationId) => locations.FirstOrDefault(l => l.Id == locationId));
            
            locationsService.Setup(service =>
                    service.AddLocation(It.IsAny<Location>()))
                .Callback((Location location) =>
                {
                    location.Id = Guid.NewGuid();
                    locations.Add(location);
                });
            
            locationsService.Setup(service =>
                    service.RemoveLocation(It.IsAny<Location>()))
                .Callback((Location location) => locations.Remove(location));
            
            locationsService.Setup(service =>
                    service.RemoveScriptFromLocations(It.IsAny<Guid>()))
                .Callback((Guid scriptId) =>
                {
                    foreach (var location in locations)
                    {
                        if (location.EnterScriptId == scriptId)
                        {
                            location.EnterScript = null;
                            location.EnterScriptId = null;
                        }
                        if (location.ExitScriptId == scriptId)
                        {
                            location.ExitScript = null;
                            location.ExitScriptId = null;
                        }
                    }
                });
            
            locationsService.Setup(service =>
                    service.DoesAdventureLocationUseSource(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId, Guid sourceKey) =>
                {
                    var locs = locations.Where(a => a.AdventureId == adventureId && a.SourceKey == sourceKey);
                    return locs.Any();
                });

            return locationsService.Object;
        }
        
        public static IScriptsService MockDataLayerScriptsService(ICollection<Script> scripts)
        {
            var scriptsService = new Mock<IScriptsService>();

            scriptsService.Setup(service =>
                    service.GetScriptById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid scriptId) => scripts.FirstOrDefault(s => s.Id == scriptId));
            
            scriptsService.Setup(service =>
                    service.GetScriptsForAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId) => scripts.Where(s => s.AdventureId == adventureId).ToList());

            scriptsService.Setup(service =>
                    service.AddScript(It.IsAny<Script>()))
                .Callback((Script script) => scripts.Add(script));
            
            scriptsService.Setup(service =>
                    service.GetScriptWithIncludedIn(It.IsAny<Guid>()))
                .ReturnsAsync((Guid scriptId) => scripts.FirstOrDefault(s => s.Id == scriptId));
            
            scriptsService.Setup(service =>
                    service.RemoveScript(It.IsAny<Script>()))
                .Callback((Script script) => scripts.Remove(script));
            
            scriptsService.Setup(service =>
                    service.RemoveAllScriptsForAdventure(It.IsAny<Guid>()))
                .Callback((Guid adventureId) =>
                {
                    for (int i = scripts.Count - 1; i >= 0; i--)
                    {
                        // Do processing here, then...
                        if (scripts.ToArray()[i].AdventureId == adventureId)
                        {
                            scripts.Remove(scripts.ToArray()[i]);
                        }
                    }
                });
            
            scriptsService.Setup(service =>
                    service.IsSourceKeyReferenced(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId, Guid sourceKey) =>
                {
                    var locs = scripts.Where(a => a.AdventureId == adventureId && 
                                                   a.Content.Contains(sourceKey.ToString()));
                    return locs.Any();
                });
            
            return scriptsService.Object;
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
                    if (filter.AdventureId != null)
                        return routes.Where(r => r.Location.AdventureId == filter.AdventureId).ToList();
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
            
            routesService.Setup(service =>
                    service.RemoveRoutes(It.IsAny<ICollection<Route>>()))
                .Callback((ICollection<Route> routesToRemove) =>
                {
                    for (int i = routes.Count - 1; i >= 0; i--)
                    {
                        foreach (var route in routesToRemove)
                        {
                            if (routes.ToArray()[i].Id == route.Id)
                            {
                                routes.Remove(routes.ToArray()[i]);
                            }
                        }
                    }
                });
            
            routesService.Setup(service =>
                    service.RemoveScriptFromRoutes(It.IsAny<Guid>()))
                .Callback((Guid scriptId) =>
                {
                    foreach (var route in routes)
                    {
                        if (route.RouteTakenScriptId == scriptId)
                        {
                            route.RouteTakenScript = null;
                            route.RouteTakenScriptId = null;
                        }
                    }
                });
            
            routesService.Setup(service =>
                    service.DoesAdventureRouteUseSource(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId, Guid sourceKey) =>
                {
                    var locs = routes.Where(a => a.Location.AdventureId == adventureId && 
                                                 (a.SourceKey == sourceKey || a.RouteTakenSourceKey == sourceKey));
                    return locs.Any();
                });

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
            
            sourcesService.Setup(service =>
                    service.RemoveAllSourceForAdventure(It.IsAny<Guid>()))
                .Callback((Guid adventureId) =>
                {
                    for (int i = sources.Count - 1; i >= 0; i--)
                    {
                        // Do processing here, then...
                        if (sources.ToArray()[i].AdventureId == adventureId)
                        {
                            sources.Remove(sources.ToArray()[i]);
                        }
                    }
                });

            sourcesService.Setup(service =>
                    service.RemoveSource(It.IsAny<Guid>()))
                .Callback((Guid sourceId) =>
                {
                    for (int i = sources.Count - 1; i >= 0; i--)
                    {
                        // Do processing here, then...
                        if (sources.ToArray()[i].Id == sourceId)
                        {
                            sources.Remove(sources.ToArray()[i]);
                        }
                    }
                });
            
            sourcesService.Setup(service =>
                    service.GetSourceById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid sourceId) =>
                {
                    return sources.FirstOrDefault(en => en.Id == sourceId);
                });
            
            sourcesService.Setup(service =>
                    service.RemoveScriptFromSources(It.IsAny<Guid>()))
                .Callback((Guid scriptId) =>
                {
                    foreach (var en in sources)
                    {
                        if (en.ScriptId == scriptId)
                        {
                            en.Script = null;
                            en.ScriptId = null;
                        }
                    }
                });

            sourcesService.Setup(service =>
                    service.GetAllSourceForAdventure(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync((Guid adventureId, string language) =>
                {
                    var enSources = sources
                        .Where(source => source.AdventureId == adventureId)
                        .Select(enSource => new Source()
                    {
                        Id = enSource.Id,
                        AdventureId = enSource.AdventureId,
                        Key = enSource.Key,
                        Language = Languages.ENGLISH,
                        Name = enSource.Name,
                        ScriptId = enSource.ScriptId,
                        Text = enSource.Text
                    });
                    return enSources.ToList();
                });
            
            sourcesService.Setup(service =>
                    service.GetAllSourceAllLanguagesForAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId) =>
                    {
                        var enSources = sources
                            .Where(source => source.AdventureId == adventureId)
                            .Select(enSource => new Source()
                            {
                                Id = enSource.Id,
                                AdventureId = enSource.AdventureId,
                                Key = enSource.Key,
                                Language = Languages.ENGLISH,
                                Name = enSource.Name,
                                ScriptId = enSource.ScriptId,
                                Text = enSource.Text
                            });
                        return enSources.ToList();
                    }
                );

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
                    service.RemoveAllContentsForGame(It.IsAny<Guid>()))
                .Callback((Guid gameId) =>
                {
                    for (int i = contents.Count - 1; i >= 0; i--)
                    {
                        // Do processing here, then...
                        if (contents.ToArray()[i].GameId == gameId)
                        {
                            contents.Remove(contents.ToArray()[i]);
                        }
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
            
            contentsService.Setup(service =>
                    service.DoesAdventureContentUseSource(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId, Guid sourceKey) =>
                {
                    var locs = contents.Where(a => a.Game.AdventureId == adventureId && 
                                                 a.SourceKey == sourceKey);
                    return locs.Any();
                });

            return contentsService.Object;
        }
        
        public static IAdventureObjectService MockDataLayerAdventureObjectsService(ICollection<AdventureObject> objects)
        {
            var objectsService = new Mock<IAdventureObjectService>();
            
            objectsService.Setup(service =>
                    service.GetAdventureObjectsForAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId) =>
                    objects.Where(ao => ao.AdventureId == adventureId).ToList());
            
            objectsService.Setup(service =>
                    service.GetAdventureObjectsByLocation(It.IsAny<Guid>()))
                .ReturnsAsync((Guid locationId) =>
                    objects.Where(ao => ao.Locations.Any(location => location.Id == locationId)).ToList());

            objectsService.Setup(service =>
                    service.GetAdventureObjectById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid objectId) => 
                    objects.FirstOrDefault(ao => ao.Id == objectId));
            
            objectsService.Setup(service =>
                    service.RemoveAdventureObject(It.IsAny<AdventureObject>()))
                .Callback((AdventureObject adventureObject) => objects.Remove(adventureObject));
            
            objectsService.Setup(service =>
                    service.AddAdventureObject(It.IsAny<AdventureObject>()))
                .Callback((AdventureObject adventureObject) => objects.Add(adventureObject));

            return objectsService.Object;
        }
        
        public static IAdventureObjectSourceService MockDataLayerAdventureObjectsSourceService(
            ICollection<AdventureObject> objects, ICollection<En> sources)
        {
            var objectSourceService = new Mock<IAdventureObjectSourceService>();

            objectSourceService.Setup(service =>
                    service.GetAdventureObjectsWithSourceById(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync((List<Guid> objectIds, string language) =>
                {
                    var objs = objects.Where(obj => objectIds.Contains(obj.Id));
                    var objectSources = new List<AdventureObjectSource>();
                    foreach (var obj in objs)
                    {
                        var nameSource = sources.First(en => en.Key == obj.NameSourceKey);
                        var descSource = sources.First(en => en.Key == obj.DescriptionSourceKey);
                        objectSources.Add(new AdventureObjectSource()
                        {
                            AdventureObject = obj,
                            DescriptionSource = descSource,
                            NameSource = nameSource
                        });
                    }
                    return objectSources;
                });

            return objectSourceService.Object;
        }
    }
}