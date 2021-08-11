using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Services;

namespace TbspRpgDataLayer.Tests
{
    public static class MockServices
    {
        public static IUsersService MockDataLayerUsersService(IEnumerable<User> users)
        {
            var usersService = new Mock<IUsersService>();
            
            usersService.Setup(service =>
                service.Authenticate(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync((string userName, string password) =>
            {
                return users.FirstOrDefault(user => user.UserName == userName && user.Password == password);
            });
            
            usersService.Setup(service =>
                service.GetById(It.IsAny<Guid>())
            ).ReturnsAsync((Guid userId) =>
            {
                return users.FirstOrDefault(user => user.Id == userId);
            });

            return usersService.Object;
        }

        public static IAdventuresService MockDataLayerAdventuresService(IEnumerable<Adventure> adventures)
        {
            var adventuresService = new Mock<IAdventuresService>();

            adventuresService.Setup(service =>
                    service.GetAllAdventures())
                .ReturnsAsync(adventures.ToList());
            
            adventuresService.Setup(service =>
                    service.GetAdventureByName(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                    adventures.FirstOrDefault(a => a.Name == name));
            
            adventuresService.Setup(service =>
                    service.GetAdventureById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid Id) =>
                    adventures.FirstOrDefault(a => a.Id == Id));

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

            return gamesService.Object;
        }

        public static ILocationsService MockDataLayerLocationsService(IEnumerable<Location> locations)
        {
            var locationsService = new Mock<ILocationsService>();
            
            locationsService.Setup(service =>
                    service.GetInitialLocationForAdventure(It.IsAny<Guid>()))
                .ReturnsAsync((Guid adventureId) =>
                    locations.FirstOrDefault(l => l.AdventureId == adventureId && l.Initial));

            return locationsService.Object;
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
                        OrderBy(c => c.Position).
                        FirstOrDefault(c => c.GameId == gameId));
            
            contentsService.Setup(service =>
                    service.GetContentForGameAfterPosition(It.IsAny<Guid>(), It.IsAny<ulong>()))
                .ReturnsAsync((Guid gameId, ulong position) => 
                    contents.
                        OrderBy(c => c.Position).
                        Where(c => c.GameId == gameId && c.Position > position).ToList());

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
                    
                    return contents.
                        OrderBy(c => c.Position).
                        Skip(start).
                        Take(count).
                        Where(c => c.GameId == gameId).ToList();
                });

            return contentsService.Object;
        }
    }
}