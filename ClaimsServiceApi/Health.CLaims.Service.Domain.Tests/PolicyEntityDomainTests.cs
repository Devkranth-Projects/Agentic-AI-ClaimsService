using System;
using FluentAssertions;
using Health.Claims.Service.Domain.Entities;
using Xunit;

namespace Health.Claims.Service.Domain.Tests
{
    public class PolicyEntityDomainTests
    {
        // ------------------------
        // 1️⃣ Initialization / Defaults
        // ------------------------
        [Fact]
        public void Constructor_ShouldInitializeDefaultsAndCollections()
        {
            var policy = new PolicyEntity();

            policy.PolicyNumber.Should().BeEmpty();
            policy.PolicyType.Should().BeEmpty();
            policy.EffectiveDate.Should().Be(default);
            policy.ExpirationDate.Should().Be(default);
            policy.Description.Should().BeNull();

            policy.ClaimantId.Should().Be(Guid.Empty);
            policy.Claimant.Should().BeNull();
            policy.Claims.Should().NotBeNull().And.BeEmpty();
        }

        // ------------------------
        // 2️⃣ Property Assignment - Positive Tests
        // ------------------------
        [Fact]
        public void PolicyNumber_ShouldStoreValues()
        {
            var policy = new PolicyEntity { PolicyNumber = "POL12345" };
            policy.PolicyNumber.Should().Be("POL12345");
        }

        [Fact]
        public void PolicyType_ShouldStoreValues()
        {
            var policy = new PolicyEntity { PolicyType = "Health" };
            policy.PolicyType.Should().Be("Health");
        }

        [Fact]
        public void EffectiveAndExpirationDates_ShouldStoreValues()
        {
            var effective = new DateTime(2025, 1, 1);
            var expiration = new DateTime(2030, 12, 31);
            var policy = new PolicyEntity
            {
                EffectiveDate = effective,
                ExpirationDate = expiration
            };

            policy.EffectiveDate.Should().Be(effective);
            policy.ExpirationDate.Should().Be(expiration);
        }

        [Fact]
        public void Description_ShouldStoreValuesOrNull()
        {
            var policy = new PolicyEntity { Description = "Premium Health Policy" };
            policy.Description.Should().Be("Premium Health Policy");

            policy.Description = null;
            policy.Description.Should().BeNull();
        }

        [Fact]
        public void ClaimantIdAndNavigation_ShouldStoreValues()
        {
            var claimantId = Guid.NewGuid();
            var claimant = new ClaimantEntity();
            var policy = new PolicyEntity
            {
                ClaimantId = claimantId,
                Claimant = claimant
            };

            policy.ClaimantId.Should().Be(claimantId);
            policy.Claimant.Should().Be(claimant);
        }

        // ------------------------
        // 3️⃣ Negative / Edge Tests
        // ------------------------
        [Fact]
        public void PolicyNumber_ShouldAllowEmptyOrLongStrings()
        {
            var policy = new PolicyEntity();

            policy.PolicyNumber = "";
            policy.PolicyNumber.Should().BeEmpty();

            var longStr = new string('P', 500);
            policy.PolicyNumber = longStr;
            policy.PolicyNumber.Length.Should().Be(500);
        }

        [Fact]
        public void PolicyType_ShouldAllowEmptyOrLongStrings()
        {
            var policy = new PolicyEntity();

            policy.PolicyType = "";
            policy.PolicyType.Should().BeEmpty();

            var longStr = new string('T', 500);
            policy.PolicyType = longStr;
            policy.PolicyType.Length.Should().Be(500);
        }

        [Fact]
        public void EffectiveAndExpirationDates_ShouldHandleMinMaxValues()
        {
            var policy = new PolicyEntity();

            policy.EffectiveDate = DateTime.MinValue;
            policy.ExpirationDate = DateTime.MaxValue;

            policy.EffectiveDate.Should().Be(DateTime.MinValue);
            policy.ExpirationDate.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void Description_ShouldAllowEmptyOrLongStrings()
        {
            var policy = new PolicyEntity();

            policy.Description = "";
            policy.Description.Should().Be("");

            var longStr = new string('D', 500);
            policy.Description = longStr;
            policy.Description.Length.Should().Be(500);
        }

        [Fact]
        public void ClaimantId_ShouldAllowEmptyGuid()
        {
            var policy = new PolicyEntity { ClaimantId = Guid.Empty };
            policy.ClaimantId.Should().Be(Guid.Empty);
        }

        [Fact]
        public void Claimant_ShouldAllowNull()
        {
            var policy = new PolicyEntity { Claimant = null };
            policy.Claimant.Should().BeNull();
        }

        // ------------------------
        // 4️⃣ Collections
        // ------------------------
        [Fact]
        public void Claims_ShouldBeEmptyByDefault()
        {
            var policy = new PolicyEntity();
            policy.Claims.Should().BeEmpty();
        }

        [Fact]
        public void Claims_ShouldAllowMultipleItems()
        {
            var policy = new PolicyEntity();
            var claim1 = new ClaimRecordEntity();
            var claim2 = new ClaimRecordEntity();

            policy.Claims.Add(claim1);
            policy.Claims.Add(claim2);

            policy.Claims.Should().HaveCount(2);
        }

        [Fact]
        public void Claims_CanContainNullItems()
        {
            var policy = new PolicyEntity();
            policy.Claims.Add(null);
            policy.Claims.Should().ContainSingle(x => x == null);
        }

        [Fact]
        public void Claims_CanBeCleared()
        {
            var policy = new PolicyEntity();
            policy.Claims.Add(new ClaimRecordEntity());
            policy.Claims.Clear();
            policy.Claims.Should().BeEmpty();
        }
    }
}
