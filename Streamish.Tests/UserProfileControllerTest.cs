using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streamish.Models;
using Streamish.Repositories;
using Streamish.Controllers;
using Streamish.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Streamish.Tests
{
    public class UserProfileControllerTest
    {
        [Fact]
        public void Get_By_Id_With_Videos()
        {
            var testData = CreateTestProfiles(20);
            var testId = testData[0].Id;
            
            var repo = new InMemoryUserRepository(testData);
            var controller = new UserProfileController(repo);

            var result = controller.GetByIdWithVideos(testId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<UserProfile>(okResult.Value);

            Assert.Equal(testId, actualUser.Id);
        }

        [Fact]
        public void Get_All()
        {
            
            var testData = CreateTestProfiles(20);

            var repo = new InMemoryUserRepository(testData);
            var controller = new UserProfileController(repo);

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<List<UserProfile>>(okResult.Value);

            Assert.Equal(actualUser, testData);
        }

        [Fact]
        public void Test_Delete()
        {
            var testValue = 99;
            var testData = CreateTestProfiles(20);
            testData[0].Id = testValue;

            var repo = new InMemoryUserRepository(testData);
            var controller = new UserProfileController(repo);

            controller.Delete(testValue);

            repo.InternalData.FirstOrDefault(p => p.Id == testValue);
        }

        [Fact]
        public void Test_Register()
        {

            var testData = CreateTestProfiles(20);
            
            var repo = new InMemoryUserRepository(testData);
            var controller = new UserProfileController(repo);

            var newUser = CreateTestUserProfile(99);

            controller.Register(newUser);
            
            Assert.Equal(testData.Last(), newUser);
        }

        [Fact]
        public void Test_Get_Firebase()
        {
            var newFireId = "yup";
            var testData = CreateTestProfiles(20);
            testData[5].FirebaseUserId = newFireId;

            var repo = new InMemoryUserRepository(testData);
            var controller = new UserProfileController(repo);

            var result = controller.GetByFireBaseUserId(newFireId);

            var OkResult = Assert.IsType<OkObjectResult>(result);
            var actualProfile = Assert.IsType<UserProfile>(OkResult.Value);

            Assert.Equal(actualProfile, testData[5]);
        }

        private List<UserProfile> CreateTestProfiles(int count)
        {
            var users = new List<UserProfile>();
            for (var i = 1; i <= count; i++)
            {
                users.Add(CreateTestUserProfile(i));
            }
            return users;
        }

        private UserProfile CreateTestUserProfile(int id)
        {
            return new UserProfile()
            {
                Id = id,
                Name = $"User {id}",
                Email = $"user{id}@example.com",
                DateCreated = DateTime.Today.AddDays(-id),
                ImageUrl = $"http://user.url/{id}",
            };
        }
    }
}
