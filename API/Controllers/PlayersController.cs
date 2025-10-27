using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        /// <summary>
        /// Récupère la liste complète des joueurs (triée par rang).
        /// </summary>
        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            _logger.LogInformation("Récupération de la liste des joueurs.");
            var players = _playerService.GetAllPlayers();
            return Ok(players);
        }

        /// <summary>
        /// Récupère un joueur par son ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public IActionResult GetPlayerById(int id)
        {
            _logger.LogInformation("Récupération du joueur avec ID {PlayerId}", id);

            var player = _playerService.GetPlayerById(id);
            if (player == null)
            {
                _logger.LogWarning("Aucun joueur trouvé avec l'ID {PlayerId}", id);
                return NotFound(new { message = $"Joueur avec ID {id} introuvable." });
            }

            return Ok(player);
        }

        /// <summary>
        /// Ajoute un nouveau joueur.
        /// </summary>
        [HttpPost]
        public IActionResult AddPlayer([FromBody] Player player)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Ajout d’un nouveau joueur : {FirstName} {LastName}", player.Firstname, player.Lastname);
            _playerService.AddPlayer(player);
            return CreatedAtAction(nameof(GetPlayerById), new { id = player.Id }, player);
        }

        /// <summary>
        /// Récupère les statistiques globales sur les joueurs.
        /// </summary>
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            _logger.LogInformation("Récupération des statistiques des joueurs.");
            var stats = _playerService.GetStats();
            return Ok(stats);
        }
    }
}
