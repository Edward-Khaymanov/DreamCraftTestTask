using System.Collections.Generic;

namespace Project.Core.Features.Units.Character.Core
{
    public class CharacterData
    {
        public float TotalHealth { get; set; }
        public float MoveSpeed { get; set; }
        public Dictionary<int, string> SlotsWeapons { get; set; }
    }
}
