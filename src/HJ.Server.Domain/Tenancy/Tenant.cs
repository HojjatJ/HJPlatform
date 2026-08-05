using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Tenancy;

public class Tenant : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    private Tenant()
    {
    }

    public Tenant(string name, string code)
    {
        Name = name;
        Code = code;
    }
}
