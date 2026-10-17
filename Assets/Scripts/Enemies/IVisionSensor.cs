using UnityEngine;

public interface IVisionSensor
{
    bool TrySeeTarget(Vector2 facing, out Transform target);
}