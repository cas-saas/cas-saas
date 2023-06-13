using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Employee;

/// <summary>
/// ������ ������ �������
/// </summary>
public class EmployeeUpdateDto
{
    /// <summary>
    /// ������������� ����������
    /// </summary>
    public Guid Id { get; set; }

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
}
