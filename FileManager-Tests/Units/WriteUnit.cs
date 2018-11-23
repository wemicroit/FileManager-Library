using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WeMicroIt.Utils.FileConverter.Tests.Units
{
    public class WriteUnit
    {
        /*[Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("HI")]
        public void WriteInvalidBlock(string value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath("hvuwsechudsvbh");
            var actual = manager.WriteBlock(value);

            //Assert
            Assert.False(actual);
        }*/

        [Theory]
        [InlineData("hi.x")]
        public void AppendInvalidCSV<T>(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.AppendCSV<T>(null );

            //Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("hi.x", false)]
        [InlineData("hi.x", true)]
        public void WriteInvalidCSV<T>(string file, bool value)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.WriteCSV<T>(null);

            //Assert
            Assert.False(actual);

            actual = manager.WriteCSV<T>(null, value);

            //Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("hi.x")]
        public void AppendInvalidJSON<T>(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.AppendJSON<T>(null);

            //Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("hi.x")]
        public void WriteInvalidJSON<T>(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.WriteJSON<T>(null);

            //Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("hi.x")]
        public void AppendInvalidXML<T>(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.AppendXML<T>(null);

            //Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("hi.x")]
        public void WriteInvalidXML<T>(string file)
        {
            //Arrange
            var manager = new FileManager();

            //Act         
            manager.SetDirectoryPath(null);
            manager.SetFilePath(file);
            var actual = manager.WriteXML<T>(null);

            //Assert
            Assert.False(actual);
        }
    }
}
