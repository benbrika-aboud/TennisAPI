using API.Controllers;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests.Controllers
{
    public class PlayersControllerTests
    {
        private readonly Mock<ILogger<PlayersController>> _loggerMock;
        private readonly Mock<IPlayerService> _serviceMock;
        private readonly PlayersController _controller;

        public PlayersControllerTests()
        {
            _loggerMock = new Mock<ILogger<PlayersController>>();
            _serviceMock = new Mock<IPlayerService>();
            _controller = new PlayersController(_serviceMock.Object, _loggerMock.Object);
        }

        private List<Player> GetSamplePlayers()
        {
            return new List<Player>
            {
                new Player { Id = 1, Firstname = "Alice", Lastname = "A", Data = new PlayerData { Rank = 2 } },
                new Player { Id = 2, Firstname = "Bob", Lastname = "B", Data = new PlayerData { Rank = 1 } }
            };
        }

        [Fact]
        public void GetAllPlayers_ShouldReturnOkWithPlayers()
        {
            var players = GetSamplePlayers();
            _serviceMock.Setup(s => s.GetAllPlayers()).Returns(players);

            var result = _controller.GetAllPlayers() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(players, result.Value);
        }

        [Fact]
        public void GetPlayerById_ShouldReturnOk_WhenFound()
        {
            var player = GetSamplePlayers()[0];
            _serviceMock.Setup(s => s.GetPlayerById(1)).Returns(player);

            var result = _controller.GetPlayerById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(player, result.Value);
        }

        [Fact]
        public void GetPlayerById_ShouldReturnNotFound_WhenNotFound()
        {
            _serviceMock.Setup(s => s.GetPlayerById(999)).Returns((Player)null);

            var result = _controller.GetPlayerById(999) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void AddPlayer_ShouldReturnCreatedAtAction()
        {
            var newPlayer = new Player { Id = 3, Firstname = "Charlie", Lastname = "C" };

            var result = _controller.AddPlayer(newPlayer) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(newPlayer, result.Value);
            Assert.Equal(nameof(PlayersController.GetPlayerById), result.ActionName);
            Assert.Equal(newPlayer.Id, result.RouteValues["id"]);
        }

        [Fact]
        public void GetStats_ShouldReturnOkWithStats()
        {
            var statsObj = new { Pays_Meilleur_Ratio = "USA", Imc_Moyen = 22.5, Mediane_Taille = 180 };
            _serviceMock.Setup(s => s.GetStats()).Returns(statsObj);

            var result = _controller.GetStats() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(statsObj, result.Value);
        }
    }
}
