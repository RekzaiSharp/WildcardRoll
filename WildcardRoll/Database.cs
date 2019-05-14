using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildcardRoll
{
    public class Database
    {
        public static List<int> Ignore = (from e in new Dictionary<int, string>()
        {
            { 5019, "Shoot" },
            { 6603, "Auto Attack" },
            { 20598, "The Human Spirit" },
            { 107, "Block" },
            { 20599, "Diplomacy" },
            { 81, "Dodge" },
            { 674, "Dual Wield" },
            { 59752, "Every Man for Himself" },
            { 668, "Language Common" },
            { 20864, "Mace Specialization" },
            { 3127, "Parry" },
            { 58985, "Perception" },
            { 3018, "Shoot" },
            { 20597, "Sword Specialization" },

            // Hunter
            { 883, "Call Pet" },
            { 2641, "Dismiss Pet" },
            { 6991, "Feed Pet" },
            { 982, "Revive Pet" },
            { 1515, "Tame Beast" },

        } select e.Key).ToList();

        static Dictionary<int, string> spellist = new Dictionary<int, string>() {
            // Balance
            { 5176, "Wrath"},
            { 8921, "Moonfire"},
            { 467, "Thorns"},
            // Feral
            { 6795, "Growl"},
            { 99, "Demoralizing Roar"},
            { 6807, "Maul"},
            { 5487, "Bear Form"},
            { 779, "Swipe (Bear)"},
            // Restro
            { 5185, "Healing Touch"},
            { 1126, "Mark of the Wild"},
            { 774, "Rejuvenation"},

            // Beast Mastery
            { 13163, "Aspect of the Monkey"},
            { 13165, "Aspect of the Hawk"},
            { 1978, "Serpent Sting"},
            // Marksmanship
            { 3044, "Arcane Shot"},
            { 1130, "Hunter's Mark"},
            { 5116, "Concussive Shot"},
            { 75, "Auto Shot"},
            // Survival
            { 2973, "Raptor Strike"},
            { 1494, "Track Beasts"},
            { 1495, "Mongoose Bite"},

            // Arcane
            { 1459, "Arcane Intellect"},
            { 5504, "Conjure Water"},
            { 587, "Conjure Food"},
            { 5143, "Arcane Missiles"},
            // Fire
            { 133, "Fireball"},
            { 2136, "Fire Blast"},
            // Frost
            { 168, "Frost Armor"},
            { 116, "Frostbolt"},

            // Holy
            { 635, "Holy Light"},
            { 21084, "Seal of Righteousness"},
            { 19742, "Blessing of Wisdom"},
            // Protection
            { 465, "Devotion Aura"},
            { 498, "Divine Protection"},
            { 853, "Hammer of Justice"},
            { 25780, "Righteous Fury"},
            // Retribution
            { 19740, "Blessing of Might"},
            { 20271, "Judgement of Light"},
            { 53408, "Judgement of Wisdom"},

            // Discipline
            { 1243, "Power Word: Fortitude"},
            { 17, "Power Word: Shield"},
            { 2050, "Lesser Heal"},
            { 585, "Smite"},
            // Holy
            { 139, "Renew"},
            // Shaow Priest
            { 589, "Shadow Word: Pain"},
            { 586, "Fade"},

            // Assassination
            { 5171, "Slice and Dice"},
            { 2098, "Eviscerate"},
            // Combat
            { 53, "Backstab"},
            { 1752, "Sinister Strike"},
            { 2983, "Sprint"},
            { 1776, "Gouge"},
            { 5277, "Evasion"},
            // Subtlety
            { 921, "Pick Pocket"},
            { 1784, "Stealth"},

            // Elemental
            { 403, "Lightning Bolt"},
            { 3599, "Searing Totem"},
            { 5730, "Stoneclaw Totem"},
            { 8042, "Earth Shock"},
            { 2484, "Earthbind Totem"},
            { 8050, "Flame Shock"},
            // Enhancement
            { 324, "Lightning Shield"},
            { 8071, "Stoneskin Totem"},
            // Restoration
            { 331, "Healing Wave"},

            // Afflication
            { 172, "Corruption"},
            { 1454, "Life Tap"},
            { 980, "Curse of Agony"},
            { 702, "Curse of Weakness"},
            { 1120, "Drain Soul"},
            { 5782, "Fear"},
            // Demonology
            { 688, "Summon Imp"},
            { 697, "Summon Voidwalker"},
            { 687, "Demon Skin"},
            // Destruction
            { 686, "Shadow Bolt"},
            { 348, "Immolate"},

            // Arms
            { 772, "Rend"},
            { 2457, "Battle Stance"},
            { 100, "Charge"},
            { 78, "Heroic Strike"},
            { 1715, "Hamstring"},
            { 6343, "Thunder Clap"},
            { 7384, "Overpower"},
            // Fury
            { 6673, "Battle Shout"},
            { 34428, "Victory Rush"},
            // Protection
            { 72, "Shield Bash"},
            { 2687, "Bloodrage"},
            { 71, "Defensive Stance"},
            { 355, "Taunt"},
            { 2565, "Shield Block"},
        };

internal static Dictionary<int, Spell> Spells = (from spell in spellist select new Spell(spell.Key, spell.Value)).ToDictionary(x => x.ID, x => x);
    }
}