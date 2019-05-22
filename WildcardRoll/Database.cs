using System.Collections.Generic;
using System.Linq;

namespace WildcardRoll
{
    public class Database
    {
        public static List<int> Ignore = (from e in new Dictionary<int, string>()
        {
            // General
            { 5019, "Shoot" },
            { 6603, "Auto Attack" },
            { 107, "Block" },
            { 81, "Dodge" },
            { 674, "Dual Wield" },
            { 3127, "Parry" },
            { 3018, "Shoot" },

            // Alliance
            { 668, "Language Common" }, // Human

            // Horde
            { 669, "Language Orcish" }, // Orc
            { 17737, "Language Gutterspeak" }, // Undead

            // Human
            { 58985, "Perception" },
            { 20599, "Diplomacy" },
            { 20598, "The Human Spirit" },
            { 59752, "Every Man for Himself" },
            { 20864, "Mace Specialization" },
            { 20597, "Sword Specialization" },

            // Dwarf
            { 2481, "Find Treasure" },
            { 20596, "Frost Resistance" },
            { 20595, "Gun Specialization" },
            { 59224, "Mace Specialization" },
            { 20594, "Stoneform" },

            // Night elf
            { 21009, "Elusiveness" },
            { 20583, "Nature Resistance" },
            { 20582, "Quickness" },
            { 58984, "Shadowmeld" },
            { 20585, "Wisp Spirit" },

            // Gnom
            { 20592, "Arcane Resistance" },
            { 20593, "Engineering Specialization" },
            { 20589, "Escape Artist" },
            { 20591, "Expansive Mind" },

            // Orc
            { 20574, "Axe Specialization" },
            { 33697, "Blood Fury" },
            { 20575, "Command" },
            { 20573, "Hardiness" },

            // Undead
            { 20577, "Cannibalize" },
            { 20579, "Shadow Resistance" },
            { 5227, "Underwater Breathing" },
            { 7744, "Will of the Forsaken" },

            // Taure
            { 20552, "Cultivation" },
            { 20550, "Endurance" },
            { 20551, "Nature Resistance" },
            { 20549, "War Stomp" },

            // Troll
            { 20557, "Beast Slaying" },
            { 26297, "Berserking" },
            { 26290, "Bow Specialization" },
            { 58943, "Da Voodoo Shuffle" },
            { 20555, "Regeneration" },
            { 20558, "Throwing Specialization" },

            // Hunter
            { 883, "Call Pet" },
            { 2641, "Dismiss Pet" },
            { 6991, "Feed Pet" },
            { 982, "Revive Pet" },
            { 1515, "Tame Beast" },
        } select e.Key).ToList();

        public static Dictionary<int, Spell> Spells = (from spell in new Dictionary<int, string>() {
            // Balance
            { 5176, "Wrath" },
            { 8921, "Moonfire" },
            { 467, "Thorns" },
            // Feral
            { 6795, "Growl" },
            { 99, "Demoralizing Roar" },
            { 6807, "Maul" },
            { 5487, "Bear Form" },
            { 779, "Swipe (Bear)" },
            // Restro
            { 5185, "Healing Touch" },
            { 1126, "Mark of the Wild" },
            { 774, "Rejuvenation" },

            // Beast Mastery
            { 13163, "Aspect of the Monkey" },
            { 13165, "Aspect of the Hawk" },
            { 1978, "Serpent Sting" },
            // Marksmanship
            { 3044, "Arcane Shot" },
            { 1130, "Hunter's Mark" },
            { 5116, "Concussive Shot" },
            { 75, "Auto Shot" },
            // Survival
            { 2973, "Raptor Strike" },
            { 1494, "Track Beasts" },
            { 1495, "Mongoose Bite" },

            // Arcane
            { 1459, "Arcane Intellect" },
            { 5504, "Conjure Water" },
            { 587, "Conjure Food" },
            { 5143, "Arcane Missiles" },
            // Fire
            { 133, "Fireball" },
            { 2136, "Fire Blast" },
            // Frost
            { 168, "Frost Armor" },
            { 116, "Frostbolt" },

            // Holy
            { 635, "Holy Light" },
            { 21084, "Seal of Righteousness" },
            { 19742, "Blessing of Wisdom" },
            // Protection
            { 465, "Devotion Aura" },
            { 498, "Divine Protection" },
            { 853, "Hammer of Justice" },
            { 25780, "Righteous Fury" },
            // Retribution
            { 19740, "Blessing of Might" },
            { 20271, "Judgement of Light" },
            { 53408, "Judgement of Wisdom" },

            // Discipline
            { 1243, "Power Word: Fortitude" },
            { 17, "Power Word: Shield" },
            { 2050, "Lesser Heal" },
            { 585, "Smite" },
            // Holy
            { 139, "Renew" },
            // Shaow Priest
            { 589, "Shadow Word: Pain" },
            { 586, "Fade" },

            // Assassination
            { 5171, "Slice and Dice" },
            { 2098, "Eviscerate" },
            // Combat
            { 53, "Backstab" },
            { 1752, "Sinister Strike" },
            { 2983, "Sprint" },
            { 1776, "Gouge" },
            { 5277, "Evasion" },
            // Subtlety
            { 921, "Pick Pocket" },
            { 1784, "Stealth" },

            // Elemental
            { 403, "Lightning Bolt" },
            { 3599, "Searing Totem" },
            { 5730, "Stoneclaw Totem" },
            { 8042, "Earth Shock" },
            { 2484, "Earthbind Totem" },
            { 8050, "Flame Shock" },
            // Enhancement
            { 324, "Lightning Shield" },
            { 8071, "Stoneskin Totem" },
            // Restoration
            { 331, "Healing Wave" },

            // Afflication
            { 172, "Corruption" },
            { 1454, "Life Tap" },
            { 980, "Curse of Agony" },
            { 702, "Curse of Weakness" },
            { 1120, "Drain Soul" },
            { 5782, "Fear" },
            // Demonology
            { 688, "Summon Imp" },
            { 697, "Summon Voidwalker" },
            { 687, "Demon Skin" },
            // Destruction
            { 686, "Shadow Bolt" },
            { 348, "Immolate" },

            // Arms
            { 772, "Rend" },
            { 2457, "Battle Stance" },
            { 100, "Charge" },
            { 78, "Heroic Strike" },
            { 1715, "Hamstring" },
            { 6343, "Thunder Clap" },
            { 7384, "Overpower" },
            // Fury
            { 6673, "Battle Shout" },
            { 34428, "Victory Rush" },
            // Protection
            { 72, "Shield Bash" },
            { 2687, "Bloodrage" },
            { 71, "Defensive Stance" },
            { 355, "Taunt" },
            { 2565, "Shield Block" },
        } select new Spell(spell.Key, spell.Value)).ToDictionary(x => x.ID, x => x);
    }
}