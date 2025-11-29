using System;
using Xunit;
using FluentAssertions;
using Health.Claims.Service.Domain.Entities;

namespace Health.Claims.Service.Domain.Tests
{
    public class ClaimantEntityDomainTests
    {
        // ------------------------
        // 1️⃣ Initialization / Defaults
        // ------------------------
        [Fact]
        public void Constructor_ShouldInitializeCollectionsAndDefaults()
        {
            var c = new ClaimantEntity();

            c.Claims.Should().NotBeNull().And.BeEmpty();
            c.Policies.Should().NotBeNull().And.BeEmpty();

            c.FirstName.Should().BeEmpty();
            c.MiddleName.Should().BeNull();
            c.LastName.Should().BeEmpty();
            c.Email.Should().BeEmpty();
            c.ConfirmEmail.Should().BeEmpty();
            c.Phone.Should().BeEmpty();
            c.AltPhone.Should().BeNull();
            c.AddressLine1.Should().BeNull();
            c.AddressLine2.Should().BeNull();
            c.City.Should().BeNull();
            c.State.Should().BeNull();
            c.Zip.Should().BeNull();
            c.Country.Should().BeNull();
            c.Passport.Should().BeNull();
            c.DriverLicense.Should().BeNull();
            c.TaxId.Should().BeNull();
            c.CardNumber.Should().BeNull();
            c.CardExpiry.Should().BeNull();
            c.CardCVV.Should().BeNull();
            c.CardHolder.Should().BeNull();
            c.Notes.Should().BeNull();
        }

        // ------------------------
        // 2️⃣ Personal Information
        // ------------------------
        [Theory]
        [InlineData("John")]
        [InlineData("Anne-Marie")]
        [InlineData("O'Connor")]
        public void FirstName_ShouldStoreValidValues(string value)
        {
            var c = new ClaimantEntity { FirstName = value };
            c.FirstName.Should().Be(value);
        }

        [Fact]
        public void FirstName_ShouldHandleEmptyWhitespaceAndLongStrings()
        {
            var c = new ClaimantEntity { FirstName = "" };
            c.FirstName.Should().BeEmpty();

            c.FirstName = "   ";
            c.FirstName.Should().Be("   ");

            var longStr = new string('A', 500);
            c.FirstName = longStr;
            c.FirstName.Length.Should().Be(500);
        }

        [Fact]
        public void MiddleName_ShouldAllowNullAndStoreValue()
        {
            var c = new ClaimantEntity { MiddleName = null };
            c.MiddleName.Should().BeNull();

            c.MiddleName = "A";
            c.MiddleName.Should().Be("A");
        }

        [Fact]
        public void LastName_ShouldStoreValuesCorrectly()
        {
            var c = new ClaimantEntity { LastName = "Doe" };
            c.LastName.Should().Be("Doe");

            c.LastName = "";
            c.LastName.Should().BeEmpty();

            var longStr = new string('B', 500);
            c.LastName = longStr;
            c.LastName.Length.Should().Be(500);
        }

        [Fact]
        public void Dob_ShouldStoreValidDatesIncludingBoundaries()
        {
            var dates = new[] { new DateTime(1990, 1, 1), DateTime.MinValue, DateTime.MaxValue, DateTime.Now };
            foreach (var dob in dates)
            {
                var c = new ClaimantEntity { Dob = dob };
                c.Dob.Should().Be(dob);
            }
        }

        [Theory]
        [InlineData("Single")]
        [InlineData("Married")]
        [InlineData("Divorced")]
        [InlineData("Widowed")]
        [InlineData("Other")]
        public void MaritalStatus_ShouldStoreValuesCorrectly(string value)
        {
            var c = new ClaimantEntity { MaritalStatus = value };
            c.MaritalStatus.Should().Be(value);
        }

        [Theory]
        [InlineData("USA")]
        [InlineData("India")]
        [InlineData("UK")]
        public void Nationality_ShouldStoreValuesCorrectly(string value)
        {
            var c = new ClaimantEntity { Nationality = value };
            c.Nationality.Should().Be(value);
        }

        // ------------------------
        // 3️⃣ Contact Information
        // ------------------------
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user@domain.com")]
        public void Email_ShouldStoreValuesCorrectly(string value)
        {
            var c = new ClaimantEntity { Email = value };
            c.Email.Should().Be(value);
        }

