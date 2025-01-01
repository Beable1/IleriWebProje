using FluentValidation;
using IleriWeb.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 
	IleriWeb.Service.Validations
{
	public class ProductDtoValidator:AbstractValidator<ProductDto>
	{
		public ProductDtoValidator()
		{
			RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{propertyName} is required");
            
            RuleFor(x=>x.Price).InclusiveBetween(1,int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
			RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0");
			RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("Please select a {PropertyName}!");
			RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithMessage("Please select a {PropertyName}!");
			RuleFor(x => x.imageFile).NotEmpty().WithMessage("Please select a {PropertyName}!");
			RuleFor(x => x.ProductFeature).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{propertyName} is required");
        }
	}
}
