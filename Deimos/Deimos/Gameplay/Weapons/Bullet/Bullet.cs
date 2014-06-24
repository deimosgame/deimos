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
        public bool Own = true;

        // for range purposes
        public float DistanceTraveled = 0;

        public char WeaponRep;

        // Constructor
        public Bullet(Vector3 pos, Vector3 dir, char rep)
            : base(new Vector2(0.13f, 0.13f))
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

                if (Own)
                {
                    if (WeaponRep == 'P')
                    {
                        if (element.GetNature() != ElementNature.Player)
                            return;

                        if (!NetworkFacade.Local && element.Owner != 0xFF)
                        {
                            NetworkFacade.MainHandling.Minigames.SendBegin(
                                element.Owner);
                        }
                    }
                    else
                    {
                        switch (element.GetNature())
                        {
                            case ElementNature.World:
                                break;
                            case ElementNature.Player:
                                if (element.Owner == 0xFF)
                                    return;

                                NetworkFacade.MainHandling.Damages.Send(element.Owner, Hurt());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public int Hurt()
        {
            Random dmg = new Random();

            return (dmg.Next((int)(minimumDmg), (int)(maximumDmg + 1)));
        }

        public void Explode(Vector3 pos)
        {
            GeneralFacade.SceneManager.SoundManager.Play3D("explosion",
                GameplayFacade.ThisPlayer.Position,
                pos
                );
            if (!NetworkFacade.Local)
            {
                NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("explosion"),
                    pos);
            }
            //PlayFireParticle(pos);

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
