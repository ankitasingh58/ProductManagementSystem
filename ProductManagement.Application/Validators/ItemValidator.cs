namespace ProductManagement.Application.Validators;

public sealed class ItemValidator
{
    public void Validate(string sku, int quantity)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("Item SKU is required.", nameof(sku));
        }

        if (quantity < 0)
        {
            throw new ArgumentException("Item quantity cannot be negative.", nameof(quantity));
        }
    }
}
