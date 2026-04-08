using LMS.Domain.Enums;

namespace LMS.Application.Common.Models;

public class UserDto
{
    public int TotalCount { get; set; }
    public int Id { get; set; }

    //Bilingual names
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FirstNameAR { get; set; }
    public string? LastNameAR { get; set; }

    public string? FullName { get; set; }
    public string? FullNameAR { get; set; }

    public int UserTypeId { get; set; }
    public string? UserType { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    //public GenderType? Gender { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? DepartmentAR { get; set; }
    //public int DepartmentId { get; set; }
    //public string? DepartmentName { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public DateTimeOffset Created { get; set; }

    // Employee-specific
    public string? Designation { get; set; }
    public string? DesignationAR { get; set; }

    // Support-specific
    public string? Skills { get; set; }
    public string? SkillsAR { get; set; }
    public string? Floor { get; set; }
    public string? OfficeNumber { get; set; }

    //public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
    //public List<UserRightDto> UserRights { get; set; } = new List<UserRightDto>();
    //public List<UserDepartmentDto> UserDepartment { get; init; } = new List<UserDepartmentDto>();
    //public List<UserDepartmentRoleDto> UserDepartmentRole { get; init; } = new List<UserDepartmentRoleDto>();
    //public string UserDepartmentRolesNames { get; set; } = string.Empty;
}

public class UserRightDto
{
    public int RightId { get; set; }
    public string? RightName { get; set; }
}


public class UserDepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; init; } = string.Empty;
    public bool IsDeleted { get; init; } = false;
}

public class UserDepartmentRoleDto
{
    public int UserDepartmentRoleId { get; set; }
    public string UserDepartmentRoleName { get; init; } = string.Empty;
    public int DepartmentRoleId { get; set; }
    public string? DepartmentRoleName { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; init; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; init; } = string.Empty;
    public bool IsDeleted { get; init; } = false;
}
