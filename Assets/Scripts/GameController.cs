using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using BoomBloxDemo.UI;

namespace BoomBloxDemo
{
    public class GameController : MonoBehaviour
    {
        enum GameStates
        {
            Start,
            Tutorial,
            EnemyShowCase,
            GameLoop,
            EndGame
        }

        public Action<int> OnGameLoopStart;
        public Action<int> OnEnemyDefeated;


        [SerializeField]
        private TutorialController _tutorialController;
        [SerializeField]
        private EndGameUi _endGameUi;
        [SerializeField]
        private GunController _gunController;
        [SerializeField]
        private CameraControl _cameraController;

        private EnemyBase[] _enemies;

        private GameStates _currentGameState = GameStates.Start;

        private int _bulletsFired;
        private int _cannonBallsFired;

        private void Awake()
        {
            _enemies = FindObjectsByType<EnemyBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var enemy in _enemies)
            {
                enemy.OnGotSmoked += () => { CheckAllEnemiesDead(); };
            }

            _gunController.enabled = false;
            _cameraController.enabled = false;
            _endGameUi.gameObject.SetActive(false);
            _tutorialController.gameObject.SetActive(true);
            _bulletsFired = 0;
            _cannonBallsFired = 0;

            _currentGameState = GameStates.Tutorial;
        }

        private void OnEnable()
        {
            _tutorialController.OnTutorialEnd += HandleTutorialEnd;
            _endGameUi.OnRestartGameRequest += RestartGame;
            _gunController.OnFireWeapon += CountBullets;
        }

        private void OnDisable()
        {
            _tutorialController.OnTutorialEnd -= HandleTutorialEnd;
            _endGameUi.OnRestartGameRequest -= RestartGame;
            _gunController.OnFireWeapon -= CountBullets;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                RestartGame();
            }
        }

        private void CountBullets(Config.Weapon weaponType, int clipSize)
        {
            if (weaponType == Config.Weapon.Cannon)
                _cannonBallsFired++;
            else
                _bulletsFired++;
        }

        private void CheckAllEnemiesDead()
        {
            int enemiesLeft = 0;
            foreach (var enemy in _enemies)
            {
                if (!enemy.GetIsDead) enemiesLeft++;
            }
            if (enemiesLeft > 0)
            {
                OnEnemyDefeated?.Invoke(enemiesLeft);
                return;
            }
            Debug.Log("All enemies are dead");
            if (_currentGameState == GameStates.GameLoop)
            {
                _currentGameState = GameStates.EndGame;
                _endGameUi.gameObject.SetActive(true);
                _endGameUi.Init(_bulletsFired, _cannonBallsFired);
                _gunController.enabled = false;
                _cameraController.enabled = false;
            }

        }

        private void HandleTutorialEnd()
        {
            _currentGameState = GameStates.GameLoop;
            _gunController.enabled = true;
            _cameraController.enabled = true;
            _tutorialController.gameObject.SetActive(false);
            OnGameLoopStart?.Invoke(_enemies.Length);
        }

        private void RestartGame()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
