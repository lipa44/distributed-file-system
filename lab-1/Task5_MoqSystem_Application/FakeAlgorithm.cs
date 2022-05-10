using AutoFixture;
using Backups.Entities;
using Backups.Interfaces;

namespace Task5_MoqSystem_Application;

public class FakeAlgorithm : IAlgorithm
{
    private readonly IFixture _fixture;

    public FakeAlgorithm(IFixture fixture)
    {
        _fixture = fixture;
    }

    public IReadOnlyCollection<Storage?> ArchiveFiles(BackupJob backupJob)
    {
        return _fixture.CreateMany<Storage?>(new Random().Next(1, 10)).ToList();
    }
}