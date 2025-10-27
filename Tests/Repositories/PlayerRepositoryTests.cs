using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Repositories
{
    public class PlayerRepositoryTests
    {
        private readonly Mock<IJsonFileHandler> _jsonHandlerMock;
        private readonly PlayerRepository _repository;

        public PlayerRepositoryTests()
        {
            _jsonHandlerMock = new Mock<IJsonFileHandler>();
            var repoLogger = new Mock<ILogger<PlayerRepository>>().Object;

            _repository = new PlayerRepository(_jsonHandlerMock.Object, repoLogger);
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
        public void GetAll_Should_ReturnPlayersOrderedByRank()
        {
            var players = GetSamplePlayers();
            _jsonHandlerMock.Setup(j => j.LoadPlayers()).Returns(players);

            var result = _repository.GetAll().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Data.Rank); // Bob
            Assert.Equal(2, result[1].Data.Rank); // Alice
        }

        [Fact]
        public void GetById_Should_ReturnCorrectPlayer()
        {
            var players = GetSamplePlayers();
            _jsonHandlerMock.Setup(j => j.LoadPlayers()).Returns(players);

            var player = _repository.GetById(2);

            Assert.NotNull(player);
            Assert.Equal("Bob", player.Firstname);
        }

        [Fact]
        public void GetById_Should_ReturnNull_WhenPlayerNotFound()
        {
            var players = GetSamplePlayers();
            _jsonHandlerMock.Setup(j => j.LoadPlayers()).Returns(players);

            var player = _repository.GetById(999);

            Assert.Null(player);
        }

        [Fact]
        public void AddPlayer_Should_AddPlayerAndCallSavePlayers()
        {
            var players = GetSamplePlayers();
            _jsonHandlerMock.Setup(j => j.LoadPlayers()).Returns(players);

            var newPlayer = new Player { Firstname = "Charlie", Lastname = "C", Data = new PlayerData { Rank = 3 } };
            _repository.AddPlayer(newPlayer);

            _jsonHandlerMock.Verify(j => j.SavePlayers(It.Is<List<Player>>(l => l.Contains(newPlayer))), Times.Once);
            Assert.True(newPlayer.Id > 0); // Id auto-incrémenté
        }
    }
}
