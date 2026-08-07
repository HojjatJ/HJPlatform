using System;

namespace HJ.Server.Application.Products;

public class ProductAlreadyExistsException : Exception
{
    public ProductAlreadyExistsException(string code) 
        : base($"A product with code '{code}' already exists.")
    {
    }
}
