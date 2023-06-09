using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.User;

/// <summary>
/// ������ ������ ������� �������
/// </summary>
public class UserDetailDto
{
    /// <summary>
    /// ������������� ������������
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
    /// ������ ������������
    /// </summary>
    public ClientStatus Status { get; set; } = ClientStatus.None;

    /// <summary>
    /// ���������� ������
    /// </summary>
    public List<EmployeeDto> Employees { get; set; } = null!;

    /// <summary>
    /// ������ �������
    /// </summary>
    public List<DeliveryDto> Deliveries { get; set; } = null!;
}
