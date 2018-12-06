using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace WeMicroIt.Utils.FileConverter.Tests.Units
{
    public class GeneralUnit
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetNullDirectory(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            var actual = manager.SetDirectoryPath(value);

            //Assert
            Assert.Equal(Directory.GetCurrentDirectory(), actual);
        }

        [Theory]
        [InlineData("Hi")]
        [InlineData("\\test/22)")]
        public void SetDirectory(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            var actual = manager.SetDirectoryPath(value);

            //Assert
            Assert.Equal(value, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetNullFile(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            var actual = manager.SetFilePath(value);

            //Assert
            Assert.Equal(DateTimeOffset.Now.Date.ToShortDateString(), actual);
        }

        [Theory]
        [InlineData("Hi.csv")]
        [InlineData("\\test/22)")]
        public void SetFile(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            var actual = manager.SetFilePath(value);

            //Assert
            Assert.Equal(value, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetNullFiles(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetFilePath(value);
            var actual = manager.GetFiles();

            //Assert
            Assert.Equal(Directory.GetFiles(Directory.GetCurrentDirectory(), "").Length, actual.Count);
        }

        [Theory]
        [InlineData("./")]
        public void GetFiles(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            var result = manager.SetFilePath(value);
            var actual = manager.GetFiles();

            //Assert
            Assert.Equal(Directory.GetFiles(value, "").Length, actual.Count);
        }

        [Theory]
        [InlineData("reuvhybrvih vsd")]
        public void GetBadFiles(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(value);
            var actual = manager.GetFiles();

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("reuvhybrvih vsd", false)]
        [InlineData("./", true)]
        public void CheckDirectory(string value, bool expected)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(value);
            var actual = manager.CheckDirectory();

            //Assert
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("reuvhybrvih vsd", false)]
        public void CheckFile(string value, bool expected)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetFilePath(value);
            var actual = manager.CheckFile();

            //Assert
            Assert.Equal(actual, expected);
        }
    }
}
