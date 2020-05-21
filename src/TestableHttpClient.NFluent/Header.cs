using System.Collections.Generic;

using NFluent.Messages;

namespace TestableHttpClient.NFluent
{
    internal class Header : ISelfDescriptiveValue
    {
        public Header(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Header(string name, IEnumerable<string> values)
        {
            Name = name;
            Value = string.Join(", ", values);
        }

        public string Name { get; }
        public string Value { get; }

        public string ValueDescription => $"[{Name}: {Value}]";

        public override string ToString()
        {
            return ValueDescription;
        }
    }
}
