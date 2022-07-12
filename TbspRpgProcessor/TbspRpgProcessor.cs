using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor;

public interface ITbspRpgProcessor
{
    Task<User> RegisterUser(UserRegisterModel userRegisterModel);
    Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel);
    Task<User> ResendUserRegistration(UserRegisterResendModel userRegisterResendModel);
}

public class TbspRpgProcessor: ITbspRpgProcessor
{
    private IUserProcessor _userProcessor;

    private readonly IUsersService _usersService;

    private readonly IMailClient _mailClient;
    
    private readonly ILogger<TbspRpgProcessor> _logger;

    #region Constructor

    public TbspRpgProcessor(
        IUsersService usersService,
        IMailClient mailClient,
        ILogger<TbspRpgProcessor> logger)
    {
        _usersService = usersService;
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

    
}