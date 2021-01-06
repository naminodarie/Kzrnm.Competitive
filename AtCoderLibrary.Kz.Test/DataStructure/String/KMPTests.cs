﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtCoder.DataStructure.String
{
    public class KMPTests
    {
        public static TheoryData Match_Data = new TheoryData<string, string, IEnumerable<int>>
        {
            { "ab", new string('q',1998)+"ab", new int[]{ 1998 } },
            { "abc", "abd", Array.Empty<int>() },
        };

        [Theory]
        [MemberData(nameof(Match_Data))]
        public void Matches(string pattern, string target, IEnumerable<int> indexes)
        {
            var kmp = new KMP(pattern);
            kmp.Matches(target).Should().Equal(indexes);
        }
    }
}
