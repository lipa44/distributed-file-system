using Backups.Algorithms;
using Backups.Entities;
using BackupsExtra.CleaningAlgorithms;
using BackupsExtra.ConflictsResolverState;
using BackupsExtra.Entities;

namespace Task5_FileSystem_Application;

public class Program
{
    private static readonly int PointsAmount = 10;
    private static readonly int JobsAmount = 100;

    private static FileSystemRepository? _fileSystemRepository;

    private static List<JobObject> _jobsToRestore;

    public static void Main()
    {
        XmlFileLogger.CreateInstance("LoggerPath");
        _fileSystemRepository = new ("FileSystemRepoTest");

        _jobsToRestore = new List<JobObject>
        {
            new ("JOBA.txt", "FilesToBackup"),
            new ("fileA.md", "FilesToBackup"),
            new ("fileB.txt", "FilesToBackup"),
            new ("fileC.rtf", "FilesToBackup"),
        };

        for (var i = 0; i < JobsAmount; i++)
        {
            var extraJob = new ExtraBackupJob(
                Guid.NewGuid().ToString(),
                new HardConflictsResolverState(),
                new BackupJobSettings(new PointsLimitAlgorithm(PointsAmount),
                    new SplitStorage()));

            extraJob.ResolverState = new HardConflictsResolverState();

            Directory.CreateDirectory("FilesToBackup");

            var jobsToRestore = new List<JobObject>();

            _jobsToRestore.ForEach(jobFile => jobsToRestore.Add(jobFile));

            for (var j = 0; j < PointsAmount; j++)
            {
                var fileName = $"{Guid.NewGuid()}.txt";
                File.Create(Path.Combine("FilesToBackup", fileName)).Dispose();
                jobsToRestore.Add(new (fileName, "FilesToBackup"));
            }
 
            jobsToRestore.ForEach(jobFile => extraJob.AddFileToBackup(jobFile));

            extraJob.CreateRestorePoint($"Point{Guid.NewGuid()}");

            _fileSystemRepository!.AddBackupJob(extraJob);
            _fileSystemRepository.Save(extraJob);
        }

        Console.WriteLine(_fileSystemRepository!.BackupJobs.Count);
    }
}