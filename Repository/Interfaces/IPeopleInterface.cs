using MakesEasy.Models;

namespace MakesEasy.Interfaces;

public interface IPeopleInterface
{
    Task<int> InsertPeople(PeopleModel people, int vId, int tId, int dId, int sId, int cId);

    Task<List<PeopleModel>> GetPeopleByVillage(string role, int villageId);

    Task<int> UpdatePeople(PeopleModel people);
    Task<int> DeletePeople(int id);

    Task<int> InsertStudent(StudentModel student);

    Task<List<StudentModel>> GetStudents(int id, string role);

    Task<int> UpdateStudent(StudentModel student);

    Task<Dictionary<string, object>> GetCount(string role, int villageId);

    Task<int> DeleteStudent(int id);


}