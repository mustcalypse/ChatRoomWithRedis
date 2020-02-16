using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace redo_mq
{
    interface IRedisCollection : IEnumerable<string>
    {
        string Key { get; }
        IRedisConnection Connection { get; }
        long Add (string json_string);
        long Remove (string json_string);
    }
    class RedisList : IRedisCollection
    {
        public string Key { get; }
        public IRedisConnection Connection { get; }
        internal RedisList (IRedisConnection con, string key) { Key = key; Connection = con; }

        public IEnumerator<string> GetEnumerator ()
        {
            foreach (var item in Connection.Database.ListRange (Key)) yield return item;
            /* for (int i = 0; i < Connection.Database.ListLength (Key); i++)
                yield return Connection.Database.ListGetByIndex (Key, i);
            */
        }
        IEnumerator IEnumerable.GetEnumerator () =>
            throw new NotImplementedException ();

        public long Add (string json_string) => Connection.Database.ListRightPush (Key, json_string);
        public long Remove (string json_string) => Connection.Database.ListRemove (Key, json_string);

    }
}