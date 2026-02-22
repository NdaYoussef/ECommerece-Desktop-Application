using System;
using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Domain.Entities;
using Mapster;

public static class ProductMapper
{
	public static void Config() 
	{
		TypeAdapterConfig<Product,ProductListDto>.NewConfig()
			.Map(d=>d.CategoryName,s=>(s.Category!=null)?s.Category.CategoryName:"No Category")
			.Map(d=>d.IsInStock,s=> s.StockQuantity>0);

		TypeAdapterConfig<Product,ProductDetailsDto>.NewConfig()
			.Map(d=>d.CategoryName,s=>(s.Category!=null)?s.Category.CategoryName:"No Category");
	}

}
