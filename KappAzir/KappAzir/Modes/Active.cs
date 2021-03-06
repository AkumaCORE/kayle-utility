﻿namespace KappAzir.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Mario_s_Lib;

    /// <summary>
    /// This mode will always run
    /// </summary>
    internal class Active : ModeManager
    {
        /// <summary>
        /// Put in here what you want to do when the mode is running
        /// </summary>
        public static void Execute()
        {
            if (SpellsManager.R.IsReady() && Game.Time - InSec.LastQTime > 0.1f && Game.Time - InSec.LastQTime < 1 && InSec.target.IsValidTarget(SpellsManager.R.Range))
            {
                SpellsManager.R.Cast(Azir.Position.Extend(InSec.rpos, SpellsManager.R.Range).To3D());
            }

            if (SpellsManager.R.Level == 1)
            {
                SpellsManager.R.Width = 220;
            }

            if (SpellsManager.R.Level == 2)
            {
                SpellsManager.R.Width = 270;
            }

            if (SpellsManager.R.Level == 3)
            {
                SpellsManager.R.Width = 320;
            }

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) <= 1)
            {
                SpellsManager.Q.Width = 65;
            }

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) == 2)
            {
                SpellsManager.Q.Width = 140;
            }

            if (Orbwalker.AzirSoldiers.Count(s => s.IsAlly) == 3)
            {
                SpellsManager.Q.Width = 215;
            }
        }
    }
}