using Project.Core.Features.GameField;
using Project.Core.Features.Projectile;
using Project.Core.Features.Units.Character.Core;
using Project.Core.Features.Units.Character.Gameplay;
using Project.Core.Features.Units.Character.Input;
using Project.Core.Features.Units.Enemy.Gameplay;
using Project.Core.Features.Weapon.Core;
using Project.Core.Features.Weapon.Gameplay;
using Project.Core.UI;
using Project.Infrastructure.Services;
using Project.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.GameFlow
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private CharacterCamera _characterCamera;
        [SerializeField] private Character _characterTemplate;
        [SerializeField] private Transform _characterSpawnPoint;
        [SerializeField] private PlayerCharacterController _playerCharacterControllerTemplate;
        [SerializeField] private WeaponDefinitionsCatalog _weaponDefinitionsCatalog;
        [SerializeField] private ProjectilesCatalog _projectilesCatalog;
        [SerializeField] private GameField _gameField;
        [SerializeField] private GameplayWindow _gameplayWindow;
        [SerializeField] private EnemiesCatalog _enemiesCatalog;

        private List<IDisposable> _disposables;
        private ILocalAssetLoader _localAssetLoader;
        private IWeaponFactory _weaponFactory;
        private IProjectileFactory _projectileFactory;
        private WindowsRepository _windowsRepository;
        private EnemySpawner _enemySpawner;
        private StaticDataProvider _staticDataProvider;
        private ICharacterFactory _characterFactory;

        private void Awake()
        {
            _disposables = new List<IDisposable>();
            _localAssetLoader = new ResourceAssetLoader();
            _projectileFactory = new ProjectileFactory(_projectilesCatalog.NamesProjectiles);
            _weaponFactory = new WeaponFactory(_weaponDefinitionsCatalog.WeaponDefinitions, _projectileFactory);
            _enemySpawner = new EnemySpawner(_enemiesCatalog.IdPrefabs);
            _staticDataProvider = new StaticDataProvider();
            _windowsRepository = new WindowsRepository();
            _characterFactory = new CharacterFactory(_characterTemplate);
            _windowsRepository.Register(_gameplayWindow);
        }

        private void Start()
        {
            var gameLoop = new GameLoop(_playerCharacterControllerTemplate, _enemySpawner, _characterSpawnPoint.position, _localAssetLoader, _weaponFactory, _gameField, _characterCamera, _windowsRepository, _staticDataProvider, _characterFactory);
            _disposables.Add(gameLoop);
            gameLoop.StartGame(CONSTANTS.FIRST_LEVEL_ID).Forget();
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }

}