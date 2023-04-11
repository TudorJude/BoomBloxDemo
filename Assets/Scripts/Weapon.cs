using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoomBloxDemo
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Custom/New Weapon")]
    public class Weapon : ScriptableObject
    {
        public string WeaponName;
        public Config.Weapon WeaponType;
        public Bullet Bullet;
        public float BulletFireForce;
        public int ClipSize;
        public float ReloadTime;
    }
}
