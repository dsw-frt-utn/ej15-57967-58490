using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data;

public interface IPersistence
{
    List<Speciality> GetSpecialities();
    Speciality? GetSpecialityById(Guid id);

    List<Doctor> GetActiveDoctors();
    Doctor? GetDoctorById(Guid id);
    void AddDoctor(Doctor doctor);
}