using HtmlConverter.Application.Interfaces;
using File = HtmlConverter.Domain.Models.File;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.EntityFrameworkCore;

namespace Converter.Tests.FileConverter
{
    public class FileStoreTests
    {
        private readonly string _pdfSample = "sample.html.pdf";
        private readonly string _contentType = "application/pdf";
        private readonly int _zero = 0;
        private readonly int _one = 1;

        [Fact]
        public async Task FileStoreSuccessAsync()
        {
            //Arrange
            var fileRepository = new Mock<IBaseRepository<File>>();
            fileRepository.Setup(x => x.Files).ReturnsDbSet(Enumerable.Empty<File>());
            IFileStore fileStore = new HtmlConverter.Application.FileConverter.FileStore(fileRepository.Object);
            Mock<IFormFile> formFile = new();

            //Act
            formFile.Setup(x => x.Length).Returns(_one);
            formFile.Setup(x => x.FileName).Returns(_pdfSample);
            formFile.Setup(x => x.ContentType).Returns(_contentType);
            var result = await fileStore.UploadFile(formFile.Object);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void FileSizeIsEmptyNotFoundException()
        {
            //Arrange
            var fileRepository = new Mock<IBaseRepository<File>>();
            IFileStore fileStore = new HtmlConverter.Application.FileConverter.FileStore(fileRepository.Object);
            Mock<IFormFile> formFile = new();

            //Act
            formFile.Setup(x => x.Length).Returns(_zero);
            formFile.Setup(x => x.FileName).Returns(_pdfSample);

            //Assert
            Assert.ThrowsAsync<FileNotFoundException>(async () => await fileStore.UploadFile(formFile.Object));
        }
    }
}
