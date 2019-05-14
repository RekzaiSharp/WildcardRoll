using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildcardRoll
{
    class Set
    {
        public bool Enabled { get; set; }

        [JsonIgnore]
        public Spell Spell1 { get { return Spells.Count >= 1 ? Spells[0] : null; } }
        [JsonIgnore]
        public Spell Spell2 { get { return Spells.Count >= 2 ? Spells[1] : null; } }
        [JsonIgnore]
        public Spell Spell3 { get { return Spells.Count >= 3 ? Spells[2] : null; } }
        [JsonIgnore]
        public Spell Spell4 { get { return Spells.Count >= 4 ? Spells[3] : null; } }
        [JsonIgnore]
        public Spell Spell5 { get { return Spells.Count >= 5 ? Spells[4] : null; } }

        public List<Spell> Spells = new List<Spell>();

        public Set() { }

        public void AddSpell(Spell spell)
        {
            if (Spells.Count < 5)
                Spells.Add(spell);
        }

        public void RemoveSpell(Spell spell)
        {
            Spells.Remove(spell);
        }

        public void RemoveSpellIndex(int i)
        {
            if (i < Spells.Count)
                Spells.RemoveAt(i);
        }
    }
}
