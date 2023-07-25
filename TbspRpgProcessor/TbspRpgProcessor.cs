using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;
using TbspRpgSettings;

namespace TbspRpgProcessor;

public interface ITbspRpgProcessor
{
    #region UserProcessor
    
    Task<User> RegisterUser(UserRegisterModel userRegisterModel);
    Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel);
    Task<User> ResendUserRegistration(UserRegisterResendModel userRegisterResendModel);
    
    #endregion

    #region SourceProcessor

    Task<Source> CreateOrUpdateSource(SourceCreateOrUpdateModel sourceCreateOrUpdateModel);
    Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel);
    Task<Guid> ResolveSourceKey(SourceForKeyModel sourceForKeyModel);
    Task<List<Source>> GetUnreferencedSources(UnreferencedSourceModel unreferencedSourceModel);
    Task RemoveSource(SourceRemoveModel sourceRemoveModel);

    #endregion
    
    #region ScriptProcessor
    
    Task<string> ExecuteScript(ScriptExecuteModel scriptExecuteModel);
    Task RemoveScript(ScriptRemoveModel scriptIdRemoveModel);
    Task UpdateScript(ScriptUpdateModel scriptUpdateModel);
    
    #endregion

    #region RouteProcessor

    Task UpdateRoute(RouteUpdateModel routeUpdateModel);
    Task RemoveRoute(RouteRemoveModel routeRemoveModel);
    Task RemoveRoutes(RoutesRemoveModel routesRemoveModel);

    #endregion

    #region MapProcessor

    Task ChangeLocationViaRoute(MapChangeLocationModel mapChangeLocationModel);

    #endregion

    #region LocationProcessor

    Task UpdateLocation(LocationUpdateModel locationUpdateModel);
    Task RemoveLocation(LocationRemoveModel locationRemoveModel);
    Task RemoveLocations(LocationsRemoveModel locationsRemoveModel);

    #endregion

    #region GameProcessor

    Task<Game> StartGame(GameStartModel gameStartModel);
    Task RemoveGame(GameRemoveModel gameRemoveModel);
    Task RemoveGames(GamesRemoveModel gamesRemoveModel);

    #endregion

    #region ContentProcessor

    Task<string> GetContentTextForKey(ContentTextForKeyModel contentTextForKeyModel);

    #endregion
    
    #region AdventureProcessor
    
    Task UpdateAdventure(AdventureUpdateModel adventureUpdateModel);
    Task RemoveAdventure(AdventureRemoveModel adventureRemoveModel);
    
    #endregion
    
    #region AdventureObjectProcessor

    Task RemoveAdventureObject(AdventureObjectRemoveModel adventureObjectRemoveModel);
    Task UpdateAdventureObject(AdventureObjectUpdateModel adventureObjectUpdateModel);

    #endregion
}

public class TbspRpgProcessor: ITbspRpgProcessor
{
    private IUserProcessor _userProcessor;
    private ISourceProcessor _sourceProcessor;
    private IScriptProcessor _scriptProcessor;
    private IRouteProcessor _routeProcessor;
    private IMapProcessor _mapProcessor;
    private ILocationProcessor _locationProcessor;
    private IGameProcessor _gameProcessor;
    private IContentProcessor _contentProcessor;
    private IAdventureProcessor _adventureProcessor;
    private IAdventureObjectProcessor _adventureObjectProcessor;

    private readonly IUsersService _usersService;
    private readonly ISourcesService _sourcesService;
    private readonly IScriptsService _scriptsService;
    private readonly IAdventuresService _adventuresService;
    private readonly IRoutesService _routesService;
    private readonly ILocationsService _locationsService;
    private readonly IGamesService _gamesService;
    private readonly IContentsService _contentsService;
    private readonly IAdventureObjectService _adventureObjectService;
    
    private readonly TbspRpgUtilities _tbspRpgUtilities;

    private readonly IMailClient _mailClient;
    
    private readonly ILogger<TbspRpgProcessor> _logger;

    #region Constructor

