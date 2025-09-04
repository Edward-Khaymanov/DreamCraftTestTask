using AYellowpaper.SerializedCollections;
using Project.Core.Features.Units.Enemy.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.Units.Enemy.Gameplay
{
    [CreateAssetMenu(menuName = "Custom/Enemies Catalog")]
    public class EnemiesCatalog : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, EnemyBase> _idPrefabs;

        public IDictionary<string, EnemyBase> IdPrefabs => _idPrefabs;
    }
}