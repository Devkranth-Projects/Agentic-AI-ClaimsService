using System;
using FluentAssertions;
using Health.Claims.Service.Domain.Entities;
using Xunit;

namespace Health.Claims.Service.Domain.Tests
{
    public class DocumentEntityDomainTests
    {
        // ------------------------
        // 1️⃣ Initialization / Defaults
        // ------------------------
        [Fact]
        public void Constructor_ShouldInitializeDefaults()
        {
            var doc = new DocumentEntity();

            doc.FileName.Should().BeEmpty();
            doc.FilePath.Should().BeEmpty();
            doc.FileType.Should().BeEmpty();
            doc.ClaimId.Should().Be(Guid.Empty);
            doc.Claim.Should().BeNull();
        }

        // ------------------------
        // 2️⃣ Property Assignment - Positive Tests
        // ------------------------
        [Fact]
        public void FileName_ShouldStoreValidValues()
        {
            var doc = new DocumentEntity { FileName = "report.pdf" };
            doc.FileName.Should().Be("report.pdf");
        }

        [Fact]
        public void FilePath_ShouldStoreValidValues()
        {
            var doc = new DocumentEntity { FilePath = "/docs/report.pdf" };
            doc.FilePath.Should().Be("/docs/report.pdf");
        }

        [Fact]
        public void FileType_ShouldStoreValidValues()
        {
            var doc = new DocumentEntity { FileType = "PDF" };
            doc.FileType.Should().Be("PDF");
        }

        [Fact]
        public void ClaimId_ShouldStoreGuidValues()
        {
            var id = Guid.NewGuid();
            var doc = new DocumentEntity { ClaimId = id };
            doc.ClaimId.Should().Be(id);
        }

        [Fact]
        public void Claim_ShouldStoreNavigationReference()
        {
            var claim = new ClaimRecordEntity();
            var doc = new DocumentEntity { Claim = claim };
            doc.Claim.Should().Be(claim);
        }

        // ------------------------
        // 3️⃣ Negative / Edge Tests
        // ------------------------
        [Fact]
        public void FileName_ShouldAllowEmptyAndLongStrings()
        {
            var doc = new DocumentEntity();

            doc.FileName = "";
            doc.FileName.Should().BeEmpty();

            var longStr = new string('F', 500);
            doc.FileName = longStr;
            doc.FileName.Length.Should().Be(500);
        }

        [Fact]
        public void FilePath_ShouldAllowEmptyAndLongStrings()
        {
            var doc = new DocumentEntity();

            doc.FilePath = "";
            doc.FilePath.Should().BeEmpty();

            var longStr = new string('P', 500);
            doc.FilePath = longStr;
            doc.FilePath.Length.Should().Be(500);
        }

        [Fact]
        public void FileType_ShouldAllowEmptyAndLongStrings()
        {
            var doc = new DocumentEntity();

            doc.FileType = "";
            doc.FileType.Should().BeEmpty();

            var longStr = new string('T', 100);
            doc.FileType = longStr;
            doc.FileType.Length.Should().Be(100);
        }

        [Fact]
        public void ClaimId_ShouldAllowEmptyGuid()
        {
            var doc = new DocumentEntity { ClaimId = Guid.Empty };
            doc.ClaimId.Should().Be(Guid.Empty);
        }

        [Fact]
        public void Claim_ShouldAllowNull()
        {
            var doc = new DocumentEntity { Claim = null };
            doc.Claim.Should().BeNull();
        }
    }
}