    public TbspRpgProcessor(
        IUsersService usersService,
        ISourcesService sourcesService,
        IScriptsService scriptsService,
        IAdventuresService adventuresService,
        IRoutesService routesService,
        ILocationsService locationsService,
        IGamesService gamesService,
        IContentsService contentsService,
        IAdventureObjectService adventureObjectService,
        TbspRpgUtilities tbspRpgUtilities,
        IMailClient mailClient,
        ILogger<TbspRpgProcessor> logger)
    {
        _usersService = usersService;
        _sourcesService = sourcesService;
        _scriptsService = scriptsService;
        _adventuresService = adventuresService;
        _routesService = routesService;
        _locationsService = locationsService;
        _gamesService = gamesService;
        _contentsService = contentsService;
        _adventureObjectService = adventureObjectService;
        _tbspRpgUtilities = tbspRpgUtilities;
        _mailClient = mailClient;
        _logger = logger;
    }

    #endregion

    #region UserProcessor

    private void LoadUserProcessor()
    {
        _userProcessor ??= new UserProcessor(_usersService, _mailClient, _logger);
    }

    public Task<User> RegisterUser(UserRegisterModel userRegisterModel)
    {
        LoadUserProcessor();
        return _userProcessor.RegisterUser(userRegisterModel);
    }

