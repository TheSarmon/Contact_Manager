using FileUploadService.Entities;
using FileUploadService.Dtos;

namespace FileUploadService.Mappers
{
    public class ContactMapper
    {
        public static ContactDto ToDto(ContactEntity entity)
        {
            return new ContactDto
            {
                Name = entity.Name,
                DateOfBirth = entity.DateOfBirth,
                Married = entity.Married,
                Phone = entity.Phone,
                Salary = entity.Salary
            };
        }

        public static ContactEntity ToEntity(ContactDto dto)
        {
            return new ContactEntity
            {
                Name = dto.Name,
                DateOfBirth = dto.DateOfBirth,
                Married = dto.Married,
                Phone = dto.Phone,
                Salary = dto.Salary
            };
        }
    }
}
