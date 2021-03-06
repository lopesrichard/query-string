using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace QueryString.Test
{
    public class QueryStringTest
    {
        [Test]
        public void ParseQueryString()
        {
            var expected = new { lorem = "ipsum" };

            var parsed = QueryString.Parse("lorem=ipsum");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringBeginningWithQuestionMark()
        {
            var expected = new { lorem = "ipsum" };

            var parsed = QueryString.Parse("?lorem=ipsum");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithMultipleParameters()
        {
            var expected = new { lorem = "ipsum", dolor = "sit" };

            var parsed = QueryString.Parse("lorem=ipsum&dolor=sit");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithNumericIndexParameters()
        {
            var expected = new { lorem = new string[] { "ipsum" } };

            var parsed = QueryString.Parse("lorem[0]=ipsum");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithMultipleNumericIndexParameters()
        {
            var expected = new { lorem = new string[] { "ipsum", "dolor" } };

            var parsed = QueryString.Parse("lorem[0]=ipsum&lorem[1]=dolor");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithNonSequentialNumericIndexParameters()
        {
            var expected = new { lorem = new string[] { null, null, null, null, "dolor", null, null, null, "ipsum" } };

            var parsed = QueryString.Parse("lorem[8]=ipsum&lorem[4]=dolor");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithEmptyIndexParameters()
        {
            var expected = new { lorem = new string[] { "ipsum" } };

            var parsed = QueryString.Parse("lorem[]=ipsum");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithMultipleEmptyIndexParameters()
        {
            var expected = new { lorem = new string[] { "ipsum", "dolor" } };

            var parsed = QueryString.Parse("lorem[]=ipsum&lorem[]=dolor");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithMixedNumericAndEmptyIndexParameters()
        {
            var expected = new
            {
                lorem = new string[] { null, "amet", null, "ipsum", "dolor" },
                consectetur = new string[] { "adipiscing" },
            };

            var parsed = QueryString.Parse("lorem[3]=ipsum&lorem[]=dolor&lorem[1]=amet&consectetur[]=adipiscing");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithStringIndexParameters()
        {
            var expected = new { lorem = new { ipsum = "dolor" } };

            var parsed = QueryString.Parse("lorem[ipsum]=dolor");

            Assert.AreEqual(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(parsed)
            );
        }

        [Test]
        public void ParseQueryStringWithAllKindOfParametersTogether()
        {
            var expected = new
            {
                lorem = new
                {
                    ipsum = new object[]
                    {
                        null,
                        null,
                        new
                        {
                            dolor = new string[]
                            {
                                null,
                                null,
                                null,
                                "sit"
                            },
                            elit = new string[] { "adipiscing" },
                        },
                        null,
                        null,
                        null,
                        new
                        {
                            amet = "consectetur"
                        }
                    }
                },
                ipsum = "amet"
            };

            var query = new List<string>()
            {
                "lorem[ipsum][2][dolor][3]=sit",
                "lorem[ipsum][2][elit][]=adipiscing",
                "lorem[ipsum][6][amet]=consectetur",
                "ipsum=amet"
            };

            var parsed = QueryString.Parse(query.Join("&"));

            Assert.AreEqual(
              JsonConvert.SerializeObject(expected),
              JsonConvert.SerializeObject(parsed)
          );
        }
    }
}