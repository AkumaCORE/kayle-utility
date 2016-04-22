namespace KappaUtility.Items
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class AutoQSS
    {
        public static Spell.Active Cleanse;
        
        public static Spell.Targeted W;
        
        public static Spell.Targeted R;

        protected static readonly Item Mercurial_Scimitar = new Item(ItemId.Mercurial_Scimitar);

        protected static readonly Item Quicksilver_Sash = new Item(ItemId.Quicksilver_Sash);

        protected static bool loaded = false;

        public static Menu QssMenu { get; private set; }

        internal static void OnLoad()
        {
            QssMenu = Load.UtliMenu.AddSubMenu("AutoQSS");
            QssMenu.AddGroupLabel("AutoQSS Settings");
            QssMenu.Add("enable", new CheckBox("Enable", false));
            QssMenu.Add("Mercurial", new CheckBox("Use Mercurial Scimitar", false));
            QssMenu.Add("Quicksilver", new CheckBox("Use Quicksilver Sash", false));
            if (Player.Spells.FirstOrDefault(o => o.SData.Name.Contains("SummonerBoost")) != null)
            {
                QssMenu.Add("Cleanse", new CheckBox("Use Cleanse Spell", false));
                Cleanse = new Spell.Active(Player.Instance.GetSpellSlotFromName("SummonerBoost"));
            }

            QssMenu.AddSeparator();
            QssMenu.AddGroupLabel("Debuffs Settings:");
            QssMenu.Add("blind", new CheckBox("Use On Blinds?", false));
            QssMenu.Add("charm", new CheckBox("Use On Charms?", false));
            QssMenu.Add("disarm", new CheckBox("Use On Disarm?", false));
            QssMenu.Add("fear", new CheckBox("Use On Fear?", false));
            QssMenu.Add("frenzy", new CheckBox("Use On Frenzy?", false));
            QssMenu.Add("silence", new CheckBox("Use On Silence?", false));
            QssMenu.Add("snare", new CheckBox("Use On Snare?", false));
            QssMenu.Add("sleep", new CheckBox("Use On Sleep?", false));
            QssMenu.Add("stun", new CheckBox("Use On Stuns?", false));
            QssMenu.Add("supperss", new CheckBox("Use On Supperss?", false));
            QssMenu.Add("slow", new CheckBox("Use On Slows?", false));
            QssMenu.Add("knockup", new CheckBox("Use On Knock Ups?", false));
            QssMenu.Add("knockback", new CheckBox("Use On Knock Backs?", false));
            QssMenu.Add("nearsight", new CheckBox("Use On NearSight?", false));
            QssMenu.Add("root", new CheckBox("Use On Roots?", false));
            QssMenu.Add("tunt", new CheckBox("Use On Taunts?", false));
            QssMenu.Add("poly", new CheckBox("Use On Polymorph?", false));
            QssMenu.Add("poison", new CheckBox("Use On Poisons?", false));

            QssMenu.AddSeparator();
            QssMenu.AddGroupLabel("Ults Settings:");
            QssMenu.Add("liss", new CheckBox("Use On Lissandra Ult?", false));
            QssMenu.Add("naut", new CheckBox("Use On Nautilus Ult?", false));
            QssMenu.Add("zed", new CheckBox("Use On Zed Ult?", false));
            QssMenu.Add("vlad", new CheckBox("Use On Vlad Ult?", false));
            QssMenu.Add("fizz", new CheckBox("Use On Fizz Ult?", false));
            QssMenu.Add("fiora", new CheckBox("Use On Fiora Ult?", false));
            QssMenu.AddSeparator();
            QssMenu.Add("hp", new Slider("Use Only When HP is Under %", 25, 0, 100));
            QssMenu.Add("human", new Slider("Humanizer Delay", 150, 0, 1500));
            QssMenu.Add("Rene", new Slider("Enemies Near to Cast", 1, 0, 5));
            QssMenu.Add("enemydetect", new Slider("Enemies Detect Range", 1000, 0, 2000));
            loaded = true;

            Obj_AI_Base.OnBuffGain += OnBuffGain;
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!loaded)
            {
                return;
            }

            if (QssMenu["enable"].Cast<CheckBox>().CurrentValue)
            {
                if (sender.IsMe)
                {
                    var debuff = (QssMenu["charm"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Charm)
                                 || (QssMenu["tunt"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Taunt)
                                 || (QssMenu["stun"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Stun)
                                 || (QssMenu["fear"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Fear)
                                 || (QssMenu["silence"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Silence)
                                 || (QssMenu["snare"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Snare)
                                 || (QssMenu["supperss"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Suppression)
                                 || (QssMenu["sleep"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Sleep)
                                 || (QssMenu["poly"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Polymorph)
                                 || (QssMenu["frenzy"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Frenzy)
                                 || (QssMenu["disarm"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Disarm)
                                 || (QssMenu["nearsight"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.NearSight)
                                 || (QssMenu["knockback"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Knockback)
                                 || (QssMenu["knockup"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Knockup)
                                 || (QssMenu["slow"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Slow)
                                 || (QssMenu["poison"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Poison)
                                 || (QssMenu["blind"].Cast<CheckBox>().CurrentValue && args.Buff.Type == BuffType.Blind)
                                 || (QssMenu["zed"].Cast<CheckBox>().CurrentValue && args.Buff.Name == "zedrtargetmark")
                                 || (QssMenu["vlad"].Cast<CheckBox>().CurrentValue && args.Buff.Name == "vladimirhemoplaguedebuff")
                                 || (QssMenu["liss"].Cast<CheckBox>().CurrentValue && args.Buff.Name == "LissandraREnemy2")
                                 || (QssMenu["fizz"].Cast<CheckBox>().CurrentValue && args.Buff.Name == "fizzmarinerdoombomb")
                                 || (QssMenu["naut"].Cast<CheckBox>().CurrentValue && args.Buff.Name == "nautilusgrandlinetarget")
                                 || (QssMenu["fiora"].Cast<CheckBox>().CurrentValue && args.Buff.Name == "fiorarmark");
                    var enemys = QssMenu["Rene"].Cast<Slider>().CurrentValue;
                    var hp = QssMenu["hp"].Cast<Slider>().CurrentValue;
                    var enemysrange = QssMenu["enemydetect"].Cast<Slider>().CurrentValue;
                    var delay = QssMenu["human"].Cast<Slider>().CurrentValue;
                    if (debuff && Player.Instance.HealthPercent <= hp && enemys >= Player.Instance.Position.CountEnemiesInRange(enemysrange))
                    {
                        Core.DelayAction(QssCast, delay);
                    }
                }
            }
        }

        public static void QssCast()
        {
            if (W.IsReady() && QssMenu["Quicksilver"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast(Player.Instance);
                 
                
            }

            if (W.IsReady() && QssMenu["Mercurial"].Cast<CheckBox>().CurrentValue)
            {
                W.Cast(Player.Instance);
                
            }

            if (Cleanse != null)
            {
                if (QssMenu["Cleanse"].Cast<CheckBox>().CurrentValue && Cleanse.IsReady())
                {
                    Cleanse.Cast();
                }
            }
        }
                private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy || sender.IsMonster)
            {
                if ((sender.IsMinion || sender.IsMonster || args.SData.IsAutoAttack()) && args.Target != null)
                {
                    for (int i = 0; i < EntityManager.Heroes.Allies.Count; i++)
                    {
                        if (args.Target.NetworkId == EntityManager.Heroes.Allies[i].NetworkId)
                        {
                            IncDamage[i][EntityManager.Heroes.Allies[i].ServerPosition.Distance(sender.ServerPosition) / args.SData.MissileSpeed + Game.Time] = sender.GetAutoAttackDamage(EntityManager.Heroes.Allies[i]);
                        }
                        if (i == me && sender is AIHeroClient)
                        {
                            var attacker = sender as AIHeroClient;
                            if (attacker != null)
                            {
                                if (dangerousAA(attacker) && SpellManager.E.IsReady())
                                {
                                    W.Cast(Player.Instance);
                                }
                            }

                        }
                    }
                }
                else if (!(sender is AIHeroClient))
                {
                    return;
                }
                else
                {
                    var attacker = sender as AIHeroClient;
                    if (attacker != null)
                    {
                        var slot = attacker.GetSpellSlotFromName(args.SData.Name);

                        if (slot != SpellSlot.Unknown)
                        {
                            if (slot == attacker.GetSpellSlotFromName("SummonerDot") && args.Target != null)
                            {
                                for (int i = 0; i < EntityManager.Heroes.Allies.Count; i++)
                                {
                                    if (args.Target.NetworkId == EntityManager.Heroes.Allies[i].NetworkId)
                                    {
                                        InstDamage[i][Game.Time + 2] = attacker.GetSummonerSpellDamage(EntityManager.Heroes.Allies[i], DamageLibrary.SummonerSpells.Ignite);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < EntityManager.Heroes.Allies.Count; i++)
                                {
                                    if ((args.Target != null && args.Target.NetworkId == EntityManager.Heroes.Allies[i].NetworkId) || args.End.Distance(EntityManager.Heroes.Allies[i].ServerPosition) < Math.Pow(args.SData.LineWidth, 2))
                                    {
                                        InstDamage[i][Game.Time + 2] = attacker.GetSpellDamage(EntityManager.Heroes.Allies[i], slot);
                                        
                                        if (i != me)
                                            continue;
                                        if (Settings.useE && (attacker.GetSpellDamage(EntityManager.Heroes.Allies[me], slot) >= Settings.minDamage / 100 * Player.Instance.MaxHealth || dangerousSpell(slot, attacker)))
                                        {
                                            //dangerous targeted spell, not covered by Evade
                                            if (args.Target != null)
                                            {
                                                W.Cast(Player.Instance);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        
        private static bool dangerousAA(AIHeroClient attacker)
        {
            if (attacker.ChampionName == "Leona" && attacker.HasBuff("LeonaShieldOfDaybreak"))
            {
                return true;
            }
            if (attacker.ChampionName == "Udyr" && attacker.HasBuff("UdyrBearStance"))
            {
                return true;
            }
            if (attacker.ChampionName == "Nautilus" && attacker.HasBuff("NautilusStaggeringBlow"))
            {
                return true;
            }
            if (attacker.ChampionName == "Renekton" && attacker.HasBuff("RenektonRuthlessPredator"))
            {
                return true;
            }
            return false;
        }

        private static bool dangerousSpell(SpellSlot slot, AIHeroClient sender)
        {
            if (sender.ChampionName == "Ahri" && slot == SpellSlot.E)
            {
                return true;
            } 
            if (sender.ChampionName == "Aatrox" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Alistar" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Annie" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Ashe" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Blitzcrank" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Braum" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Bard" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Amumu" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Amumu" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Caitlyn" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Cassiopeia" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Chogath" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Chogath" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Darius" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Diana" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Draven" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Ekko" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Elise" && slot == SpellSlot.E)
            {
                return true;
            }
            if (sender.ChampionName == "Ezreal" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Fiora" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Fizz" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Galio" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Gnar" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Gragas" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Graves" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Hecarim" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Jinx" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Karthus" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Kennen" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Leona" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Leona" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Leona" && slot == SpellSlot.E)
            {
                return true;
            }
            if (sender.ChampionName == "Malphite" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Morgana" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Nami" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Nami" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Nautilus" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Nautilus" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Orianna" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Sejuani" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Shen" && slot == SpellSlot.E)
            {
                return true;
            }
            if (sender.ChampionName == "Shyvana" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Skarner" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Sona" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Syndra" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Thresh" && slot == SpellSlot.Q)
            {
                return true;
            }
            if (sender.ChampionName == "Taric" && slot == SpellSlot.E)
            {
                return true;
            }
            if (sender.ChampionName == "Veigar" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Vi" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Zed" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Ziggs" && slot == SpellSlot.R)
            {
                return true;
            }
            if (sender.ChampionName == "Zyra" && slot == SpellSlot.R)
            {
                return true;
            }
            return false;
        }

    }
}
