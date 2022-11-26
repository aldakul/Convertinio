using File = HtmlConverter.Domain.Models.File;
using FileFormat = HtmlConverter.Domain.Models.enums.FileFormat;
using Hangfire;
using HtmlConverter.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Converter.Tests.FileConverter
{
    public class FileConverterTests
    {
        private readonly string _htmlSample = "sample.html";
        private readonly string _pdfSample = "sample.html.pdf";

        [Fact]
        public async Task FileConverterSuccess()
        {
            //Arrange
            IFileConverter fileConverter = new HtmlConverter.Application.FileConverter.FileConverter(
                Mock.Of<IBackgroundJobClient>(),
                Mock.Of<IBaseRepository<File>>(),
                Mock.Of<IFileStore>());
            Mock<IFormFile> formFile = new();

            //Act
            formFile.Setup(x => x.FileName).Returns(_htmlSample);
            var result = await fileConverter.Convert(formFile.Object, FileFormat.PDF);
            //Assert
            Assert.NotNull(fileConverter);
            Assert.Equal(null, result);
        }

        [Fact]
        public void FileNotFoundException()
        {
            //Arrange
            IFileConverter fileConverter = new HtmlConverter.Application.FileConverter.FileConverter(
                Mock.Of<IBackgroundJobClient>(),
                Mock.Of<IBaseRepository<File>>(),
                Mock.Of<IFileStore>());
            Mock<IFormFile> formFile = new();

            //Act
            //Assert
            Assert.ThrowsAsync<FileNotFoundException>(async () => await fileConverter.Convert(null, FileFormat.PDF));
        }

        [Fact]
        public void FileConverterNotFoundException()
        {
            //Arrange
            IFileConverter fileConverter = new HtmlConverter.Application.FileConverter.FileConverter(
                Mock.Of<IBackgroundJobClient>(),
                Mock.Of<IBaseRepository<File>>(),
                Mock.Of<IFileStore>());
            Mock<IFormFile> formFile = new();

            //Act
            formFile.Setup(x => x.FileName).Returns(_pdfSample);
            //Assert
            Assert.ThrowsAsync<FileNotFoundException>(async () => await fileConverter.Convert(formFile.Object, FileFormat.PDF));
        }
    }
}
