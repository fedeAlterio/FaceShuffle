using FaceShuffle.Models.Session;

namespace FaceShuffle.Models.PendingJobs;

public record DeleteUserPicturesPendingJob(UserSessionGuid UserSessionGuid);
