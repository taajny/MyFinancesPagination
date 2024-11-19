using MyFinances.Core.Dtos;
using MyFinances.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFinances.Models.Converters
{
    public static class OperationConverter
    {
        public static OperationDto ToDto(this Operations model)
        {
            return new OperationDto
            {
                CategoryId = model.CategoryId,
                Date = model.Date,
                Description = model.Description,
                Id = model.Id,
                Name = model.Name,
                Value = model.Value
            };
        }

        public static IEnumerable<OperationDto> ToDtos(this IEnumerable<Operations> model)
        {
            if (model == null)
            {
                return Enumerable.Empty<OperationDto>();
            }

            return model.Select(x => ToDto(x));
        }

        public static Operations ToDao(this OperationDto model)
        {
            return new Operations
            {
                CategoryId = model.CategoryId,
                Date = model.Date,
                Description = model.Description,
                Id = model.Id,
                Name = model.Name,
                Value = model.Value
            };
        }
    }
}