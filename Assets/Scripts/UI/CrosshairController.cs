using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BoomBloxDemo.UI
{
    public class CrosshairController : MonoBehaviour
    {
        [SerializeField]
        Image _crossHair;
        public void OnFireWeapon()
        {
            _crossHair.DOKill();
            _crossHair.transform.rotation = Quaternion.identity;
            _crossHair.transform.DOPunchScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
        }

        public void OnReloadWeapon(float reloadTimer)
        {
            _crossHair.DOKill();
            _crossHair.transform.rotation = Quaternion.identity;
            _crossHair.transform.localScale = Vector3.one;
            _crossHair.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutExpo);

            _crossHair.transform.DORotate(new Vector3(0f, 0f, -360), reloadTimer, RotateMode.LocalAxisAdd).SetEase(Ease.OutExpo).onComplete = () =>
            {
                _crossHair.transform.rotation = Quaternion.identity;
                _crossHair.transform.localScale = Vector3.one;
            };

        }
    }
}
