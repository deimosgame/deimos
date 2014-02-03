using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Bullet
    {
        // The projectile is always spawned by the weapon,
        // and destroys itself after collision with boundingbox,
        // or dissipates when it is sure of not hitting anything after completion of trajectory
        // -> moving object, with predictive calculations.

        // Damage calculations may also be made inside the Bullet class,
        // provided that values are restricted by Weapon, or Player.Health.
    }
}
