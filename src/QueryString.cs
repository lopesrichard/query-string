using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace QueryString
{
    public class ParseOptions
    {
        public bool Decode { get; set; } = true;
    }

    public static class QueryString
    {
        public static JsonObject Parse(string query, ParseOptions options = null)
        {
            if (options == null)
            {
                options = new ParseOptions();
            }

            query = query.Trim().TrimStart('?');

            var obj = new JsonObject();

            foreach (var results in query.Split('&'))
            {
                var split = results.Split('=');

                if (split.Length < 2)
                {
                    continue;
                }

                var key = split.First();
                var value = split.Skip(1).Join();

                var match = Regex.Match(key, @"^(?<parameter>\w+)(?:\[(?<index>\w+)\])*(?<append>\[\])?$");

                if (!match.Success)
                {
                    throw new ArgumentException($"{key} is not a valid parameter");
                }

                var parameter = match.Groups["parameter"].Value;
                var captures = match.Groups["index"].Captures;
                var append = match.Groups["append"].Success;

                var indexes = new Queue<string>();

                indexes.Enqueue(parameter);

                for (var i = 0; i < captures.Count; i++)
                {
                    indexes.Enqueue(captures[i].Value);
                }

                Set(obj, indexes, append, value);
            }

            return obj;
        }

        private static void Set(JsonNode token, Queue<string> indexes, bool append, string value)
        {
            string current = indexes.Dequeue();
            string next = null;

            if (indexes.Count > 0)
            {
                next = indexes.Peek();
            }

            if (token is JsonArray arr)
            {
                SetArrayToken(arr, current, next, value);
            }
            else if (token is JsonObject obj)
            {
                SetObjectToken(obj, current, next, value, append);
            }

            if (indexes.Count > 0)
            {
                if (int.TryParse(current, out var index))
                {
                    Set(token[index], indexes, append, value);
                }
                else
                {
                    Set(token[current], indexes, append, value);
                }
            }
        }

        public static void SetArrayToken(JsonArray token, string current, string next, string value)
        {
            var index = int.Parse(current);

            for (var i = token.Count; i <= index; i++)
            {
                token.Add(null);
            }

            if (token[index] != null)
            {
                return;
            }

            if (next == null)
            {
                token[index] = value;

                return;
            }

            if (int.TryParse(next, out var integer))
            {
                token[index] = new JsonArray();
            }
            else
            {
                token[index] = new JsonObject();
            }
        }

        public static void SetObjectToken(JsonObject token, string current, string next, string value, bool append)
        {

            if (token[current] == null)
            {
                if (int.TryParse(next, out var integer))
                {
                    if (next == null)
                    {
                        token[current] = value;
                    }
                    else
                    {
                        token[current] = new JsonArray();
                    }
                }
                else
                {
                    if (next == null)
                    {
                        if (append)
                        {
                            token[current] = new JsonArray(value);
                        }
                        else
                        {
                            token[current] = value;
                        }
                    }
                    else
                    {
                        token[current] = new JsonObject();
                    }
                }
            }
            else if (token[current] is JsonArray array && next == null && append)
            {
                array.Add(value);
            }
        }
    }
}