using CivCost.Domain.Abstractions;

namespace CivCost.Domain.Suppliers;

public class Supplier : Entity
{
    private Supplier(
        Guid id,
        string name,
        string email,
        string phone
    ) : base(id)
    {

        Name = name;
        Email = email;
        Phone = phone;
    }

    //For EFcore
    private Supplier() { }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    public static Supplier Create(
        Guid id,
        string name,
        string email,
        string phone
    )
    {
        return new Supplier(id, name, email, phone);
    }

}
