using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;

namespace WildcardRoll
{
    public class Collection : INotifyPropertyChanged
    {
        public string Name { get; set; } = "New Collection";
        public BindingList<Set> Sets { get; private set; } = new BindingList<Set>();

        [JsonIgnore]
        public string ShowAs {  get { return $"{Name} ({ActiveSets}/{Sets.Count})"; } }

        [JsonIgnore]
        public int ActiveSets { get { return (from set in Sets where set.Enabled select set).Count(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public Collection() {
            Sets.ListChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, ListChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowAs"));
        }
    }
}
