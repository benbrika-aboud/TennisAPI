using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetAll();
        Player? GetById(int id);
        void AddPlayer(Player player);
    }
}
