using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("TDCore/TargetSelector/StrongCriptSelector")]
public class StrongCriptSelector : TargetSelector
{
    public override IHittable SelectTarger()
    {
        if (allEnemy.Count <= 0) return null;
        return allEnemy.OrderBy(enemy => enemy.HP).ToList()[allEnemy.Count - 1];
    }
}
