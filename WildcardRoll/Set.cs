using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WildcardRoll
{
    public class Set : INotifyPropertyChanged
    {
        bool enabled;

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Enabled"));
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public Set() { }

        public void AddSpell(Spell spell)
        {
            if (Spells.Count < 5)
                Spells.Add(spell);
        }

        public void RemoveSpellByIndex(int i)
        {
            if (i < Spells.Count)
                Spells.RemoveAt(i);
        }

        public Set Clone()
        {
            return new Set()
            {
                enabled = enabled,
                Spells = (from spell in Spells select spell).ToList()
            };
        }
    }
}
