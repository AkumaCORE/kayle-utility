﻿using System;

using EloBuddy;

using GenesisSpellLibrary.Spells;

namespace GenesisSpellLibrary
{
    internal class SpellLibrary
    {
        public static SpellBase GetSpells(Champion heroChampion)
        {
            Type championType = Type.GetType("GenesisSpellLibrary.Spells." + heroChampion);
            if (championType != null)
            {
                return Activator.CreateInstance(championType) as SpellBase;
            }

            else
            {
                Chat.Print(heroChampion + " is not supported.");
                //throw new NotImplementedException();
                return null;
            }
        }

        public static bool IsOnCooldown(AIHeroClient hero, SpellSlot slot)
        {
            if (!hero.Spellbook.GetSpell(slot).IsLearned)
            {
                return true;
            }

            float cooldown = hero.Spellbook.GetSpell(slot).CooldownExpires - Game.Time;
            return cooldown > 0;
        }

        public static void Initialize()
        {
        }
    }
}