using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;

namespace TwitterDoodle.Data {

    public class TwitterQueryCollection : ICollection<KeyValuePair<string, string>> {

        private readonly ICollection<KeyValuePair<string, string>> _pairs;

        public TwitterQueryCollection() {

            _pairs = new Collection<KeyValuePair<string, string>>();
        }

        public TwitterQueryCollection(ICollection<KeyValuePair<string, string>> pairs) {

            if (pairs == null) {

                throw new NullReferenceException("pairs");
            }

            _pairs = pairs;
        }

        public override string ToString() {

            StringBuilder queryBuilder = new StringBuilder();
            _pairs.ForEach(pair => {

                queryBuilder.AppendFormat("{0}={1}&", pair.Key, pair.Value);
            });

            var query = queryBuilder.ToString();
            query = query.Substring(0, query.Length - 1);

            return query;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {

            return _pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {

            IEnumerable ie = _pairs;
            return ie.GetEnumerator();
        }

        public void Add(string key, string value) { 

            Add(new KeyValuePair<string,string>(key, value));
        }

        public void Add(KeyValuePair<string, string> item) {

            _pairs.Add(new KeyValuePair<string, string>(item.Key, OAuthUtil.PercentEncode(item.Value)));
        }

        public void Clear() {

            _pairs.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item) {

            return _pairs.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {

            _pairs.CopyTo(array, arrayIndex);
        }

        public int Count {

            get { return _pairs.Count; }
        }

        public bool IsReadOnly {

            get { return _pairs.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, string> item) {

            return _pairs.Remove(item);
        }
    }
}