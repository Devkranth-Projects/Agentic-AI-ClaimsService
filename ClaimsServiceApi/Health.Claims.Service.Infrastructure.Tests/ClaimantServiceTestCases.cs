using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Health.Claims.Service.Infrastructure.Tests.Services
{
    public class ClaimantServiceTests
    {
        private readonly Mock<IClaimantRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ClaimantService>> _loggerMock;
        private readonly ClaimantService _service;

        public ClaimantServiceTests()
        {
            _repoMock = new Mock<IClaimantRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ClaimantService>>();

            _service = new ClaimantService(_repoMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        private ClaimantEntity MakeEntity(Guid? id = null) =>
            new ClaimantEntity
            {
                EntityId = id ?? Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

        private ClaimantDto MakeDto(Guid? id = null) =>
            new ClaimantDto
            {
                ClaimantId = id ?? Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

        // -------------------------
        // Happy path / functional Test cases
        // -------------------------
        [Fact]
        public async Task GetAllClaimantsAsync_ReturnsMappedDtos_DefaultExcludeDeleted()
        {
            var entities = Enumerable.Range(1, 3).Select(_ => MakeEntity()).ToList();
            var dtos = entities.Select(e => new ClaimantDto { ClaimantId = e.EntityId, FirstName = e.FirstName }).ToList();

            _repoMock.Setup(r => r.GetAllClaimants(false)).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ClaimantDto>>(It.IsAny<IEnumerable<ClaimantEntity>>())).Returns(dtos);

            var result = await _service.GetAllClaimantsAsync();

            result.Should().NotBeNull().And.HaveCount(3);
            result.Select(x => x.ClaimantId).Should().BeEquivalentTo(entities.Select(e => e.EntityId));
            _repoMock.Verify(r => r.GetAllClaimants(false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<ClaimantDto>>(It.IsAny<IEnumerable<ClaimantEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllClaimantsAsync_WithIncludeDeletedTrue_CallsRepositoryWithTrue()
        {
            var entities = new List<ClaimantEntity>();
            _repoMock.Setup(r => r.GetAllClaimants(true)).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ClaimantDto>>(It.IsAny<IEnumerable<ClaimantEntity>>())).Returns(new List<ClaimantDto>());

            var result = await _service.GetAllClaimantsAsync(includeDeleted: true);

            result.Should().NotBeNull();
            _repoMock.Verify(r => r.GetAllClaimants(true), Times.Once);
        }

        [Fact]
        public async Task GetClaimantByIdAsync_ReturnsMappedDto_WhenEntityFound()
        {
            var entity = MakeEntity();
            var dto = new ClaimantDto { ClaimantId = entity.EntityId, FirstName = entity.FirstName };

            _repoMock.Setup(r => r.GetDetailsByClaimantId(entity.EntityId)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ClaimantDto>(entity)).Returns(dto);

            var result = await _service.GetClaimantByIdAsync(entity.EntityId);

            result.Should().NotBeNull();
            result!.ClaimantId.Should().Be(entity.EntityId);
            _repoMock.Verify(r => r.GetDetailsByClaimantId(entity.EntityId), Times.Once);
            _mapperMock.Verify(m => m.Map<ClaimantDto>(entity), Times.Once);
        }

        [Fact]
        public async Task AddClaimantAsync_ValidDto_CallsRepositoryWithMappedEntity()
        {
            var dto = MakeDto();
            var entity = MakeEntity(dto.ClaimantId);

            _mapperMock.Setup(m => m.Map<ClaimantEntity>(dto)).Returns(entity);
            _repoMock.Setup(r => r.AddNewClaimant(entity)).Returns(Task.CompletedTask);

            await _service.AddClaimantAsync(dto);

            _repoMock.Verify(r => r.AddNewClaimant(entity), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, "Adding new claimant");
        }

        [Fact]
        public async Task UpdateClaimantAsync_ValidDto_CallsRepositoryUpdate()
        {
            var dto = MakeDto();
            var entity = MakeEntity(dto.ClaimantId);

            _mapperMock.Setup(m => m.Map<ClaimantEntity>(dto)).Returns(entity);
            _repoMock.Setup(r => r.EditExistingClaimant(entity)).Returns(Task.CompletedTask);

            await _service.UpdateClaimantAsync(dto);

            _repoMock.Verify(r => r.EditExistingClaimant(entity), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, "Updating claimant");
        }

        [Fact]
        public async Task DeleteClaimantAsync_ValidGuid_CallsRepositoryDelete()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.DeleteExistingClaimant(id)).Returns(Task.CompletedTask);

            await _service.DeleteClaimantAsync(id);

            _repoMock.Verify(r => r.DeleteExistingClaimant(id), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Information, "Deleting claimant");
        }

        // -------------------------
        // Negative / validation tests
        // -------------------------
        [Fact]
        public async Task GetClaimantByIdAsync_EmptyGuid_ThrowsArgumentException()
        {
            await FluentActions.Awaiting(() => _service.GetClaimantByIdAsync(Guid.Empty))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddClaimantAsync_NullDto_ThrowsArgumentNullException()
        {
            await FluentActions.Awaiting(() => _service.AddClaimantAsync(null!))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateClaimantAsync_NullDto_ThrowsArgumentNullException()
        {
            await FluentActions.Awaiting(() => _service.UpdateClaimantAsync(null!))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteClaimantAsync_EmptyGuid_ThrowsArgumentException()
        {
            await FluentActions.Awaiting(() => _service.DeleteClaimantAsync(Guid.Empty))
                .Should().ThrowAsync<ArgumentException>();
        }

        // -------------------------
        // Mapper exception scenarios
        // -------------------------
        [Fact]
        public async Task GetAllClaimantsAsync_MapperThrows_ExceptionPropagatedAndLogged()
        {
            var entities = new List<ClaimantEntity> { MakeEntity() };
            _repoMock.Setup(r => r.GetAllClaimants(false)).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ClaimantDto>>(It.IsAny<IEnumerable<ClaimantEntity>>()))
                       .Throws(new InvalidOperationException("map fail"));

            await FluentActions.Awaiting(() => _service.GetAllClaimantsAsync())
                .Should().ThrowAsync<InvalidOperationException>();

            _loggerMock.VerifyLog(LogLevel.Error, "Error fetching claimants");
        }

        [Fact]
        public async Task GetClaimantByIdAsync_MapperReturnsNull_ReturnsNull()
        {
            var entity = MakeEntity();
            _repoMock.Setup(r => r.GetDetailsByClaimantId(entity.EntityId)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ClaimantDto>(entity)).Returns((ClaimantDto?)null);

            var result = await _service.GetClaimantByIdAsync(entity.EntityId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task VerifyNoUnexpectedRepoCalls_OnSimpleGetAll()
        {
            var entities = new List<ClaimantEntity> { MakeEntity() };
            _repoMock.Setup(r => r.GetAllClaimants(false)).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ClaimantDto>>(It.IsAny<IEnumerable<ClaimantEntity>>()))
                       .Returns(new List<ClaimantDto>());

            await _service.GetAllClaimantsAsync();

            _repoMock.Verify(r => r.GetAllClaimants(false), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        // -------------------------
        // Edge cases & large collection handling
        // -------------------------
        [Fact]
        public async Task GetAllClaimantsAsync_CanHandleLargeCollections()
        {
            var entities = Enumerable.Range(1, 1000).Select(_ => MakeEntity()).ToList();
            _repoMock.Setup(r => r.GetAllClaimants(false)).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ClaimantDto>>(It.IsAny<IEnumerable<ClaimantEntity>>()))
                       .Returns((IEnumerable<ClaimantEntity> src) =>
                           src.Select(e => new ClaimantDto { ClaimantId = e.EntityId, FirstName = e.FirstName }).ToList());

            var result = await _service.GetAllClaimantsAsync();

            result.Should().HaveCount(1000);
        }
        
    }

    // -------------------------
    // Helper extension for logger verification
    // -------------------------
    internal static class LoggerMockExtensions
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, string containsText)
        {
            logger.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains(containsText)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }
    }
}
