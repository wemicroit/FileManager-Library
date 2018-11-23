using System;
using System.Collections.Generic;
using System.Text;
using WeMicroIt.Utils.FileConverter;
using Xunit;

namespace FileManager_Tests.Units
{
    public class ReadUnit
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReadInvalidFile(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(value);
            var actual = manager.ReadFile();

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReadInvalidLine(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(value);
            var actual = manager.ReadLine();

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReadInvalidLines(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(value);
            var actual = manager.ReadLines();

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("hi.x", false)]
        [InlineData("hi.x", true)]
        public void ReadInvalidCSV(string file, bool value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.ReadCSV<object>();

            //Assert
            Assert.Null(actual);

            actual = manager.ReadCSV<object>(value);

            //Assert
            Assert.Null(actual);
        }

        /*[Theory]
        [InlineData("hi.csv", false)]
        [InlineData("hi.csv", true)]
        public void ReadCSV(string file, bool value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.ReadCSV<object>();

            //Assert
            Assert.NotNull(actual);

            actual = manager.ReadCSV<object>(value);

            //Assert
            Assert.NotNull(actual);
        }*/

        [Theory]
        [InlineData("hi.j")]
        public void ReadInvalidJSON(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.ReadJSON<object>();

            //Assert
            Assert.Null(actual);
        }

        /*[Theory]
        [InlineData("hi.json")]
        public void ReadJSON(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.ReadJSON<object>();

            //Assert
            Assert.NotNull(actual);
        }*/

        [Theory]
        [InlineData("hi.x")]
        public void ReadInvalidXML(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.ReadJSON<object>();

            //Assert
            Assert.Null(actual);
        }

        /*[Theory]
        [InlineData("hi.xml")]
        public void ReadXML(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.ReadJSON<object>();

            //Assert
            Assert.NotNull(actual);
        }*/
    }
}
