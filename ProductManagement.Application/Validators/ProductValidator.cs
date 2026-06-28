namespace ProductManagement.Application.Validators;

public sealed class ProductValidator
{
    public void Validate(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentException("Product price cannot be negative.", nameof(price));
        }
    }
}
