using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public enum Weapon
    {
        SemiAuto,
        Cannon
    }
    public static float BULLET_LIFETIME_SECONDS = 5f;
    public static float COLLISION_FORCE_DAMAGE_THRESHOLD = 10f;
}
