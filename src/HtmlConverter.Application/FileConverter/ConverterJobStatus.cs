using Hangfire;
using HtmlConverter.Application.Common.Exceptions;
using HtmlConverter.Application.Interfaces;

namespace HtmlConverter.Application.FileConverter
{
    public class ConverterJobStatus : IConverterJobStatus
    {
        public string GetJobStatus(string jobId)
        {
            var jobStorageConnection = JobStorage.Current.GetConnection();
            var result = jobStorageConnection.GetJobData(jobId);

            if (result == null)
                throw new NotFoundException(jobId);

            return result.State;
        }
    }
}
