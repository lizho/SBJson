using System.Text;

namespace Lizho.SimpleBriefJson
{
    public enum SBJsonHelperStatus
    {
        Success, Error, Null
    }

    public class SBJsonHelper
    {
        public SBJsonHelper(string json)
        {
            ParseJsonText(json);
        }

        public void ParseJsonText(string json)
        {
            JsonText = json.Trim();
            Parse();
            Status = SBJsonHelperStatus.Success;
        }

        public SBJsonBase this[int index] => Root[index];

        public SBJsonBase this[string index] => Root[index];

        public SBJsonHelperStatus Status { get; protected set; } = SBJsonHelperStatus.Null;

        public string JsonText { get; protected set; }

        public SBJsonBase Root { get; protected set; }

        protected enum SBJsonParseStatus
        {
            Null, Key, Value
        }

        protected void Parse()
        {
            #region type of value
            switch (JsonText[0])
            {
                case '[':
                    Root = new SBJsonArray();
                    break;

                case '{':
                    Root = new SBJsonDict();
                    break;

                case '"':
                    Root = new SBJsonString(JsonText.Trim('"'));
                    Root.JsonText = JsonText;
                    Status = SBJsonHelperStatus.Success;
                    return;

                default:
                    Status = SBJsonHelperStatus.Success;
                    if (double.TryParse(JsonText, out var rd))
                    {
                        Root = new SBJsonNumber(rd);
                    }
                    else if (bool.TryParse(JsonText, out var rb))
                    {
                        Root = new SBJsonBoolean(rb);
                    }
                    else if (JsonText == "null")
                    {
                        Root = new SBJsonNull();
                    }
                    else
                        Status = SBJsonHelperStatus.Error;
                    Root.JsonText = JsonText;
                    return;
            }
            #endregion

            Root.JsonText = JsonText;

            #region parse key-value pair
            var _key = "";
            var _temp = new StringBuilder();
            var _status = SBJsonParseStatus.Null;
            var _escChar = false;       // whether last JsonChar is an escape character
            var _inQuotes = false;      // whether current JsonChar is in a string value
            var _brackets = 0;          // how many '[' or '{' hvae been stored

            // fxxk my brain
            foreach (var JsonChar in JsonText)
            //for (var i = 0; i < JsonText.Length; i++)
            {
                //  var JsonChar = JsonText[i];
                if (_inQuotes)
                {
                    _inQuotes = _escChar || JsonChar != '"';
                    // as same as
                    // __inQuotes = !(!_escChar && JsonChar == '"');
                    _escChar = false;
                }
                else
                {
                    if (char.IsWhiteSpace(JsonChar))
                        continue;

                    switch (JsonChar)
                    {
                        case '"':
                            _inQuotes = true;
                            break;

                        case '\\':
                            _escChar = true;
                            break;

                        case '[':
                            if (_status == SBJsonParseStatus.Null) _status = SBJsonParseStatus.Value;
                            goto case '{';

                        case '{':
                            if (++_brackets == 1)
                                continue;
                            break;

                        case '}':
                            if (--_brackets == 0)     // end of a json block
                            {
                                var _json = new SBJsonHelper(_temp.ToString()).Root;
                                _temp.Clear();
                                Root.CastTo<SBJsonDict>().Add(_key, _json);
                                _status = SBJsonParseStatus.Null;
                                continue;
                            }
                            break;

                        case ']':
                            if (--_brackets == 0)     // end of a json array
                            {
                                var _json = new SBJsonHelper(_temp.ToString()).Root;
                                _temp.Clear();
                                Root.CastTo<SBJsonArray>().Add(_json);
                                _status = SBJsonParseStatus.Null;
                                continue;
                            }
                            break;

                        case ',':
                            if (_brackets == 1)     // end of a json value
                            {
                                var _json = new SBJsonHelper(_temp.ToString()).Root;
                                _temp.Clear();
                                if (Root.Type == SBJsonNodeType.ArrayNode)
                                {
                                    Root.CastTo<SBJsonArray>().Add(_json);
                                }
                                else
                                {
                                    Root.CastTo<SBJsonDict>().Add(_key, _json);
                                    _status = SBJsonParseStatus.Null;
                                }
                                continue;
                            }
                            break;
                        case ':':
                            if (_status == SBJsonParseStatus.Key)     // end of a json key
                            {
                                _key = _temp.ToString().Trim('"');
                                _temp.Clear();
                                _status = SBJsonParseStatus.Value;
                                continue;
                            }
                            break;
                        default:
                            break;
                    }

                    if (_status == SBJsonParseStatus.Null) _status = SBJsonParseStatus.Key;
                }

                _temp.Append(JsonChar);     // MAGIC!
            }
            #endregion
        }

        public override string ToString() => Root.ToString();
    }
}
