using Project.Core.Features.Units.Enemy.Core;
using System;
using System.Collections.Generic;

namespace Project.Core.GameFlow
{
    [Serializable]
    public class LevelData
    {
        public float EnemySpawnInterval { get; set; }
        public List<EnemyToSpawn> EnemiesToSpawn { get; set; }
    }
}