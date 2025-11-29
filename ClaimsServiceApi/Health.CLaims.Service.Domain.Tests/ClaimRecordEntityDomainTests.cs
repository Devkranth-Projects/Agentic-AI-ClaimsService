using System;
using System.Collections.Generic;
using FluentAssertions;
using Health.Claims.Service.Domain.Entities;
using Xunit;

namespace Health.Claims.Service.Domain.Tests
{
    public class ClaimRecordEntityDomainTests
    {
        // ------------------------
        // 1️⃣ Initialization / Defaults
        // ------------------------
        [Fact]
        public void Constructor_ShouldInitializeCollectionsAndDefaults()
        {
            var claim = new ClaimRecordEntity();

            claim.Description.Should().BeEmpty();
            claim.Amount.Should().Be(0);
            claim.DateOfIncident.Should().Be(default);
            claim.IncidentLocation.Should().BeEmpty();

            claim.ClaimantId.Should().Be(Guid.Empty);
            claim.PolicyId.Should().Be(Guid.Empty);
            claim.StatusId.Should().Be(Guid.Empty);

            claim.Claimant.Should().BeNull();
            claim.Policy.Should().BeNull();
            claim.Status.Should().BeNull();
            claim.Documents.Should().NotBeNull().And.BeEmpty();
        }

        // ------------------------
        // 2️⃣ Property Assignment - Positive Tests
        // ------------------------
        [Fact]
        public void Description_ShouldStoreValues()
        {
            var claim = new ClaimRecordEntity { Description = "Car accident" };
            claim.Description.Should().Be("Car accident");
        }

        [Fact]
        public void Amount_ShouldStoreDecimalValues()
        {
            var claim = new ClaimRecordEntity { Amount = 1234.56m };
            claim.Amount.Should().Be(1234.56m);
        }

        [Fact]
        public void DateOfIncident_ShouldStoreValidDates()
        {
            var date = new DateTime(2025, 10, 30);
            var claim = new ClaimRecordEntity { DateOfIncident = date };
            claim.DateOfIncident.Should().Be(date);
        }

        [Fact]
        public void IncidentLocation_ShouldStoreValues()
        {
            var claim = new ClaimRecordEntity { IncidentLocation = "New York" };
            claim.IncidentLocation.Should().Be("New York");
        }

        [Fact]
        public void ForeignKeys_ShouldStoreGuidValues()
        {
            var claim = new ClaimRecordEntity
            {
                ClaimantId = Guid.NewGuid(),
                PolicyId = Guid.NewGuid(),
                StatusId = Guid.NewGuid()
            };

            claim.ClaimantId.Should().NotBe(Guid.Empty);
            claim.PolicyId.Should().NotBe(Guid.Empty);
            claim.StatusId.Should().NotBe(Guid.Empty);
        }

        // ------------------------
        // 3️⃣ Negative / Edge Tests
        // ------------------------
        [Fact]
        public void Description_ShouldAllowEmptyOrLongStrings()
        {
            var claim = new ClaimRecordEntity();

            // Empty string
            claim.Description = "";
            claim.Description.Should().BeEmpty();

            // Long string
            var longStr = new string('D', 1000);
            claim.Description = longStr;
            claim.Description.Length.Should().Be(1000);
        }

        [Fact]
        public void IncidentLocation_ShouldAllowEmptyOrLongStrings()
        {
            var claim = new ClaimRecordEntity();

            claim.IncidentLocation = "";
            claim.IncidentLocation.Should().BeEmpty();

            var longStr = new string('L', 500);
            claim.IncidentLocation = longStr;
            claim.IncidentLocation.Length.Should().Be(500);
        }

        [Fact]
        public void Amount_ShouldHandleNegativeAndZero()
        {
            var claim = new ClaimRecordEntity();

            claim.Amount = 0;
            claim.Amount.Should().Be(0);

            claim.Amount = -100;
            claim.Amount.Should().Be(-100);
        }

        [Fact]
        public void DateOfIncident_ShouldHandleMinMaxValues()
        {
            var claim = new ClaimRecordEntity { DateOfIncident = DateTime.MinValue };
            claim.DateOfIncident.Should().Be(DateTime.MinValue);

            claim.DateOfIncident = DateTime.MaxValue;
            claim.DateOfIncident.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void ForeignKeys_ShouldAllowEmptyGuids()
        {
            var claim = new ClaimRecordEntity
            {
                ClaimantId = Guid.Empty,
                PolicyId = Guid.Empty,
                StatusId = Guid.Empty
            };

            claim.ClaimantId.Should().Be(Guid.Empty);
            claim.PolicyId.Should().Be(Guid.Empty);
            claim.StatusId.Should().Be(Guid.Empty);
        }

        // ------------------------
        // 4️⃣ Collections
        // ------------------------
        [Fact]
        public void Documents_ShouldBeEmptyByDefault()
        {
            var claim = new ClaimRecordEntity();
            claim.Documents.Should().BeEmpty();
        }

        [Fact]
        public void Documents_ShouldAllowMultipleItems()
        {
            var claim = new ClaimRecordEntity();
            var doc1 = new DocumentEntity();
            var doc2 = new DocumentEntity();

            claim.Documents.Add(doc1);
            claim.Documents.Add(doc2);

            claim.Documents.Should().HaveCount(2);
        }

        [Fact]
        public void Documents_CanContainNullItems()
        {
            var claim = new ClaimRecordEntity();
            claim.Documents.Add(null);
            claim.Documents.Should().ContainSingle(x => x == null);
        }

        [Fact]
        public void Documents_CanBeCleared()
        {
            var claim = new ClaimRecordEntity();
            claim.Documents.Add(new DocumentEntity());
            claim.Documents.Clear();

            claim.Documents.Should().BeEmpty();
        }
    }
}
