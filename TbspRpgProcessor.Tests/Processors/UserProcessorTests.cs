using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class UserProcessorTests: ProcessorTest
    {
        #region RegisterUser

        [Fact]
        public async void RegisterUser_ExistingEmail_ThrowException()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            Task Act() => processor.RegisterUser(new UserRegisterModel()
            {
                Email = "test@test.com",
                Password = "test"
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void RegisterUser_Valid_UserCreated()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            var user = await processor.RegisterUser(new UserRegisterModel()
            {
                Email = "test_two@test.com",
                Password = "test"
            });
            
            // assert
            Assert.NotNull(user);
            Assert.Equal("test_two@test.com", user.Email);
            Assert.Equal(6, user.RegistrationKey.Length);
        }
        

        #endregion

        #region VerifyUserRegistration

        [Fact]
        public async void VerifyUserRegistration_InvalidUserId_ThrowException()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            Task Act() => processor.VerifyUserRegistration(new UserVerifyRegisterModel()
            {
                UserId = Guid.NewGuid(),
                RegistrationKey = "123456"
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void VerifyUserRegistration_RegistrationComplete_ThrowException()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = true
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            Task Act() => processor.VerifyUserRegistration(new UserVerifyRegisterModel()
            {
                UserId = testUsers[0].Id,
                RegistrationKey = "123456"
            });

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        [Fact]
        public async void VerifyUserRegistration_WrongRegistrationKey_ReturnNull()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = false,
                    RegistrationKey = "000000"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            var user = await processor.VerifyUserRegistration(new UserVerifyRegisterModel()
            {
                UserId = testUsers[0].Id,
                RegistrationKey = "123456"
            });
            
            // assert
            Assert.Null(user);
        }

        [Fact]
        public async void VerifyUserRegistration_Valid_RegistrationCompleted()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = false,
                    RegistrationKey = "000000"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            var user = await processor.VerifyUserRegistration(new UserVerifyRegisterModel()
            {
                UserId = testUsers[0].Id,
                RegistrationKey = "000000"
            });
            
            // assert
            Assert.NotNull(user);
            Assert.Null(user.RegistrationKey);
            Assert.True(user.RegistrationComplete);
        }

        #endregion

        #region ResendUserRegistration

        [Fact]
        public async void ResendUserRegistration_InvalidUserId_ThrowException()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = false,
                    RegistrationKey = "000000"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            Task Act() => processor.ResendUserRegistration(new UserRegisterResendModel()
            {
                UserId = Guid.NewGuid()
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void ResendUserRegistration_AlreadyRegistered_ThrowException()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = true,
                    RegistrationKey = "000000"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            Task Act() => processor.ResendUserRegistration(new UserRegisterResendModel()
            {
                UserId = testUsers[0].Id
            });

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        [Fact]
        public async void ResendUserRegistration_Vald_ResendRegistration()
        {
            // arrange
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = false,
                    RegistrationKey = "000000"
                }
            };
            var processor = CreateUserProcessor(testUsers);
            
            // act
            var user = await processor.ResendUserRegistration(new UserRegisterResendModel()
            {
                UserId = testUsers[0].Id
            });
            
            // assert
            Assert.NotNull(user);
            Assert.NotEqual("000000", user.RegistrationKey);
        }

        #endregion

        #region MailTest

        // [Fact]
        // public async void SendMailTest()
        // {
            // var mailClient = new MailClient(
            //     new SmtpSettings()
            //     {
            //         Server = "smtp-relay.gmail.com",
            //         Port = 587,
            //         Username = "",
            //         Password = "",
            //         SendMail = true
            //     });
            //
            // await mailClient.SendRegistrationVerificationMail("cdavid.vanhorn@gmail.com", "000000");
        // }

        #endregion
    }
}