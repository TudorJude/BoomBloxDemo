using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BoomBloxDemo.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField]
        private CrosshairController _crossHairController;
        [SerializeField]
        private GameController _gameController;
        [SerializeField]
        private GunController _gunController;
        [SerializeField]
        private SelectedWeaponUi _semiAutoUi;
        [SerializeField]
        private SelectedWeaponUi _cannonUi;
        [SerializeField]
        private RectTransform _enemyShowcaseUi;
        [SerializeField]
        private TextMeshProUGUI _enemiesLeftText;

        private void Awake()
        {
            var weapons = _gunController.GetWeaponsData();
            foreach (var item in weapons)
            {
                HandleInitWeaponUI(item.WeaponType, item.ClipSize);
            }

        }

        private void OnEnable()
        {
            _gunController.OnFireWeapon += HandleWeaponFire;
            _gunController.OnReloadingWeapon += HandleWeaponReload;
            _gunController.OnDoneReloading += HandleUpdateClipSize;
            _gunController.OnSelectWeapon += HandleWeaponSelect;

            _gameController.OnGameLoopStart += HandleGameStart;
            _gameController.OnEnemyDefeated += HandleUpdateEnemyCount;
        }

        private void OnDisable()
        {
            _gunController.OnFireWeapon -= HandleWeaponFire;
            _gunController.OnReloadingWeapon -= HandleWeaponReload;
            _gunController.OnDoneReloading -= HandleUpdateClipSize;
            _gunController.OnSelectWeapon -= HandleWeaponSelect;

            _gameController.OnGameLoopStart -= HandleGameStart;
            _gameController.OnEnemyDefeated -= HandleUpdateEnemyCount;
        }

        private void HandleInitWeaponUI(Config.Weapon weaponType, int clipSize) //this could be made prettier
        {
            if (weaponType == Config.Weapon.Cannon)
            {
                _cannonUi.Init(clipSize);
            }
            if (weaponType == Config.Weapon.SemiAuto)
            {
                _semiAutoUi.Init(clipSize);
            }
        }
        private void HandleWeaponFire(Config.Weapon weaponType, int clipsLeft)
        {
            _crossHairController.OnFireWeapon();

            HandleUpdateClipSize(weaponType, clipsLeft);

        }
        private void HandleUpdateClipSize(Config.Weapon weaponType, int clipsLeft)
        {
            if (weaponType == Config.Weapon.Cannon)
            {
                _cannonUi.OnClipAmmoChange(clipsLeft);
            }
            if (weaponType == Config.Weapon.SemiAuto)
            {
                _semiAutoUi.OnClipAmmoChange(clipsLeft);
            }
        }

        private void HandleWeaponReload(float reloadDuration)
        {
            _crossHairController.OnReloadWeapon(3f);
        }

        private void HandleWeaponSelect(Config.Weapon weaponType) //this could be made prettier
        {
            if (weaponType == Config.Weapon.Cannon)
            {
                _cannonUi.OnWeaponSelect();
                _semiAutoUi.OnWeaponDeselect();
            }
            if (weaponType == Config.Weapon.SemiAuto)
            {
                _semiAutoUi.OnWeaponSelect();
                _cannonUi.OnWeaponDeselect();
            }
        }

        private void HandleUpdateEnemyCount(int enemyCount)
        {
            _enemiesLeftText.text = "x" + enemyCount;
        }

        private void HandleGameStart(int enemyCount)
        {
            _enemyShowcaseUi.gameObject.SetActive(true);
            _enemiesLeftText.text = "x" + enemyCount;
        }
    }
}
