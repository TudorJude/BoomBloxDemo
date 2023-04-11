using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    public class GunController : MonoBehaviour
    {
        public Action<Config.Weapon, int> OnFireWeapon;
        public Action<float> OnReloadingWeapon;
        public Action<Config.Weapon, int> OnDoneReloading;
        public Action<Config.Weapon> OnSelectWeapon;

        [SerializeField]
        private List<Weapon> _weapons;
        [SerializeField]
        private AudioSource _audioSourcePrefab;
        [SerializeField]
        private AudioClip _reloadSound;
        [SerializeField]
        private AudioClip _weaponSwapSound;
        private int[] _weaponClips;
        private int _currentSelectedWeapon = 0;
        private List<ObjectPooler> _bulletPooler;
        private List<ObjectPooler> _bulletImpactEffectsPooler;
        private ObjectPooler _audioSourcePool;
        private bool _isReloading;
        private void Awake()
        {
            _isReloading = false;
            _bulletPooler = new List<ObjectPooler>(_weapons.Count);
            _bulletImpactEffectsPooler = new List<ObjectPooler>(_weapons.Count);
            _weaponClips = new int[_weapons.Count];
            for (int i = 0; i < _weapons.Count; i++)
            {
                _bulletPooler.Add(gameObject.AddComponent<ObjectPooler>());
                _bulletPooler[i].Init(_weapons[i].Bullet.gameObject);
                _weaponClips[i] = _weapons[i].ClipSize;

                _bulletImpactEffectsPooler.Add(gameObject.AddComponent<ObjectPooler>());
                _bulletImpactEffectsPooler[i].Init(_weapons[i].Bullet.ImpactEffect);
            }
            _audioSourcePool = gameObject.AddComponent<ObjectPooler>();
            _audioSourcePool.Init(_audioSourcePrefab.gameObject);
        }

        public List<Weapon> GetWeaponsData()
        {
            return _weapons;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LaunchBullet();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CycleWeapon();
            }
        }

        private void CycleWeapon()
        {
            if (_isReloading) return;
            _currentSelectedWeapon++;
            if (_currentSelectedWeapon >= _weapons.Count)
                _currentSelectedWeapon = 0;

            OnSelectWeapon?.Invoke(_weapons[_currentSelectedWeapon].WeaponType);
            StartCoroutine(PlayAudio(_weaponSwapSound));
        }

        private void LaunchBullet()
        {
            if (_isReloading) return;
            Bullet generatedBullet = _bulletPooler[_currentSelectedWeapon].Instantiate().GetComponent<Bullet>();
            generatedBullet.transform.position = transform.position;
            generatedBullet.GetComponent<Rigidbody>().AddForce(transform.forward * _weapons[_currentSelectedWeapon].BulletFireForce);

            generatedBullet.OnCollision = (bullet, pos) =>
            {
                if ((bullet.ImpactSounds != null) && (bullet.ImpactSounds.Count > 0))
                    StartCoroutine(PlayAudio(bullet.ImpactSounds[UnityEngine.Random.Range(0, bullet.ImpactSounds.Count - 1)]));

                _bulletPooler[_currentSelectedWeapon].Destroy(bullet.gameObject);
                StartCoroutine(ShowImpactEffect(pos));
            };
            generatedBullet.OnExpire = (bullet) => { _bulletPooler[_currentSelectedWeapon].Destroy(bullet.gameObject); };

            StartCoroutine(PlayAudio(generatedBullet.FireSound));

            _weaponClips[_currentSelectedWeapon]--;
            OnFireWeapon?.Invoke(_weapons[_currentSelectedWeapon].WeaponType, _weaponClips[_currentSelectedWeapon]);
            if (_weaponClips[_currentSelectedWeapon] <= 0)
            {
                StartCoroutine(ReloadWeapon(_weapons[_currentSelectedWeapon].ReloadTime, _weapons[_currentSelectedWeapon].ClipSize));
            }
        }

        private IEnumerator ReloadWeapon(float reloadTime, int clipSize)
        {
            _isReloading = true;
            StartCoroutine(PlayAudio(_reloadSound));
            OnReloadingWeapon?.Invoke(reloadTime);
            yield return new WaitForSeconds(reloadTime);
            _weaponClips[_currentSelectedWeapon] = clipSize;
            _isReloading = false;
            OnDoneReloading?.Invoke(_weapons[_currentSelectedWeapon].WeaponType, clipSize);
        }

        private IEnumerator PlayAudio(AudioClip clip)
        {
            AudioSource tempAudio = _audioSourcePool.Instantiate().GetComponent<AudioSource>();
            tempAudio.clip = clip;
            tempAudio.Play();
            yield return new WaitForSecondsRealtime(clip.length);
            _audioSourcePool.Destroy(tempAudio.gameObject);
        }

        private IEnumerator ShowImpactEffect(Vector3 pos)
        {
            var effect = _bulletImpactEffectsPooler[_currentSelectedWeapon].Instantiate();
            effect.transform.position = pos;
            yield return new WaitForSeconds(3f);
            _bulletImpactEffectsPooler[_currentSelectedWeapon].Destroy(effect);
        }
    }
}
