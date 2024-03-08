using System.Numerics;

namespace VendorPortal.API.Models.Domain
{
    public class DocumentsUpload
    {
        public Guid Id { get; set; }
        public string VendorId { get; set; }
        public Guid DocumentId { get; set; }
        public string? DocumentPath { get; set; }
        public string? Comment { get; set; }
        public bool Status { get; set; }

        public UserProfile UserProfile { get; set; }
        public Document Document { get; set; }
    }
}
