using format = HtmlConverter.Domain.Models.enums.FileFormat;
using File = HtmlConverter.Domain.Models.File;
using Moq;
using HtmlConverter.Application.Interfaces;
using MockQueryable.Moq;
using HtmlConverter.Application.FileConverter.Pdf;
using Converter.Tests.FileConverter.Pdf.helpers;
using HtmlConverter.Application.Common.Infrastructure;

namespace Converter.Tests.FileConverter.Pdf
{
    public class PdfConvertJobTests
    {

        private readonly string _uploadPath = @"FileConverter/Pdf/files/upload/Home.html.pdf";
        private readonly string _downloadPath = @"FileConverter/Pdf/files/Home.html.pdf";
        private readonly List<File> _files = new()
        {
            new File
            {
                Id = 1,
                Name = "sample.html",
                FileFormat = format.HTML,
                FileData = new byte[1],
                ContentType = "application/pdf",
                Created = DateTime.Now
            },
            new File
            {
                Id =2,
                Name = "sample.html",
                FileFormat = format.HTML,
                FileData = null,
                ContentType = "application/pdf",
                Created = null
            },
            new File
            {
                Id =3,
                Name = "Home.html",
                FileFormat = format.HTML,
                FileData = System.IO.File.ReadAllBytes(@"FileConverter/Pdf/files/Home.html"),
                ContentType = "application/pdf",
                Created = DateTime.Now
            }
        };

        [Fact]
        public async Task FileExistIsInDb()
        {
            //Arrange
            List<File> file = _files.FindAll(x => x.Id == 1).ToList();
            var dbSet = file.AsQueryable().BuildMockDbSet();
            var filesRepositoryMock = new Mock<IBaseRepository<File>>();

            //Act
            filesRepositoryMock.Setup(x => x.Files).Returns(dbSet.Object);
            filesRepositoryMock.Setup(x => x.Files.AddAsync(It.IsAny<File>(), It.IsAny<CancellationToken>()))
                .Callback<File, CancellationToken>((file, cancellationToken) => _files.Add(file));

            var job = new PdfConvertJob(filesRepositoryMock.Object);
            var id = await job.Convert(_files.First(x => x.Id == 1).Id);
            var result = _files.First(x => x.Id == id);

            //Assert
            Assert.NotNull(result.FileData);
            Assert.Equal(858, result.FileData.Length);
        }

        [Fact]
        public async Task CorrectConvertToPdfSuccess()
        {
            //Arrange
            List<File> file = _files.FindAll(x => x.Id == 3).ToList();
            var dbSet = file.AsQueryable().BuildMockDbSet();
            var filesRepositoryMock = new Mock<IBaseRepository<File>>();
            var pdfhelper = new PdfHelper();

            //Act
            filesRepositoryMock.Setup(x => x.Files).Returns(dbSet.Object);
            filesRepositoryMock.Setup(x => x.Files.AddAsync(It.IsAny<File>(), It.IsAny<CancellationToken>()))
                .Callback<File, CancellationToken>((file, cancellationToken) => _files.Add(file));

            var job = new PdfConvertJob(filesRepositoryMock.Object);
            var id = await job.Convert(_files.First(x => x.Id == 3).Id);
            var result = _files.First(x => x.Id == id);

            ConfigHelper.InitializeFolder(_uploadPath);
            await System.IO.File.WriteAllBytesAsync(_uploadPath, result.FileData);
            var actualResult = pdfhelper.GetPdfContent(_uploadPath);
            var expectedResult = pdfhelper.GetPdfContent(_downloadPath);
            System.IO.File.Delete(_uploadPath);
            
            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ConvertJobSuccess()
        {
            //Arrange
            List<File> file = _files.FindAll(x => x.Id == 1).ToList();
            var dbSet = file.AsQueryable().BuildMockDbSet();
            var filesRepositoryMock = new Mock<IBaseRepository<File>>();

            //Act
            filesRepositoryMock.Setup(x => x.Files).Returns(dbSet.Object);
            filesRepositoryMock.Setup(x=>x.Files.AddAsync(It.IsAny<File>(), It.IsAny<CancellationToken>()))
                .Callback<File, CancellationToken>((file, cancellationToken) => _files.Add(file));

            var job = new PdfConvertJob(filesRepositoryMock.Object);
            var id = await job.Convert(_files.First(x => x.Id == 1).Id);
            var result = _files.First(x => x.Id == id);

            //Assert
            Assert.Equal(result.ContentType, _files.First(x => x.Id == 1).ContentType);
            Assert.Equal(result.Name, _files.First(x => x.Id == 1).Name + "." + "pdf");
            Assert.Equal(result.Name, _files.First(x => x.Id == 1).Name + "." + "pdf");
        }

        [Fact]
        public Task ConvertJobFileDataIsNull_Fail()
        {
            //Arrange
            List<File> file = _files.FindAll(x => x.Id == 2).ToList();
            var dbSet = file.AsQueryable().BuildMockDbSet();
            var filesRepositoryMock = new Mock<IBaseRepository<File>>();

            //Act
            filesRepositoryMock.Setup(x => x.Files).Returns(dbSet.Object);
            filesRepositoryMock.Setup(x => x.Files.AddAsync(It.IsAny<File>(), It.IsAny<CancellationToken>()))
                .Callback<File, CancellationToken>((file, cancellationToken) => _files.Add(file));

            var job = new PdfConvertJob(filesRepositoryMock.Object);

            //Assert
            _ = Assert.ThrowsAsync<NullReferenceException>(async () => await job.Convert(_files.First(x => x.Id == 2).Id));
            return Task.CompletedTask;
        }
    }
}
