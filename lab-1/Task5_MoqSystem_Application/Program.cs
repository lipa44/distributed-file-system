using Aspose.Zip.Saving;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Backups.Algorithms;
using Backups.Entities;
using Backups.Interfaces;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.ConflictsResolverState;
using BackupsExtra.Entities;
using BackupsExtra.Interfaces;
using Moq;

namespace Task5_MoqSystem_Application;

public class Program
{
    private static readonly int PointsAmount = 10000;
    private static readonly int JobsAmount = 100;

    private static FileSystemRepository? _fileSystemRepository;

    public static void Main()
    {
        _fileSystemRepository = new Fixture().Create<FileSystemRepository>();
        AddHugeAmountOfBackupJobsAndSaveToRepository(new Fixture(), new Mock<SplitStorage>());
    }

    public static void AddHugeAmountOfBackupJobsAndSaveToRepository(
        [Frozen] IFixture fixture,
        [Frozen] Mock<SplitStorage> algorithm)
    {
        fixture.Customizations.Add(
            new TypeRelay(
                typeof(ConflictsResolverState),
                typeof(SoftConflictsResolverState)));

        fixture.Customizations.Add(
            new TypeRelay(
                typeof(ICleaningAlgorithm),
                typeof(PointsLimitAlgorithm)));

        fixture.Customizations.Add(
            new TypeRelay(
                typeof(IAlgorithm),
                typeof(FakeAlgorithm)));

        fixture.Customizations.Add(
            new TypeRelay(
                typeof(CompressionSettings),
                typeof(TraditionalEncryptionSettings)));

        fixture.Customizations.Add(
            new TypeRelay(
                typeof(EncryptionSettings),
                typeof(Bzip2CompressionSettings)));

        for (var i = 0; i < JobsAmount; i++)
        {
            var job = fixture.Build<ExtraBackupJob>()
                .With(job => job.ResolverState, new SoftConflictsResolverState())
                .With(job => job.BackupJobSettings,
                    new BackupJobSettings(new PointsLimitAlgorithm(PointsAmount / 2), new FakeAlgorithm(fixture)))
                .Create();

            var jobObjects = fixture.CreateMany<JobObject>(new Random().Next(1, 10)).ToList();
            job.AddFilesToBackup(jobObjects);

            try
            {
                job.CreateRestorePoint(fixture.Create<string>());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _fileSystemRepository!.AddBackupJob(job);
        }

        var amount = _fileSystemRepository!.BackupJobs.Count;
    }
}