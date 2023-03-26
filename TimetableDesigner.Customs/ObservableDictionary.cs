using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Customs
{
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<ObservableKeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        #region PROPERTIES

        public ICollection<TKey> Keys => Items.Select(p => p.Key).ToList();
        public ICollection<TValue> Values => Items.Select(p => p.Value).ToList();
        public bool IsReadOnly => false;

        #endregion



        #region INDEXERS

        public TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out TValue result))
                {
                    throw new ArgumentException("Key not found");
                }
                return result;
            }
            set
            {
                if (ContainsKey(key))
                {
                    GetKeyValuePairByTheKey(key).Value = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        #endregion



        #region CONSTRUCTORS

        public ObservableDictionary() : base()
        { }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : base()
        {
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                Add(pair);
            }
        }

        #endregion



        #region PUBLIC METHODS

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("The dictionary already contains the key");
            }
            Add(new ObservableKeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            ObservableKeyValuePair<TKey, TValue> pair = GetKeyValuePairByTheKey(item.Key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
            {
                return false;
            }
            return Equals(pair.Value, item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            ObservableKeyValuePair<TKey, TValue> pair = ((ObservableCollection<ObservableKeyValuePair<TKey, TValue>>)this).FirstOrDefault((i) => Equals(key, i.Key));

            return !Equals(default(ObservableKeyValuePair<TKey, TValue>), pair);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            List<ObservableKeyValuePair<TKey, TValue>> remove = ((ObservableCollection<ObservableKeyValuePair<TKey, TValue>>)this).Where(pair => Equals(key, pair.Key)).ToList();
            foreach (ObservableKeyValuePair<TKey, TValue> pair in remove)
            {
                Remove(pair);
            }
            return remove.Count > 0;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            ObservableKeyValuePair<TKey, TValue> pair = GetKeyValuePairByTheKey(item.Key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
            {
                return false;
            }
            if (!Equals(pair.Value, item.Value))
            {
                return false;
            }
            return Remove(pair);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            value = default;
            var pair = GetKeyValuePairByTheKey(key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
            {
                return false;
            }
            value = pair.Value;
            return true;
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => ((ObservableCollection<ObservableKeyValuePair<TKey, TValue>>)this).Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Value)).GetEnumerator();

        #endregion



        #region PRIVATE METHODS

        private ObservableKeyValuePair<TKey, TValue> GetKeyValuePairByTheKey(TKey key) => ((ObservableCollection<ObservableKeyValuePair<TKey, TValue>>)this).FirstOrDefault(i => i.Key.Equals(key));

        #endregion
    }
}
