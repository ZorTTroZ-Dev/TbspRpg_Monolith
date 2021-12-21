using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class UserProcessorTests: ProcessorTest
    {
        #region RegisterUser

        [Fact]
        public async void RegisterUser_ExistingEmail_ThrowException()
        {
            
        }

        [Fact]
        public async void RegisterUser_Valid_UserCreated()
        {
            
        }
        

        #endregion

        #region VerifyUserRegistration

        [Fact]
        public async void VerifyUserRegistration_InvalidUserId_ThrowException()
        {
            
        }

        [Fact]
        public async void VerifyUserRegistration_RegistrationComplete_ThrowException()
        {
            
        }

        [Fact]
        public async void VerifyUserRegistration_WrongRegistrationKey_ReturnNull()
        {
            
        }

        [Fact]
        public async void VerifyUserRegistration_Valid_RegistrationCompleted()
        {
            
        }

        #endregion
    }
}