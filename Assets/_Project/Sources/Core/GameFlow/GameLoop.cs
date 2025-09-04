using Cysharp.Threading.Tasks;
using Project.Core.Features.GameField;
using Project.Core.Features.Units.Character.Core;
using Project.Core.Features.Units.Character.Gameplay;
using Project.Core.Features.Units.Character.Input;
using Project.Core.Features.Units.Enemy.Core;
using Project.Core.Features.Units.Enemy.Gameplay;
using Project.Core.Features.Weapon.Core;
using Project.Core.UI;
using Project.Infrastructure.Services;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.GameFlow
{
    public class GameLoop : IDisposable
    {
        private EnemySpawner _enemySpawner;
        private Vector3 _characterSpawnPosition;
        private ILocalAssetLoader _localAssetLoader;
        private IWeaponFactory _weaponFactory;
        private IGameField _gameField;
        private WindowsRepository _windowsRepository;
        private StaticDataProvider _staticDataProvider;
        private ICharacterFactory _characterFactory;
        private CancellationTokenSource _enemySpawnTokenSource;
        private PlayerCharacterController _playerCharacterControllerTemplate;

        private CharacterCamera _characterCamera;
        private ICharacter _character;
        private PlayerCharacterController _playerCharacterController;

        public GameLoop(PlayerCharacterController characterController, EnemySpawner enemySpawner, Vector3 characterSpawnPosition, ILocalAssetLoader localAssetLoader, IWeaponFactory weaponFactory, IGameField gameField, CharacterCamera characterCamera, WindowsRepository windowsRepository, StaticDataProvider staticDataProvider, ICharacterFactory characterFactory)
        {
            _playerCharacterControllerTemplate = characterController;
            _enemySpawner = enemySpawner;
            _characterSpawnPosition = characterSpawnPosition;
            _localAssetLoader = localAssetLoader;
            _weaponFactory = weaponFactory;
            _gameField = gameField;
            _characterCamera = characterCamera;
            _windowsRepository = windowsRepository;
            _staticDataProvider = staticDataProvider;
            _characterFactory = characterFactory;
        }

        public void Dispose()
        {
            StopSpawning();
        }

        public async UniTaskVoid StartGame(string levelKey)
        {
            var levelDataPath = Path.Combine(CONSTANTS.LEVEL_DATA_PATH, levelKey);
            var levelData = await _localAssetLoader.LoadFromJsonAsync<LevelData>(levelDataPath);

            var character = SetupCharacter();
            var characterController = SetupCharacterController(character);

            if (_windowsRepository.TryGetValue<GameplayWindow>(out var gameplayWindow))
            {
                gameplayWindow.HealthBar.Init(character.Health);
                gameplayWindow.Show();
            }

            _enemySpawner.Init();
            _characterCamera.SetCharacter(character);
            _character = character;
            _playerCharacterController = characterController;
            characterController.Enable();

            _enemySpawnTokenSource = new CancellationTokenSource();
            SpawnEnemyPeriodically(levelData.EnemySpawnInterval, levelData.EnemiesToSpawn, _enemySpawnTokenSource.Token).Forget();
        }

        private ICharacter SetupCharacter()
        {
            var characterData = _staticDataProvider.GetCharacterData();
            var character = _characterFactory.CreateCharacter(_characterSpawnPosition);
            character.Died += OnCharacterDied;
            character.Init(characterData);

            foreach (var slotWeaponName in characterData.SlotsWeapons)
            {
                var weapon = _weaponFactory.CreateWeapon(slotWeaponName.Value);
                character.AddWeapon(weapon, slotWeaponName.Key);
            }

            return character;
        }

        private PlayerCharacterController SetupCharacterController(ICharacter character)
        {
            var characterInput = default(ICharacterInput);
#if UNITY_STANDALONE || UNITY_EDITOR
            characterInput = new PCCharacterInput(() => character.Transform.position, _characterCamera);
#endif

            var characterController = GameObject.Instantiate(_playerCharacterControllerTemplate);
            characterController.Init(characterInput, character);
            return characterController;
        }


        private async UniTaskVoid SpawnEnemyPeriodically(float intervalInSeconds, IEnumerable<EnemyToSpawn> enemies, CancellationToken cancellationToken)
        {
            var intervalTimeSpan = TimeSpan.FromSeconds(intervalInSeconds);

            while (cancellationToken.IsCancellationRequested == false)
            {
                var idIndex = UnityEngine.Random.Range(0, enemies.Count());
                var enemyInfo = enemies.ElementAt(idIndex);
                var cameraRect = _characterCamera.Camera.GetWorldRect();
                var freeSectors = _gameField.GetFreeSectors(cameraRect);
                var targetSector = freeSectors.GetRandomElement();
                var targetPosition = targetSector.GetRandomPoint();
                var enemy = _enemySpawner.Spawn(enemyInfo.ID, enemyInfo.Data, targetPosition);
                enemy.SeeCharacter(_character);
                await UniTask.Delay(intervalTimeSpan, cancellationToken: cancellationToken);
            }
        }

        private void StopSpawning()
        {
            CancellationTokenUtils.Destroy(ref _enemySpawnTokenSource);
        }

        private void OnCharacterDied(ICharacter character)
        {
            _playerCharacterController.Disable();
            _characterCamera.SetCharacter(null);
            character.Died -= OnCharacterDied;
            character.Destroy();
            StopSpawning();
            _enemySpawner.Clear();

            if (_windowsRepository.TryGetValue<GameplayWindow>(out var gameplayWindow))
            {
                gameplayWindow.HealthBar.Dispose();
                gameplayWindow.Hide();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}