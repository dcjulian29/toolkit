using System.Diagnostics.CodeAnalysis;
using ToolKit.Syslog;
using Xunit;

namespace UnitTests.Syslog
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class FacilityTests
    {
        [Fact]
        public void Auth_Should_BeEqualTo4()
        {
            // Act & Assert
            Assert.Equal(4, (int)Facility.Auth);
        }

        [Fact]
        public void Cron_Should_BeEqualTo9()
        {
            // Act & Assert
            Assert.Equal(9, (int)Facility.Cron);
        }

        [Fact]
        public void Daemon_Should_BeEqualTo3()
        {
            // Act & Assert
            Assert.Equal(3, (int)Facility.Daemon);
        }

        [Fact]
        public void Kernel_Should_BeEqualTo0()
        {
            // Act & Assert
            Assert.Equal(0, (int)Facility.Kernel);
        }

        [Fact]
        public void Local0_Should_BeEqualTo16()
        {
            // Act & Assert
            Assert.Equal(16, (int)Facility.Local0);
        }

        [Fact]
        public void Local1_Should_BeEqualTo17()
        {
            // Act & Assert
            Assert.Equal(17, (int)Facility.Local1);
        }

        [Fact]
        public void Local2_Should_BeEqualTo18()
        {
            // Act & Assert
            Assert.Equal(18, (int)Facility.Local2);
        }

        [Fact]
        public void Local3_Should_BeEqualTo19()
        {
            // Act & Assert
            Assert.Equal(19, (int)Facility.Local3);
        }

        [Fact]
        public void Local4_Should_BeEqualTo20()
        {
            // Act & Assert
            Assert.Equal(20, (int)Facility.Local4);
        }

        [Fact]
        public void Local5_Should_BeEqualTo21()
        {
            // Act & Assert
            Assert.Equal(21, (int)Facility.Local5);
        }

        [Fact]
        public void Local6_Should_BeEqualTo22()
        {
            // Act & Assert
            Assert.Equal(22, (int)Facility.Local6);
        }

        [Fact]
        public void Local7_Should_BeEqualTo23()
        {
            // Act & Assert
            Assert.Equal(23, (int)Facility.Local7);
        }

        [Fact]
        public void Lpr_Should_BeEqualTo6()
        {
            // Act & Assert
            Assert.Equal(6, (int)Facility.Lpr);
        }

        [Fact]
        public void Mail_Should_BeEqualTo2()
        {
            // Act & Assert
            Assert.Equal(2, (int)Facility.Mail);
        }

        [Fact]
        public void News_Should_BeEqualTo7()
        {
            // Act & Assert
            Assert.Equal(7, (int)Facility.News);
        }

        [Fact]
        public void Syslog_Should_BeEqualTo5()
        {
            // Act & Assert
            Assert.Equal(5, (int)Facility.Syslog);
        }

        [Fact]
        public void System0_Should_BeEqualTo10()
        {
            // Act & Assert
            Assert.Equal(10, (int)Facility.System0);
        }

        [Fact]
        public void System1_Should_BeEqualTo11()
        {
            // Act & Assert
            Assert.Equal(11, (int)Facility.System1);
        }

        [Fact]
        public void System2_Should_BeEqualTo12()
        {
            // Act & Assert
            Assert.Equal(12, (int)Facility.System2);
        }

        [Fact]
        public void System3_Should_BeEqualTo13()
        {
            // Act & Assert
            Assert.Equal(13, (int)Facility.System3);
        }

        [Fact]
        public void System4_Should_BeEqualTo14()
        {
            // Act & Assert
            Assert.Equal(14, (int)Facility.System4);
        }

        [Fact]
        public void System5_Should_BeEqualTo15()
        {
            // Act & Assert
            Assert.Equal(15, (int)Facility.System5);
        }

        [Fact]
        public void User_Should_BeEqualTo1()
        {
            // Act & Assert
            Assert.Equal(1, (int)Facility.User);
        }

        [Fact]
        public void Uucp_Should_BeEqualTo8()
        {
            // Act & Assert
            Assert.Equal(8, (int)Facility.UUCP);
        }
    }
}
