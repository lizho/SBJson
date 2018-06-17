using System;
using System.Collections.Generic;
using System.Linq;

namespace Lizho.SimpleBriefJson
{
    public enum SBJsonNodeType
    {
        StringNode,
        NumberNode,
        BooleanNode,
        ArrayNode,
        DictNode,
        NullNode,
    }

    public class SBJsonTypeException : Exception
    {
        public SBJsonTypeException(string type) : base($"Object is not {type}.") { }
    }

    public abstract class SBJsonBase
    {
        public string JsonText { get; set; }

        public SBJsonBase this[int index] =>
            Type == SBJsonNodeType.ArrayNode ?
                (this as SBJsonArray).Value[index] :
                throw new SBJsonTypeException("SBJsonArray");

        public SBJsonBase this[string index] =>
            Type == SBJsonNodeType.DictNode ?
                (this as SBJsonDict).Value[index] :
                throw new SBJsonTypeException("SBJsonDict");

        public T CastTo<T>() where T : SBJsonBase
        {
            return this as T;
        }

        abstract public SBJsonNodeType Type { get; }

        public bool IsNull => Type == SBJsonNodeType.NullNode;
    }

    public class SBJsonNull : SBJsonBase
    {
        public override SBJsonNodeType Type => SBJsonNodeType.NullNode;
        public override string ToString() => "null";
    }

    public class SBJsonNumber : SBJsonBase
    {
        public SBJsonNumber(Double value) { Value = value; }
        public double Value { get; set; }
        public override SBJsonNodeType Type => SBJsonNodeType.NumberNode;
        public override string ToString() => Value.ToString();
    }

    public class SBJsonBoolean : SBJsonBase
    {
        public SBJsonBoolean(bool value) { Value = value; }
        public bool Value { get; set; }
        public override SBJsonNodeType Type => SBJsonNodeType.BooleanNode;
        public override string ToString() => Value.ToString();
    }

    public class SBJsonString : SBJsonBase
    {
        public SBJsonString(string value) { Value = value; }
        public string Value { get; set; }
        public override SBJsonNodeType Type => SBJsonNodeType.StringNode;
        public override string ToString() => $@"""{Value}""";
    }

    public class SBJsonDict : SBJsonBase
    {
        public Dictionary<string, SBJsonBase> Value { get; set; } = new Dictionary<string, SBJsonBase>();
        public void Add(string key, SBJsonBase value) => Value.Add(key, value);
        public override SBJsonNodeType Type => SBJsonNodeType.DictNode;
        public override string ToString() =>
            $"{{{string.Join(", ", Value.Select(q => $@"""{q.Key}"": {q.Value}"))}}}";
    }

    public class SBJsonArray : SBJsonBase
    {
        protected List<SBJsonBase> JBList = new List<SBJsonBase>();
        public void Add(SBJsonBase item) => JBList.Add(item);
        public SBJsonBase[] Value => JBList.ToArray();
        public override SBJsonNodeType Type => SBJsonNodeType.ArrayNode;
        public override string ToString() =>
            $"[{string.Join(", ", Value.Select(q => q.ToString()))}]";
    }
}
