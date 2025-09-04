using Project.Core.Features.Units.Character.Core;
using System.Collections.Generic;

namespace Project.Infrastructure.Services
{
    public class StaticDataProvider
    {
        public CharacterData GetCharacterData()
        {
            var data = new CharacterData()
            {
                MoveSpeed = 30,
                TotalHealth = 400,
                SlotsWeapons = new Dictionary<int, string>()
                {
                    [1] = "glock-1",
                    [2] = "shotgun-1",
                }
            };
            return data;
        }
    }
}
