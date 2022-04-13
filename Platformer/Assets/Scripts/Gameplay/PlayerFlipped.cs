using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player character lands after being airborne.
    /// </summary>
    /// <typeparam name="PlayerLanded"></typeparam>
    public class PlayerFlipped : Simulation.Event<PlayerFlipped>
    {
        public bool flipX;
        public SpriteRenderer spriteRenderer;

        public override void Execute()
        {
            spriteRenderer.flipX = flipX;
        }
    }
}