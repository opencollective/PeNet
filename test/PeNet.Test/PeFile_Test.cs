﻿using System;
using System.Linq;
using Xunit;

namespace PeNet.Test
{
    public class PeFileTest
    {
        [Fact]
        public void ExportedFunctions_WithForwardedFunctions_ParsedForwardedFunctions()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            var forwardExports = peFile.ExportedFunctions.Where(e => e.HasForward).ToList();

            Assert.Equal(180, forwardExports.Count);
            Assert.Equal("NTDLL.RtlEnterCriticalSection", forwardExports.First(e => e.Name == "EnterCriticalSection").ForwardName);
        }

        [Fact]
        public void NetGuidModuleVersionId_NotClrPE_Empty()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Empty(peFile.ClrModuleVersionIds);
        }

        [Fact]
        public void NetGuidModuleVersionId_ClrPE_NotEmpty()
        {
            var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal(new Guid("5250e853-c17a-4e76-adb3-0a716ec8af5d"), peFile.ClrModuleVersionIds.First());
        }

        [Fact]
        public void NetGuidComTypeLibId_NotClrPE_Empty()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Equal(string.Empty, peFile.ClrComTypeLibId);
        }

        [Fact]
        public void NetGuidComTypeLibId_ClrPE_NotEmpty()
        {
            var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal("a782d109-aa8f-427b-8dcf-1c786054c7e0", peFile.ClrComTypeLibId);
        }


        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_DifferentFiles_TrueOrFalse(string file, bool expected)
        {
            Assert.Equal(expected, PeFile.IsPEFile(file));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", false)]
        [InlineData(@"Binaries/TLSCallback_x86.exe", false)]
        [InlineData(@"Binaries/NetCoreConsole.dll", false)]
        [InlineData(@"Binaries/win_test.dll", false)]
        [InlineData(@"Binaries/krnl_test.sys", true)]
        public void IsDriver_GivenAPeFile_ReturnsDriverOrNot(string file, bool isDriver)
        {
            var peFile = new PeFile(file);

            Assert.Equal(isDriver, peFile.IsDriver);
        }
    }
}