using System;
using System.Runtime.Serialization;

namespace Draft.Responses
{
    [DataContract]
    internal class KeyData : IKeyData, IHaveAValueConverter
    {

        private KeyData[] _children = Array.Empty<KeyData>();

        [field: IgnoreDataMember]
        private Func<IKeyDataValueConverter> _valueConverter = () => Etcd.Configuration.ValueConverter;

        [DataMember(Name = "nodes")]
        public KeyData[] Children
        {
            get { return _children ?? (_children = new KeyData[0]); }
            private set { _children = value; }
        }

        [IgnoreDataMember]
        public Func<IKeyDataValueConverter> ValueConverter
        {
            get { return _valueConverter; }
            set
            {
                _valueConverter = value;

                foreach (var c in Children)
                {
                    c.ValueConverter = _valueConverter;
                }
            }
        }

        [IgnoreDataMember]
        IKeyData[] IKeyData.Children
            // ReSharper disable once CoVariantArrayConversion
            => Children;

        [DataMember(Name = "createdIndex")]
        public long CreatedIndex { get; private set; }

        [DataMember(Name = "expiration")]
        public DateTime? Expiration { get; private set; }

        [DataMember(Name = "dir")]
        public bool IsDir { get; private set; }

        [DataMember(Name = "key")]
        public string Key { get; private set; } = string.Empty;

        [DataMember(Name = "modifiedIndex")]
        public long? ModifiedIndex { get; private set; }

        [DataMember(Name = "value")]
        public string RawValue { get; private set; } = string.Empty;

        [DataMember(Name = "ttl")]
        public long? TtlSeconds { get; private set; }

    }
}
