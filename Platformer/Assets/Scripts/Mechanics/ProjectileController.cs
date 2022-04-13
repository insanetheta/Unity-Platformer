using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Mechanics;
using Platformer.Core;
using static Platformer.Core.Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Controls Creation of Projectiles fired by the Player
    /// </summary>
    public class ProjectileController : MonoBehaviour
    {
        protected const KeyCode FIRE_KEY = KeyCode.F;

        [SerializeField]
        protected Projectile ProjectilePrefab;

        [SerializeField]
        protected float Cooldown = 1f;

        protected bool _isFlipped;
        protected float _currentCooldown = 0f;


        // Start is called before the first frame update
        protected void Start()
        {
            PlayerFlipped.OnExecute += (PlayerFlipped playerFlipped) =>
            {
                _isFlipped = playerFlipped.flipX;
            };
        }

        // Update is called once per frame
        protected void Update()
        {
            if (Input.GetKeyDown(FIRE_KEY) && _currentCooldown == 0)
            {
                _currentCooldown += Cooldown;
                ProjectileFired projectileFired = Schedule<ProjectileFired>();
                projectileFired.isFlipped = _isFlipped;
                projectileFired.projectilePrefab = ProjectilePrefab;
            }
        }
    }
}


public class ProjectileFired: Simulation.Event<ProjectileFired>
{
    public bool isFlipped;
    public Projectile projectilePrefab;
    public override void Execute()
    {
        Projectile spawnedProjectile = GameObject.Instantiate<Projectile>(projectilePrefab);
        if (isFlipped)
        {
            spawnedProjectile.Initialize(new Vector2(-1, 0));
        }
		else
		{
            spawnedProjectile.Initialize(new Vector2(1, 0));

        }
        
    }
}
