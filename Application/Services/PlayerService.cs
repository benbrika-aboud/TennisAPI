using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repo;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository repo, ILogger<PlayerService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _repo.GetAll();
        }

        public Player? GetPlayerById(int id)
        {
            return _repo.GetById(id);
        }

        public void AddPlayer(Player player)
        {
            _repo.AddPlayer(player);
        }

        public object GetStats()
        {
            var players = _repo.GetAll().ToList();
            if (!players.Any()) return new { message = "Aucun joueur disponible" };

            // Pays avec le meilleur ratio de victoires
            var bestCountry = players
                .GroupBy(p => p.Country.Code)
                .Select(g => new
                {
                    Country = g.Key,
                    WinRate = g.SelectMany(p => p.Data.Last).DefaultIfEmpty().Average()
                })
                .OrderByDescending(x => x.WinRate)
                .First();

            // IMC moyen
            var averageIMC = players.Average(p => (p.Data.Weight/1000) / Math.Pow(p.Data.Height / 100.0, 2));

            // Médiane de la taille
            var heights = players.Select(p => p.Data.Height).OrderBy(h => h).ToList();
            double medianHeight = heights.Count % 2 == 1
                ? heights[heights.Count / 2]
                : (heights[heights.Count / 2 - 1] + heights[heights.Count / 2]) / 2.0;

            return new
            {
                Pays_Meilleur_Ratio = bestCountry.Country,
                Imc_Moyen = Math.Round(averageIMC, 2),
                Mediane_Taille = medianHeight
            };
        }
    }
}
