using System;
using System.Collections.Generic;

namespace Draft
{
    internal sealed class FormBodyBuilder
    {

        private readonly Dictionary<object, object> _items = new Dictionary<object, object>();

        public FormBodyBuilder() { }

        public FormBodyBuilder Add<TKey, TValue>(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _items.Add(key, value);
            return this;
        }

        public IDictionary<object, object> Build()
        {
            return _items;
        }

    }
}