    public Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel)
    {
        LoadUserProcessor();
        return _userProcessor.VerifyUserRegistration(userVerifyRegisterModel);
    }

    public Task<User> ResendUserRegistration(UserRegisterResendModel userRegisterResendModel)
    {
        LoadUserProcessor();
        return _userProcessor.ResendUserRegistration(userRegisterResendModel);
    }

    #endregion

    #region SourceProcessor

    private void LoadSourceProcessor()
    {
        LoadScriptProcessor();
        _sourceProcessor ??= new SourceProcessor(
            _scriptProcessor,
            _sourcesService,
            _adventuresService,
            _locationsService,
            _routesService,
            _contentsService,
            _scriptsService,
            _tbspRpgUtilities,
            _logger);
    }

    public Task<Source> CreateOrUpdateSource(SourceCreateOrUpdateModel sourceCreateOrUpdateModel)
    {
        LoadSourceProcessor();
        return _sourceProcessor.CreateOrUpdateSource(sourceCreateOrUpdateModel);
    }

    public Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel)
    {
        LoadSourceProcessor();
        return _sourceProcessor.GetSourceForKey(sourceForKeyModel);
    }

    public Task<Guid> ResolveSourceKey(SourceForKeyModel sourceForKeyModel)
    {
        LoadSourceProcessor();
        return _sourceProcessor.ResolveSourceKey(sourceForKeyModel);
    }

    public Task<List<Source>> GetUnreferencedSources(UnreferencedSourceModel unreferencedSourceModel)
    {
        LoadSourceProcessor();
        return _sourceProcessor.GetUnreferencedSources(unreferencedSourceModel);
    }
    
    public Task RemoveSource(SourceRemoveModel sourceRemoveModel)
    {
        LoadSourceProcessor();
        return _sourceProcessor.RemoveSource(sourceRemoveModel);
    }

    #endregion

    #region ScriptProcessor

    private void LoadScriptProcessor()
    {
        _scriptProcessor ??= new ScriptProcessor(
            _scriptsService,
            _adventuresService,
            _routesService,
            _locationsService,
            _sourcesService,
            _gamesService,
            _logger);
    }

    public Task<string> ExecuteScript(ScriptExecuteModel scriptExecuteModel)
    {
        LoadScriptProcessor();
        return _scriptProcessor.ExecuteScript(scriptExecuteModel);
    }

    public Task RemoveScript(ScriptRemoveModel scriptIdRemoveModel)
    {
        LoadScriptProcessor();
        return _scriptProcessor.RemoveScript(scriptIdRemoveModel);
    }

    public Task UpdateScript(ScriptUpdateModel scriptUpdateModel)
    {
        LoadScriptProcessor();
        return _scriptProcessor.UpdateScript(scriptUpdateModel);
    }

    #endregion

    #region RouteProcessor

    private void LoadRouteProcessor()
    {
        LoadSourceProcessor();
        _routeProcessor ??= new RouteProcessor(
            _sourceProcessor,
            _routesService,
            _locationsService,
            _logger);
    }
    
    public Task UpdateRoute(RouteUpdateModel routeUpdateModel)
    {
        LoadRouteProcessor();
        return _routeProcessor.UpdateRoute(routeUpdateModel);
    }

    public Task RemoveRoute(RouteRemoveModel routeRemoveModel)
    {
        LoadRouteProcessor();
        return _routeProcessor.RemoveRoute(routeRemoveModel);
    }

    public Task RemoveRoutes(RoutesRemoveModel routesRemoveModel)
    {
        LoadRouteProcessor();
        return _routeProcessor.RemoveRoutes(routesRemoveModel);
    }

    #endregion

    #region MapProcessor

    private void LoadMapProcessor()
    {
        LoadScriptProcessor();
        LoadSourceProcessor();
        _mapProcessor ??= new MapProcessor(
            _scriptProcessor,
            _sourceProcessor,
            _gamesService,
            _routesService,
            _contentsService,
            _logger);
    }
    
    public Task ChangeLocationViaRoute(MapChangeLocationModel mapChangeLocationModel)
    {
        LoadMapProcessor();
        return _mapProcessor.ChangeLocationViaRoute(mapChangeLocationModel);
    }

    #endregion

    #region LocationProcessor

    private void LoadLocationProcessor()
    {
        LoadSourceProcessor();
        _locationProcessor ??= new LocationProcessor(
            _sourceProcessor,
            _locationsService,
            _routesService,
            _logger);
    }

    public Task UpdateLocation(LocationUpdateModel locationUpdateModel)
    {
        LoadLocationProcessor();
        return _locationProcessor.UpdateLocation(locationUpdateModel);
    }

    public Task RemoveLocation(LocationRemoveModel locationRemoveModel)
    {
        LoadLocationProcessor();
        return _locationProcessor.RemoveLocation(locationRemoveModel);
    }

    public Task RemoveLocations(LocationsRemoveModel locationsRemoveModel)
    {
        LoadLocationProcessor();
        return _locationProcessor.RemoveLocations(locationsRemoveModel);
    }

    #endregion

    #region GameProcessor

    private void LoadGameProcessor()
    {
        LoadScriptProcessor();
        _gameProcessor ??= new GameProcessor(
            _scriptProcessor,
            _adventuresService,
            _usersService,
            _gamesService,
            _locationsService,
            _contentsService,
            _logger);
    }

    public Task<Game> StartGame(GameStartModel gameStartModel)
    {
        LoadGameProcessor();
        return _gameProcessor.StartGame(gameStartModel);
    }

    public Task RemoveGame(GameRemoveModel gameRemoveModel)
    {
        LoadGameProcessor();
        return _gameProcessor.RemoveGame(gameRemoveModel);
    }

    public Task RemoveGames(GamesRemoveModel gamesRemoveModel)
    {
        LoadGameProcessor();
        return _gameProcessor.RemoveGames(gamesRemoveModel);
    }

    #endregion

    #region ContentProcessor

    private void LoadContentProcessor()
    {
        LoadSourceProcessor();
        _contentProcessor ??= new ContentProcessor(_gamesService,
            _sourceProcessor, _logger);
    }
    
    public Task<string> GetContentTextForKey(ContentTextForKeyModel contentTextForKeyModel)
    {
        LoadContentProcessor();
        return _contentProcessor.GetContentTextForKey(contentTextForKeyModel);
    }

    #endregion
    
    #region AdventureProcessor

    private void LoadAdventureProcessor()
    {
        LoadSourceProcessor();
        LoadGameProcessor();
        LoadLocationProcessor();
        _adventureProcessor ??= new AdventureProcessor(
            _sourceProcessor,
            _gameProcessor,
            _locationProcessor,
            _adventuresService,
            _sourcesService,
            _scriptsService,
            _logger);
    }
    
    public Task UpdateAdventure(AdventureUpdateModel adventureUpdateModel)
    {
        LoadAdventureProcessor();
        return _adventureProcessor.UpdateAdventure(adventureUpdateModel);
    }

    public Task RemoveAdventure(AdventureRemoveModel adventureRemoveModel)
    {
        LoadAdventureProcessor();
        return _adventureProcessor.RemoveAdventure(adventureRemoveModel);
    }

    #endregion
    
    #region AdventureObjectProcessor

    private void LoadAdventureObjectProcessor()
    {
        _adventureObjectProcessor ??= new AdventureObjectProcessor(
            _adventureObjectService,
            _logger);
    }
    
    public Task RemoveAdventureObject(AdventureObjectRemoveModel adventureObjectRemoveModel)
    {
        LoadAdventureObjectProcessor();
        return _adventureObjectProcessor.RemoveAdventureObject(adventureObjectRemoveModel);
    }

    public Task UpdateAdventureObject(AdventureObjectUpdateModel adventureObjectUpdateModel)
    {
        LoadAdventureObjectProcessor();
        return _adventureObjectProcessor.UpdateAdventureObject(adventureObjectUpdateModel);
    }

    #endregion
}