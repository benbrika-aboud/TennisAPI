using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Shortname { get; set; } = "";
        public string Sex { get; set; } = "";
        public string Picture { get; set; } = "";
        public Country Country { get; set; } = new();
        public PlayerData Data { get; set; } = new();
    }
}
