using Domain.Entities;
using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface IJsonFileHandler
    {
        List<Player> LoadPlayers();
        void SavePlayers(List<Player> players);
    }
}
