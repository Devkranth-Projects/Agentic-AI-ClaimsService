using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    public class DocumentUploadDto
    {
        // The name of the file
        public string FileName { get; set; } = string.Empty;

        // The path or URL to the file in storage
        public string FilePath { get; set; } = string.Empty;

        // The file type or MIME type
        public string FileType { get; set; } = string.Empty;
    }
}
