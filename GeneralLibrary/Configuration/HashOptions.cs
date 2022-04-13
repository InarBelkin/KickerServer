namespace GeneralLibrary.Configuration;

public record HashOptions
{
    public byte[] Salt { get; init; } = new byte[256 / 8];
}