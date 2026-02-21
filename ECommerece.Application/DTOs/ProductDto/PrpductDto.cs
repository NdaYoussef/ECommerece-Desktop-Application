using System;

public class PrpductDto
{
	
    public string Label { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; } = false;

    public int CategoryId { get; set; }
    
}