        [Fact]
        public void Email_ShouldHandleEmptyWhitespaceAndLongStrings()
        {
            var c = new ClaimantEntity { Email = "" };
            c.Email.Should().BeEmpty();

            c.Email = "   ";
            c.Email.Should().Be("   ");

            var longStr = new string('E', 500);
            c.Email = longStr;
            c.Email.Length.Should().Be(500);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0987654321")]
        public void AltPhone_ShouldAllowNullEmptyOrValid(string value)
        {
            var c = new ClaimantEntity { AltPhone = value };
            c.AltPhone.Should().Be(value);
        }

        [Fact]
        public void Phone_ShouldStoreValidValues()
        {
            var c = new ClaimantEntity { Phone = "1234567890" };
            c.Phone.Should().Be("1234567890");
        }

        [Fact]
        public void ConfirmEmail_ShouldStoreValuesCorrectly()
        {
            var c = new ClaimantEntity { ConfirmEmail = "confirm@domain.com" };
            c.ConfirmEmail.Should().Be("confirm@domain.com");
        }

        // ------------------------
        // 4️⃣ Address
        // ------------------------
        [Fact]
        public void Address_ShouldStoreAllFieldsCorrectly()
        {
            var c = new ClaimantEntity
            {
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 4",
                City = "NYC",
                State = "NY",
                Zip = "10001",
                Country = "USA"
            };

            c.AddressLine1.Should().Be("123 Main St");
            c.AddressLine2.Should().Be("Apt 4");
            c.City.Should().Be("NYC");
            c.State.Should().Be("NY");
            c.Zip.Should().Be("10001");
            c.Country.Should().Be("USA");
        }

        // ------------------------
        // 5️⃣ Identification
        // ------------------------
        [Fact]
        public void Identification_ShouldStoreValuesCorrectly()
        {
            var c = new ClaimantEntity
            {
                Passport = "P123",
                DriverLicense = "D123",
                TaxId = "T123"
            };

            c.Passport.Should().Be("P123");
            c.DriverLicense.Should().Be("D123");
            c.TaxId.Should().Be("T123");
        }

        // ------------------------
        // 6️⃣ Credit Card
        // ------------------------
        [Fact]
        public void CreditCard_ShouldStoreAllFieldsCorrectly()
        {
            var c = new ClaimantEntity
            {
                CardNumber = "4111111111111111",
                CardExpiry = "12/30",
                CardCVV = "123",
                CardHolder = "John Doe"
            };

            c.CardNumber.Should().Be("4111111111111111");
            c.CardExpiry.Should().Be("12/30");
            c.CardCVV.Should().Be("123");
            c.CardHolder.Should().Be("John Doe");
        }

        // ------------------------
        // 7️⃣ Notes
        // ------------------------
        [Fact]
        public void Notes_ShouldAllowNullOrValidString()
        {
            var c = new ClaimantEntity { Notes = null };
            c.Notes.Should().BeNull();

            c.Notes = "Some note";
            c.Notes.Should().Be("Some note");
        }

        // ------------------------
        // 8️⃣ Collections
        // ------------------------
        [Fact]
        public void Collections_ShouldBeEmptyByDefault()
        {
            var c = new ClaimantEntity();
            c.Claims.Should().BeEmpty();
            c.Policies.Should().BeEmpty();
        }

        [Fact]
        public void Collections_ShouldAllowMultipleItems()
        {
            var c = new ClaimantEntity();
            var claim1 = new ClaimRecordEntity();
            var claim2 = new ClaimRecordEntity();
            var policy1 = new PolicyEntity();

            c.Claims.Add(claim1);
            c.Claims.Add(claim2);
            c.Policies.Add(policy1);

            c.Claims.Should().HaveCount(2);
            c.Policies.Should().HaveCount(1);
        }

        [Fact]
        public void Collections_CanContainNullItems()
        {
            var c = new ClaimantEntity();
            c.Claims.Add(null);
            c.Claims.Should().ContainSingle(x => x == null);
        }

        [Fact]
        public void Collections_CanBeCleared()
        {
            var c = new ClaimantEntity();
            c.Claims.Add(new ClaimRecordEntity());
            c.Policies.Add(new PolicyEntity());

            c.Claims.Clear();
            c.Policies.Clear();

            c.Claims.Should().BeEmpty();
            c.Policies.Should().BeEmpty();
        }
    }
}
