using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Repositories.Courses;
using LMS.Repository.Repositories.Enrollments;
using LMS.Service.DTOs.Enrollments;
using LMS.Service.Mapper.Enrollments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Enrollments;

public class EnrollmentService : IEnrollmentService
   
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IEnrollmentMapper _enrollmentMapper;
    public EnrollmentService(IEnrollmentRepository enrollmentRepository,IEnrollmentMapper enrollmentMapper)
    {

       _enrollmentRepository = enrollmentRepository; 
       _enrollmentMapper = enrollmentMapper;
    }

    public async Task AcceptEnrollmentAsync(int enrollmentId)
    {
        await _enrollmentRepository.AcceptEnrollmentAsync(enrollmentId);
    }

    public async Task<List<EnrollmentDTO>> GetAllPendingEnrollments()
    {
        var enrollments = await _enrollmentRepository.GetPendingEnrollments();
        return _enrollmentMapper.MapFromEnrollmentToEnrollmentDTO(enrollments);
    }

    public async Task RejectEnrollmentAsync(int enrollmentId)
    {
        await _enrollmentRepository.RejectEnrollmentAsync(enrollmentId);
    }

    public async Task UpdateEnrollment(EnrollmentDTO enrollmentDTO)
    {
        var enrollment = _enrollmentMapper.MapFromEnrollmentDTOtoEnrollment(enrollmentDTO);
        await _enrollmentRepository.Update(enrollment);
    }


}

