using FluentValidation;
using Repository.dto;
using Service.product;
using System.Globalization;

namespace ProjectWPF.Validation
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        private readonly ProductService _productService;

        public ProductValidator(ProductService productService)
        {
            _productService = productService;
            // Validate Name
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Phải có tên sản phẩm.")
                .MaximumLength(255).WithMessage("Tên sản phẩm có ĐỘ dài tối đa là 255 kí tự.")
                .Must(CheckUniqueName)
                .WithMessage("Tên sản phẩm trùng với tên sản phẩm đã có sẵn");

            // Validate Description
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Phải có chú thích.")
                .MaximumLength(255).WithMessage("Chú thích có độ dài tối đa là 255 kí tự.");

            RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Phải có giá sản phẩm.")
            .Must(price => int.TryParse(price, out _)).WithMessage("Giá phải là số nguyên.")
            .Must(BePositive).WithMessage("Giá phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.ProductUnit)
                .NotNull().WithMessage("Đơn vị không được để trống");
            // Optional: if you ever want to validate nested ProductBatch objects
            // RuleForEach(x => x.ProductBatches)
            //     .SetValidator(new ProductBatchValidator());
        }

        private bool CheckUniqueName(ProductDto product, string name)
        {
            return !_productService
                .GetAllProducts()
                .Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                       && p.Id != product.Id);
        }
        private bool BePositive(string? price)
        {
            return decimal.TryParse(price, out var value) && value >= 0;
        }
    }
}
