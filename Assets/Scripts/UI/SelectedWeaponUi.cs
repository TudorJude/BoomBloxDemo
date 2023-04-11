using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BoomBloxDemo.UI
{
    public class SelectedWeaponUi : MonoBehaviour
    {
        [SerializeField]
        private Image _bulletIcon;
        private Image _weaponIcon;

        private List<Image> _bullets;

        private void Awake()
        {
            _weaponIcon = GetComponent<Image>();
        }

        public void Init(int numberOfBullets)
        {
            _bullets = new List<Image>
        {
            _bulletIcon
        };
            for (int i = 1; i < numberOfBullets; i++)
            {
                _bullets.Add(Instantiate(_bulletIcon, _weaponIcon.transform));
            }
        }

        public void OnWeaponSelect()
        {
            _weaponIcon.transform.DOKill();
            _weaponIcon.transform.localScale = Vector3.one;
            _weaponIcon.transform.DOScale(1.3f, 0.1f).SetEase(Ease.OutBack);
        }

        public void OnWeaponDeselect()
        {
            _weaponIcon.transform.DOKill();
            _weaponIcon.transform.localScale = Vector3.one;
        }

        public void OnClipAmmoChange(int ammoAmount)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (ammoAmount < i + 1)
                    _bullets[i].color = new Color(1f, 1f, 1f, 0.3f);
                else
                    _bullets[i].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
