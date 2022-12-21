using MessagingLibrary.Core.Messages;

namespace Shooting.Contracts.ShootingInfo;

public class ShootingInfoContract : IMessageContract
{
    public List<Shot> Shots { get; set; } = new();
    public int LaneNumber { get; set; }
}

public class Shot
{
    public Guid Id { get; set; } = Guid.NewGuid();
}