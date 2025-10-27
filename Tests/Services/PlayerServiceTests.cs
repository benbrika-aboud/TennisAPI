using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly PlayerService _service;

        public PlayerServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            var logger = new Mock<ILogger<PlayerService>>().Object;
            _service = new PlayerService(_repoMock.Object, logger);
        }

        private List<Player> GetSamplePlayers()
        {
            return new List<Player>
            {
                new Player
                {
                    Id = 1,
                    Firstname = "Alice",
                    Lastname = "A",
                    Data = new PlayerData { Rank = 2, Height = 180, Weight = 70000, Last = new List<int> { 1, 0, 1 } },
                    Country = new Country { Code = "FR" }
                },
                new Player
                {
                    Id = 2,
                    Firstname = "Bob",
                    Lastname = "B",
                    Data = new PlayerData { Rank = 1, Height = 175, Weight = 80000, Last = new List<int> { 1, 1, 1 } },
                    Country = new Country { Code = "USA" }
                }
            };
        }

        [Fact]
        public void GetAllPlayers_ShouldReturnAllPlayers()
        {
            var players = GetSamplePlayers();
            _repoMock.Setup(r => r.GetAll()).Returns(players);

            var result = _service.GetAllPlayers();

            Assert.Equal(players.Count, ((List<Player>)result).Count);
        }

        [Fact]
        public void GetPlayerById_ShouldReturnCorrectPlayer()
        {
            var players = GetSamplePlayers();
            _repoMock.Setup(r => r.GetById(1)).Returns(players[0]); // <- CORRECTED

            var player = _service.GetPlayerById(1);

            Assert.NotNull(player);
            Assert.Equal("Alice", player.Firstname);
        }

        [Fact]
        public void GetPlayerById_ShouldReturnNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetById(999)).Returns((Player)null);

            var player = _service.GetPlayerById(999);

            Assert.Null(player);
        }

        [Fact]
        public void AddPlayer_ShouldCallRepository()
        {
            var newPlayer = new Player { Firstname = "Charlie", Lastname = "C" };

            _service.AddPlayer(newPlayer);

            _repoMock.Verify(r => r.AddPlayer(newPlayer), Times.Once);
        }

        [Fact]
        public void GetStats_ShouldReturnExpectedFields()
        {
            var players = GetSamplePlayers();
            _repoMock.Setup(r => r.GetAll()).Returns(players);

            var stats = _service.GetStats();

            Assert.NotNull(stats);
            Assert.Contains("Pays_Meilleur_Ratio", stats.ToString());
            Assert.Contains("Imc_Moyen", stats.ToString());
            Assert.Contains("Mediane_Taille", stats.ToString());
        }
    }
}
