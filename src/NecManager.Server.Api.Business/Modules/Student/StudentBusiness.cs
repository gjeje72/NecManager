namespace NecManager.Server.Api.Business.Modules.Student;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Common.Extensions;
using NecManager.Server.Api.Business.Modules.Student.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

using static NecManager.Common.ApiResponseError;

internal sealed class StudentBusiness : IStudentBusiness
{
    private readonly ILogger<IStudentBusiness> logger;
    private readonly IStudentAccessLayer studentAccessLayer;
    private readonly IGroupAccessLayer groupAccessLayer;

    public StudentBusiness(ILogger<IStudentBusiness> logger, IStudentAccessLayer studentAccessLayer, IGroupAccessLayer groupAccessLayer)
    {
        this.logger = logger;
        this.studentAccessLayer = studentAccessLayer;
        this.groupAccessLayer = groupAccessLayer;
    }

    public async Task<ApiResponse<PageableResult<StudentOutputBase>>> SearchStudents(ServiceMonitoringDefinition monitoringIds, StudentQueryInput query)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var (pageSize, currentPage, weaponType, groupId, state, filter, isPageable) = query;
        var pageableResult = await this.studentAccessLayer.GetPageableCollectionAsync(new(pageSize, currentPage, weaponType, groupId, state, filter), isPageable == null ? true : (bool)isPageable);
        if (pageableResult.Items is not null)
        {
            var pageableStudents = new PageableResult<StudentOutputBase>
            {
                Items = pageableResult.Items.Select(student => this.MapStudentToStudentOutputBase(student)),
                TotalElements = pageableResult.TotalElements,
            };
            return new ApiResponse<PageableResult<StudentOutputBase>>(monitoringIds, new(pageableStudents));
        }
        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponse<StudentOutputBase>> GetStudentByIdAsync(ServiceMonitoringDefinition monitoringIds, int trainingId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingStudent = await this.studentAccessLayer.GetSingleAsync(x => x.Id == trainingId, false, x => x.Include(t => t.StudentGroups!).ThenInclude(sg => sg.Group!)).ConfigureAwait(false);
        return matchingStudent is null
            ? (new(monitoringIds, new(ApiResponseResultState.NotFound, ApiResponseError.StudentApiErrors.StudentNotFound)))
            : (new(monitoringIds, new(this.MapStudentToStudentOutputBase(matchingStudent))));
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> CreateStudent(StudentCreationInput creationInput, ServiceMonitoringDefinition monitoringIds)
    {
        var studentToCreate = new Student
        {
            FirstName = creationInput.FirstName,
            Name = creationInput.Name,
            EmailAddress = creationInput.EmailAddress,
            BirthDate = creationInput.Birthdate,
            PhoneNumber = creationInput.PhoneNumber,
            IsMaster = creationInput.IsMaster,
            State = creationInput.State,
        };
        if (creationInput.GroupIds.Any() && await this.groupAccessLayer.ExistsRangeAsync(creationInput.GroupIds))
        {
            foreach (var groupId in creationInput.GroupIds)
            {
                studentToCreate.StudentGroups.Add(new StudentGroup { GroupId = groupId, IsResponsiveMaster = creationInput.IsMaster });
            }
        }

        var result = await this.studentAccessLayer.AddAsync(studentToCreate).ConfigureAwait(false);
        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> UpdateStudentAsync(ServiceMonitoringDefinition monitoringIds, StudentUpdateInput input)
    {
        // ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingStudent = await this.studentAccessLayer.GetSingleAsync(x => x.Id == input.Id, true, x => x.Include(s => s.StudentGroups)).ConfigureAwait(false);
        if (matchingStudent is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, StudentApiErrors.StudentNotFound));

        try
        {
            matchingStudent.StudentGroups = new List<StudentGroup>();
            if (input.GroupIds.Count > 0)
            {
                foreach (var groupId in input.GroupIds)
                {
                    var matchingLesson = await this.groupAccessLayer.GetSingleAsync(x => x.Id == groupId, true).ConfigureAwait(false);
                    if (matchingLesson is null)
                        continue;

                    matchingStudent.StudentGroups.Add(new StudentGroup { GroupId = groupId, StudentId = matchingStudent.Id, IsResponsiveMaster = input.IsMaster });
                }
            }

            matchingStudent.Name = input.Name;
            matchingStudent.FirstName = input.FirstName;
            matchingStudent.State = input.State;
            matchingStudent.BirthDate = input.Birthdate;
            matchingStudent.PhoneNumber = input.PhoneNumber;
            matchingStudent.EmailAddress = input.EmailAddress;

            await this.studentAccessLayer.UpdateAsync(matchingStudent).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating student studentId={studentId}", input.Id);
            return new(monitoringIds, new(StudentApiErrors.StudentUpdateFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> DeleteStudentAsync(ServiceMonitoringDefinition monitoringIds, int studentId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingStudent = await this.studentAccessLayer.GetSingleAsync(x => x.Id == studentId, true).ConfigureAwait(false);
        if (matchingStudent is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, StudentApiErrors.StudentNotFound));

        try
        {
            await this.studentAccessLayer.RemoveAsync(matchingStudent).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while trying to delete student Id={studentId}", studentId);
            return new(monitoringIds, new(StudentApiErrors.StudentDeletionFailure));
        }

        return new(monitoringIds, new());
    }

    private StudentOutputBase MapStudentToStudentOutputBase(Student student)
        => new()
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.Name,
            Birthdate = student.BirthDate,
            Categorie = student.BirthDate.ToCategoryType(),
            State = student.State,
            Weapon = student.StudentGroups?.FirstOrDefault()?.Group?.Weapon ?? WeaponType.None,
            GroupIds = student.StudentGroups?.Select(sg => sg.GroupId) ?? new List<int>(),
            GroupName = string.Join(", ", student.StudentGroups?.Select(sg => sg.Group?.Title) ?? Enumerable.Empty<string>()),
            EmailAddress = student.EmailAddress,
            IsMaster = student.IsMaster,
        };
}
