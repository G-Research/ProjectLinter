using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectLinter.Test
{
    public class ProjectHelperTests
    {
        [Theory]
        [InlineData("./ADirectory/WithAFile.txt", false)]
        [InlineData("\\Directory/WithAFile.txt", true)]
        [InlineData("/ADirectory/WithAFile.txt", true)]
        [InlineData("C:/ADirectory/WithAFile.txt", true)]
        [InlineData("C:\\ADirectory/WithAFile.txt", true)]
        [InlineData("C\\ADirectory/WithAFile.txt", false)]
        public void TestIsPathRooted(string path, bool isRooted)
        {
            path.IsRootedPath().Should().Be(isRooted);
        }
    }
}
