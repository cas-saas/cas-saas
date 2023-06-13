using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.User;

/// <summary>
/// ������ ���������� �������
/// </summary>
public class UserUpdateDto
{
    /// <summary>
    /// ������������� ������������
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

    /// <summary>
    /// ������ ������������
    /// </summary>
    public ClientStatus Status { get; set; } = ClientStatus.None;
}
