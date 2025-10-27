using System.Text.Json;
using Domain.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Handlers
{
    public class JsonFileHandler : IJsonFileHandler
    {
        private readonly string _filePath;
        private readonly ILogger<JsonFileHandler> _logger;

        public JsonFileHandler(ILogger<JsonFileHandler> logger)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "headtohead.json");
            _logger = logger;
        }

        public List<Player> LoadPlayers()
        {
            try
            {
                if (!File.Exists(_filePath))
                    throw new FileNotFoundException("Le fichier JSON est introuvable.");

                var json = File.ReadAllText(_filePath);
                using var doc = JsonDocument.Parse(json);
                var playersElement = doc.RootElement.GetProperty("Players");

                var players = JsonSerializer.Deserialize<List<Player>>(playersElement.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (players == null)
                    throw new JsonFileException("Erreur lors de la désérialisation des joueurs.");

                return players;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la lecture des joueurs dans le fichier JSON.");
                throw;
            }
        }

        public void SavePlayers(List<Player> players)
        {
            try
            {
                var wrapper = new { Players = players };
                var json = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'écriture des joueurs dans le fichier JSON.");
                throw new JsonFileException("Impossible d'écrire la liste des joueurs dans le fichier JSON.", ex);
            }
        }
    }
}
