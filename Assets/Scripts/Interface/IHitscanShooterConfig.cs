using UnityEngine;

    //scriptableObjects for HitscanShoots must implement this interface
    public interface IHitscanShooterConfig
    {
        float Range { get; }
        float FireRate { get; }
        float Damage { get; }
        float FireSpread { get; }
        LayerMask TargetMask { get; }
        Transform hitscanOrigin { get; }
        HitscanType HitscanType { get; }
    }