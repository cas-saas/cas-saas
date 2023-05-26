using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Employee;

/// <summary>
/// ������ ������ �������
/// </summary>
public class EmployeeDto
{
    /// <summary>
    /// ������������� ����������
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ��� ������������
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// ����� �������� ������������
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// �������� ����� ������������
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// ���
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// �������
    /// </summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// ��������
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// ���� ������������
    /// </summary>
    public UserRoles Role { get; set; }

    /// <summary>
    /// ������� �� ���������
    /// </summary>
    public bool IsActive { get; set; } = false;
}
