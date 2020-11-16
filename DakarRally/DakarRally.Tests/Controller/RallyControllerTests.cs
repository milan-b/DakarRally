using Contracts;
using DakarRally.Controllers;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DakarRally.Tests.Controller
{
    public class RallyControllerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly RallyController _controller;

        public RallyControllerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _controller = new RallyController(Mock.Of<ILogger<RallyController>>(), _mockRepo.Object, null);
        }

        [Fact]
        public async Task CreateRace_ActionExecutes_CreateOneRace()
        {
            Race race = null;
            _mockRepo.Setup(r => r.Race.Create(It.IsAny<Race>()))
                    .Callback<Race>(x => race = x);
            var result  = await _controller.CreateRace(new RaceDTO { Year = 2020});

            Assert.IsType<CreatedResult>(result);
            
            _mockRepo.Verify(x => x.Race.Create(It.IsAny<Race>()), Times.Once);
        }

        


    }
}
