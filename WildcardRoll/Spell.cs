using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WildcardRoll
{
    public class Spell : IComparable<Spell>
    {
        public int ID { get; private set; }
        public string Name { get; set; }

        static Dictionary<int, Bitmap> icons = new Dictionary<int, Bitmap>();

        [JsonIgnore]
        public Bitmap Icon {
            get {
                if (!icons.ContainsKey(ID))
                {
                    var bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + ID);
                    if (bmp == null)
                        bmp = Properties.Resources.inv_misc_questionmark;

                    icons.Add(ID, bmp);
                }
                return icons.ContainsKey(ID) ? icons[ID] : Properties.Resources.inv_misc_questionmark;
            }
        }

        public Spell(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int CompareTo(Spell other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
