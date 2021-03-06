﻿using System;
using System.Linq;

namespace Draft.Tests
{
    internal static partial class Fixtures
    {

        public static class Queue
        {

            public const int DefaultTtl = 300;

            public const string DefaultValue = "{7FF2E050-F09B-4E5E-9600-B50374A97B0A}";

            public const string Path = "/foo/notaqueue";

            public static readonly object DefaultResponse = new
            {
                Message = "Foo"
            };

            public static string DefaultRequest(string value = DefaultValue)
            {
                return WithValue(value).AsRequestBody();
            }

            public static string TtlRequest(string value = DefaultValue, int ttl = DefaultTtl)
            {
                return WithValue(value)
                    .Add(Constants.Etcd.Parameter_Ttl, ttl)
                    .AsRequestBody();
            }

            private static FormBodyBuilder WithValue(string value)
            {
                return new FormBodyBuilder()
                    .Add(Constants.Etcd.Parameter_Value, value);
            }

        }

    }
}
