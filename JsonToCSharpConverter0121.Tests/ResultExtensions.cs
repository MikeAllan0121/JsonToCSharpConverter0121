using Extensions0121.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSharpConverter0121.Tests;

internal static class ResultExtensions
{


    public static void AssertOutput(this Result<string> result, string expectedOutput)
    {
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedOutput.Replace("\r\n", "\n"), result.Value?.Replace("\r\n", "\n"));
    }
}
