using FluentValidation;
using Repository;
using Service.product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWPF.Validation
{
    public class EditProductValidator : AbstractValidator<Product>
    {
        public EditProductValidator(ProductService productService) {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Phải có tên sản phẩm.")
               .MaximumLength(255).WithMessage("Tên sản phẩm có ĐỘ dài tối đa là 255 kí tự.")
               .Must(newName => !productService.GetAllProducts().Any(p => ProductNameCompare(p.Name, newName)))
               .WithMessage("Tên sản phẩm trùng với tên sản phẩm đã có sẵn");

            // Validate Description
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Phải có chú thích.")
                .MaximumLength(255).WithMessage("Chú thích có độ dài tối đa là 255 kí tự.");

            RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Phải có giá sản phẩm.")
            .Must(BePositive).WithMessage("Giá phải lớn hơn hoặc bằng 0.");

        }
        private bool BePositive(int value)
        {
            return value >= 0;
        }
        //So sánh bỏ qua viết hoa viết thường
        private bool ProductNameCompare(string name, string otherName)
        {
            return string.Compare(name, otherName,
                CultureInfo.GetCultureInfo("vi-VN"),
                CompareOptions.IgnoreCase) == 0;
        }
    }
}
