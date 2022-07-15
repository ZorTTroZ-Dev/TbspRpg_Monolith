using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor;

public interface ITbspRpgProcessor
{
    #region UserProcessor
    
    Task<User> RegisterUser(UserRegisterModel userRegisterModel);
    Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel);
    Task<User> ResendUserRegistration(UserRegisterResendModel userRegisterResendModel);
    
    #endregion

    #region SourceProcessor

    Task<Source> CreateOrUpdateSource(Source updatedSource, string language);
    Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel);
    Task<Guid> ResolveSourceKey(SourceForKeyModel sourceForKeyModel);

    #endregion
    
    #region ScriptProcessor
    
    Task<string> ExecuteScript(Guid scriptId, Guid gameId);
    Task<string> ExecuteScript(Guid scriptId, Game game);
    Task<string> ExecuteScript(Guid scriptId);
    string ExecuteScript(Script script, Game game);
    Task RemoveScript(ScriptRemoveModel scriptIdRemoveModel);
    Task UpdateScript(ScriptUpdateModel scriptUpdateModel);
    
    #endregion
}

public class TbspRpgProcessor: ITbspRpgProcessor
{
    private IUserProcessor _userProcessor;
    private ISourceProcessor _sourceProcessor;
    private IScriptProcessor _scriptProcessor;

    private readonly IUsersService _usersService;
    private readonly ISourcesService _sourcesService;
    private readonly IScriptsService _scriptsService;
    private readonly IAdventuresService _adventuresService;
    private readonly IRoutesService _routesService;
    private readonly ILocationsService _locationsService;
    private readonly IGamesService _gamesService;

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
        _sourceProcessor ??= new SourceProcessor(_scriptProcessor, _sourcesService, _logger);
    }

    public Task<Source> CreateOrUpdateSource(Source updatedSource, string language)
    {
        LoadSourceProcessor();
        return _sourceProcessor.CreateOrUpdateSource(updatedSource, language);
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

    public Task<string> ExecuteScript(Guid scriptId, Guid gameId)
    {
        LoadScriptProcessor();
        return _scriptProcessor.ExecuteScript(scriptId, gameId);
    }

    public Task<string> ExecuteScript(Guid scriptId, Game game)
    {
        LoadScriptProcessor();
        return _scriptProcessor.ExecuteScript(scriptId, game);
    }

    public Task<string> ExecuteScript(Guid scriptId)
    {
        LoadScriptProcessor();
        return _scriptProcessor.ExecuteScript(scriptId);
    }

    public string ExecuteScript(Script script, Game game)
    {
        LoadScriptProcessor();
        return _scriptProcessor.ExecuteScript(script, game);
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
}