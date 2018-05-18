using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using ToolKit;
using ToolKit.DirectoryServices;
using Xunit;

namespace UnitTests.DirectoryServices
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class UsersTests
    {
        [Fact]
        public void Create_An_Empty_User_List()
        {
            // Arrange

            // Act
            var userList = new Users();

            // Assert
            Assert.Empty(userList);
        }

        [Fact]
        public void Create_An_User_List_From_Another_Collection()
        {
            // Arrange
            var userArray = new IUser[10];
            var user = new Mock<IUser>();
            user.Setup(x => x.DisplayName).Returns("Mock User");
            userArray[5] = user.Object;

            // Act
            var userList = new Users(userArray);

            // Assert
            Assert.Equal("Mock User", userList[5].DisplayName);
        }

        [Fact]
        public void Create_An_User_List_With_Ten_Slots()
        {
            // Arrange
            var expected = 10;

            // Act
            var userList = new Users(10);

            // Assert
            Assert.Equal(expected, userList.Capacity);
        }

        [Fact]
        public void Sort_User_List_By_Changed_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByChanged(true));

            // Assert
            Assert.Equal("User2", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_Changed_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByChanged(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User2", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_Created_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByCreated(true));

            // Assert
            Assert.Equal("User2", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_Created_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByCreated(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User2", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_DisplayName_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByDisplayName(true));

            // Assert
            Assert.Equal("User1", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_DisplayName_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByDisplayName(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User1", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_EmailAddress_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByEmailAddress(true));

            // Assert
            Assert.Equal("User1", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_EmailAddress_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByEmailAddress(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User1", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_LastLogonTimestamp_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByLastLogonTimestamp(true));

            // Assert
            Assert.Equal("User2", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_LastLogonTimestamp_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByLastLogonTimestamp(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User2", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_PasswordLastSet_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByPasswordLastSet(true));

            // Assert
            Assert.Equal("User2", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_PasswordLastSet_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByPasswordLastSet(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User1", userList[1].DisplayName);
            Assert.Equal("User2", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_SamAccountName_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortBySamAccountName(true));

            // Assert
            Assert.Equal("User1", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_SamAccountName_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortBySamAccountName(false));

            // Assert
            Assert.Equal("User1", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_UserPrincipalName_Ascending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByUserPrincipalName(true));

            // Assert
            Assert.Equal("User1", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User3", userList[2].DisplayName);
        }

        [Fact]
        public void Sort_User_List_By_UserPrincipalName_Decending()
        {
            // Arrange
            var userList = InitializeData();

            // Act
            userList.Sort(new Users.SortByUserPrincipalName(false));

            // Assert
            Assert.Equal("User3", userList[0].DisplayName);
            Assert.Equal("User2", userList[1].DisplayName);
            Assert.Equal("User1", userList[2].DisplayName);
        }

        private Users InitializeData()
        {
            var userList = new Users();

            var user1 = new Mock<IUser>();
            var user2 = new Mock<IUser>();
            var user3 = new Mock<IUser>();

            userList.Add(user1.Object);
            userList.Add(user2.Object);
            userList.Add(user3.Object);

            user1.Setup(u => u.Changed).Returns(DateTime.Now);
            user1.Setup(u => u.Created).Returns(DateTime.Now);
            user1.Setup(u => u.LastLogonTimestamp).Returns(DateTime.Now);
            user1.Setup(u => u.PasswordLastSet).Returns(DateTime.Now);
            user1.Setup(u => u.DisplayName).Returns("User1");
            user1.Setup(u => u.EmailAddress).Returns("User1@company.com");
            user1.Setup(u => u.UserPrincipalName).Returns("User1@hq.company.com");
            user1.Setup(u => u.SamAccountName).Returns("use836");

            user2.Setup(u => u.Changed).Returns(DateTime.Now.AddDays(-1));
            user2.Setup(u => u.Created).Returns(DateTime.Now.AddDays(-1));
            user2.Setup(u => u.LastLogonTimestamp).Returns(DateTime.Now.AddDays(-1));
            user2.Setup(u => u.PasswordLastSet).Returns(DateTime.Now.AddDays(-1));
            user2.Setup(u => u.DisplayName).Returns("User2");
            user2.Setup(u => u.EmailAddress).Returns("User2@company.com");
            user2.Setup(u => u.UserPrincipalName).Returns("User2@hq.company.com");
            user2.Setup(u => u.SamAccountName).Returns("Use017");

            user3.Setup(u => u.Changed).Returns(DateTime.Now.AddDays(1));
            user3.Setup(u => u.Created).Returns(DateTime.Now.AddDays(1));
            user3.Setup(u => u.LastLogonTimestamp).Returns(DateTime.Now.AddDays(1));
            user3.Setup(u => u.PasswordLastSet).Returns(DateTime.Now.AddDays(1));
            user3.Setup(u => u.DisplayName).Returns("User3");
            user3.Setup(u => u.EmailAddress).Returns("User3@company.com");
            user3.Setup(u => u.UserPrincipalName).Returns("User3@hq.company.com");
            user3.Setup(u => u.SamAccountName).Returns("Use316");

            return userList;
        }
    }
}
