using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Infrastructure.Handlers;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IJsonFileHandler _jsonHandler;
        private readonly ILogger<PlayerRepository> _logger;

        public PlayerRepository(IJsonFileHandler jsonHandler, ILogger<PlayerRepository> logger)
        {
            _jsonHandler = jsonHandler;
            _logger = logger;
        }

        public IEnumerable<Player> GetAll()
        {
            List<Player> players = _jsonHandler.LoadPlayers();
            return players.OrderBy(p => p.Data.Rank);
        }

        public Player? GetById(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public void AddPlayer(Player player)
        {
            var players = _jsonHandler.LoadPlayers();
            player.Id = players.Any() ? players.Max(p => p.Id) + 1 : 1;
            players.Add(player);
            _jsonHandler.SavePlayers(players);
        }
    }
}
