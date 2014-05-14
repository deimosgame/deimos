using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Bullet : CollisionElement
    {
        public bool Collided = false;

        // The projectile is always spawned by the weapon,
        // and destroys itself after collision with boundingbox,
        // or dissipates when it is sure of not hitting anything after completion of trajectory
        // -> moving object, with predictive calculations.

        // Damage calculations may also be made inside the Bullet class,
        // provided that values are restricted by Weapon, or Player.Health.
        public Vector3 Direction;
        public Vector3 Position;
        public float speed;
        public float range;
        public float minimumDmg;
        public float maximumDmg;
        public float lifeSpan = 5;

        // for range purposes
        public float DistanceTraveled = 0;

        public char WeaponRep;

        // Constructor
        public Bullet(Vector3 pos, Vector3 dir, char rep)
            : base(new Vector3(1f, 1f, 1f))
        {
            // Setting initial bullet spawn location
            Position = pos;

            // Setting bullet direction according to current player's camera
            Direction = dir;

            // Setting bullet properties according to current player's current weapon
            speed = GameplayFacade.ThisPlayer.CurrentWeapon.ProjectileSpeed;
            range = GameplayFacade.ThisPlayer.CurrentWeapon.Range;
            minimumDmg = GameplayFacade.ThisPlayer.CurrentWeapon.minDmg;
            maximumDmg = GameplayFacade.ThisPlayer.CurrentWeapon.maxDmg;

            Nature = ElementNature.Bullet;

            WeaponRep = rep;
        }

        // Destructor
        ~Bullet()
        {

        }

        // Methods
        public override void CollisionEvent(CollisionElement element)
        {
            if (element.GetNature() != ElementNature.Object
                && element.GetNature() != ElementNature.Bullet)
            {
                Collided = true;

                if (WeaponRep == 'E')
                {
                    Explode(Position);
                }

                else
                {
                    switch (element.Nature)
                    {
                        case ElementNature.World:
                            break;
                        case ElementNature.Player:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void Hurt()
        {

        }

        public void Explode(Vector3 pos)
        {
            GeneralFacade.SceneManager.SoundManager.Play3D("explosion",
                GameplayFacade.ThisPlayer.Position,
                pos
                );
            PlayFireParticle(pos);

            //BoundingSphere explosion = new BoundingSphere();
            //explosion.Center = pos;
            //explosion.Radius = 30;
            //bool touch = false;
            //bool playert = false;
            //CollisionElement test = new CollisionElement(new Vector3(20, 20, 20));
            //test.Model = GeneralFacade.SceneManager.ModelManager.GetLevelModel("dummy");
            //test.Model.Position = new Vector3(17, 0, -9);
            //test.Nature = ElementNature.Player;
            //BoundingBox box = test.Box;
            //explosion.Intersects(ref box, out touch);
            //CollisionElement player = new CollisionElement(new Vector3(20,20,20));
            //player.Model = GeneralFacade.SceneManager.ModelManager.GetLevelModel("player");
            //player.Model.Position = GameplayFacade.ThisPlayer.Position - new Vector3(0, 6, 0);
            //player.Model.show = false;
            //player.Nature = ElementNature.Player;
            //BoundingBox playbox = player.Box;
            
            //explosion.Intersects(ref playbox, out playert);

            //DisplayFacade.DebugScreen.Debug(touch.ToString());

            //if (playert)
            //{
            //    GameplayFacade.ThisPlayer.Hurt((int)(GameplayFacade.Weapons.GetWeapon(GameplayFacade.Weapons.GetName(WeaponRep)).minDmg));
            //}
        }

        public void PlayFireParticle(Vector3 pos)
        {
            ExplosionParticleEmitter emitter = new ExplosionParticleEmitter(pos);
            DisplayFacade.ExplosionParticleSystem.AddEmitter(emitter);
            emitter.Emit(10);
        }
    }
}
