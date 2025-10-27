using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetAllPlayers();
        Player? GetPlayerById(int id);
        void AddPlayer(Player player);
        object GetStats();
    }
}
