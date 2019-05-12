using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class Database
    {
        public Dictionary<int, string> SpellList = new Dictionary<int, string>();

        public void Initialize()
        {
           
            SpellList = new Dictionary<int, string>()
            {
                {5176, "Wrath"}, {8921, "Moonfire"}, {467, "Thorns"}, {99, "Demoralizing Roar"}, {6807, "Maul"}, {5487, "Bear Form"},
                {779, "Swipe (Bear)"}, {5185, "Healing Touch"}, {1126, "Mark of the Wild"}, {774, "Rejuvenation"}, {13163, "Aspect of the Monkey"},
                {1515, "Tame Beast"}, {13165, "Aspect of the Hawk"}, {1978, "Serpent Sting"}, {3044, "Arcane Shot"}, {1130, "Hunter's Mark"},
                {5116, "Concussive Shot"}, {75, "Auto Shot"}, {2973, "Raptor Strike"}, {1494, "Track Beasts"}, {1459, "Arcane Intellect"}, {5504, "Conjure Water"},
                {587, "Conjure Food"}, {5143, "Arcane Missiles"}, {133, "Fireball"}, {2136, "Fire Blast"}, {168, "Frost Armor"}, {116, "Frostbolt"}, {635, "Holy Light"},
                {21084, "Seal of Righteousness"}, {19742, "Blessing of Wisdom"}, {465, "Devotion Aura"}, {498, "Divine Protection"}, {853, "Hammer of Justice"},
                {25780, "Righteous Fury"}, {25781, "Righteous Fury"}, {19740, "Blessing of Might"}, {20271, "Judgement of Light"}, {53408, "Judgement of Wisdom"},
                {1243, "Power Word: Fortitude"}, {17, "Power Word: Shield"}, {2050, "Lesser Heal"}, {585, "Smite"},
                {139, "Renew"}, {586, "Fade"}, {5171, "Slice and Dice"}, {2098, "Eviscerate"}, {53, "Backstab"}, {1752, "Sinister Strike"}, {2983, "Sprint"},
                {1776, "Gouge"}, {5277, "Evasion"}, {921, "Pick Pocket"}, {1784, "Stealth"}, {403, "Lightning Bolt"}, {3599, "Searing Totem"},
                {5730, "Stoneclaw Totem"}, {8042, "Earth Shock"}, {2484, "Earthbind Totem"}, {8050, "Flame Shock"}, {324, "Lightning Shield"},
                {8071, "Stoneskin Totem"}, {331, "Healing Wave"}, {172, "Corruption"}, {1454, "Life Tap"}, {980, "Curse of Agony"}, {702, "Curse of Weakness"},
                {1120, "Drain Soul"}, {5782, "Fear"}, {688, "Summon Imp"}, {697, "Summon Voidwalker"}, {687, "Demon Skin"}, {686, "Shadow Bolt"}, {348, "Immolate"},
                {772, "Rend"}, {2457, "Battle Stance"}, {100, "Charge"}, {78, "Heroic Strike"}, {1715, "Hamstring"}, {6343, "Thunder Clap"}, {7384, "Overpower"},
                {6673, "Battle Shout"}, {34428, "Victory Rush"}, {72, "Shield Bash"}, {2687, "Bloodrage"}, {71, "Defensive Stance"}, {355, "Taunt"}, {2565, "Shield Block"},
                {1495, "Mongoose Bite"}, {2649, "Growl"}
            };
        }
    }

    public class Set
    {
        public string Category;
        public List<string> Spells;
    }