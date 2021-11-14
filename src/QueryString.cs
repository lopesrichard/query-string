using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QueryString
{
    public class ParseOptions
    {
        public bool Decode { get; set; } = true;
    }

    public static class QueryString
    {
        public static JObject Parse(string query, ParseOptions options = null)
        {
            if (options == null)
            {
                options = new ParseOptions();
            }

            query = query.Trim().TrimStart('?');

            var obj = new JObject();

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

        private static void Set(JToken token, Queue<string> indexes, bool append, string value)
        {
            string current = indexes.Dequeue();
            string next = null;

            if (indexes.Count > 0)
            {
                next = indexes.Peek();
            }

            if (token is JArray arr)
            {
                SetArrayToken(arr, current, next, value);
            }
            else if (token is JObject obj)
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

        public static void SetArrayToken(JArray token, string current, string next, string value)
        {
            if (!int.TryParse(current, out var index))
            {
                throw new ArgumentException("Attempt to set named index in array");
            }

            for (var i = token.Count; i <= index; i++)
            {
                token.Add(null);
            }

            if (token[index].HasValues)
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
                token[index] = new JArray();
            }
            else
            {
                token[index] = new JObject();
            }
        }

        public static void SetObjectToken(JObject token, string current, string next, string value, bool append)
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
                        token[current] = new JArray();
                    }
                }
                else
                {
                    if (next == null)
                    {
                        if (append)
                        {
                            token[current] = new JArray(value);
                        }
                        else
                        {
                            token[current] = value;
                        }
                    }
                    else
                    {
                        token[current] = new JObject();
                    }
                }
            }
            else if (token[current] is JArray array && next == null && append)
            {
                array.Add(value);
            }
        }
    }
}