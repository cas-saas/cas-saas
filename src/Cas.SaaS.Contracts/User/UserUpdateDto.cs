using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;
using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "������� ����� ��������!")] 
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// �������� ����� ������������
    /// </summary>
    [Required(ErrorMessage = "������� �������� �����!")] 
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// ���
    /// </summary>
    [Required(ErrorMessage = "������� ���!")] 
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// �������
    /// </summary>
    [Required(ErrorMessage = "������� �������!")] 
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
