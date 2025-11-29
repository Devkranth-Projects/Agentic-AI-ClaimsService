namespace Health.Claims.Service.Domain.Entities
{
    public class DocumentEntity :BaseEntity
    {
        public string FileName { get; set; }= string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;

        public Guid ClaimId { get; set; } // Foreign Key to Claim
        public virtual ClaimRecordEntity? Claim { get; set; } // Navigation property 
    }
}
