using System;
using System.Collections.Generic;
using FluentAssertions;
using Health.Claims.Service.Domain.Entities;
using Xunit;

namespace Health.Claims.Service.Domain.Tests
{
    public class ClaimStatusEntityDomainTests
    {
        // ------------------------
        // 1️⃣ Initialization / Defaults
        // ------------------------
        [Fact]
        public void Constructor_ShouldInitializeDefaultsAndCollections()
        {
            var status = new ClaimStatusEntity();

            status.StatusName.Should().BeEmpty();
            status.Claims.Should().NotBeNull().And.BeEmpty();
        }

        // ------------------------
        // 2️⃣ Property Assignment - Positive Tests
        // ------------------------
        [Fact]
        public void StatusName_ShouldStoreValidValues()
        {
            var status = new ClaimStatusEntity { StatusName = "Open" };
            status.StatusName.Should().Be("Open");

            status.StatusName = "Closed";
            status.StatusName.Should().Be("Closed");
        }

        // ------------------------
        // 3️⃣ Negative / Edge Tests
        // ------------------------
        [Fact]
        public void StatusName_ShouldAllowEmptyAndLongStrings()
        {
            var status = new ClaimStatusEntity();

            // Empty string
            status.StatusName = "";
            status.StatusName.Should().BeEmpty();

            // Whitespace
            status.StatusName = "   ";
            status.StatusName.Should().Be("   ");

            // Long string (edge)
            var longStr = new string('S', 500);
            status.StatusName = longStr;
            status.StatusName.Length.Should().Be(500);
        }

        // ------------------------
        // 4️⃣ Collections
        // ------------------------
        [Fact]
        public void Claims_ShouldBeEmptyByDefault()
        {
            var status = new ClaimStatusEntity();
            status.Claims.Should().BeEmpty();
        }

        [Fact]
        public void Claims_ShouldAllowMultipleItems()
        {
            var status = new ClaimStatusEntity();
            var claim1 = new ClaimRecordEntity();
            var claim2 = new ClaimRecordEntity();

            status.Claims.Add(claim1);
            status.Claims.Add(claim2);

            status.Claims.Should().HaveCount(2);
        }

        [Fact]
        public void Claims_CanContainNullItems()
        {
            var status = new ClaimStatusEntity();
            status.Claims.Add(null);
            status.Claims.Should().ContainSingle(x => x == null);
        }

        [Fact]
        public void Claims_CanBeCleared()
        {
            var status = new ClaimStatusEntity();
            status.Claims.Add(new ClaimRecordEntity());
            status.Claims.Clear();
            status.Claims.Should().BeEmpty();
        }
    }
}
