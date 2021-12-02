using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using WebApp2015_5Var8.Models;


using log4net;
using log4net.Config;

namespace WebApp2015_5Var8.DAO
{
    public class EmployeeDAO : DAO
    {
        //Включаем логирование
        public static readonly ILog log = LogManager.GetLogger(typeof(EmployeeDAO));
        public EmployeeDAO()
        {
            // подключение конфигурации логгера (для статической переменной вызываем однократно
            if (!LogManager.GetRepository().Configured)
            {
                XmlConfigurator.Configure();
                log.Info("Start");
            }
        }
        public Employee GetRecord(int id)
        {
            Connect();
            Employee employee = new Employee();
            try
            {
                SqlCommand command = new SqlCommand("select * from university_employees where id = @Id", Connection);
                // задаем идентификатор сотрудника в качестве параметра запроса
                command.Parameters.Add(new SqlParameter("@Id", id));
                SqlDataReader reader = command.ExecuteReader();

                // читаем одну строку, так как идентификатор уникален
                reader.Read();

                // заполнить поля объекта
                GetFields(reader, employee);

                reader.Close();
                log.Info("Получена одна запись");
            }
            catch (Exception ex)
            {
                // Не обрабатывать исключительные ситуации - дурной тон
                // добавить вывод в лог
                log.Error("Ошибка при чтении записи", ex);
            }
            finally
            {
                Disconnect();
            }
            return employee;
        }

        public List<Employee> GetAllRecords()
        {
            Connect();
            List<Employee> employeeList = new List<Employee>();
            try
            {
                SqlCommand command = new SqlCommand("select * from university_employees " +
                                                    "order by last_name, first_name, middle_name",
                                                    Connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Employee employee = new Employee();

                    // заполнить поля объекта
                    GetFields(reader, employee);

                    // добавить в список
                    employeeList.Add(employee);
                }
                reader.Close();
                log.Info("Получен весь список");

            }
            catch (Exception ex)
            {
                // Не обрабатывать исключительные ситуации - дурной тон
                // добавить вывод в лог
                log.Error("Ошибка при чтении записей", ex);
                //Console.WriteLine("Ошибка при чтении записей: ", ex.Message);
                //Console.ReadKey();
            }
            finally
            {
                Disconnect();
            }
            return employeeList;
        }

        public bool UpdateRecord(Employee employee) // обновление записи в базе данных
        {
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("update university_employees set  " +
                                                "Last_name = @Lastname, " +
                                                "First_name = @FirstName, " +
                                                "Middle_name = @MiddleName, " +
                                                "Birth_date = @BirthDate, " +
                                                "Phone_Number = @PhoneNumber, " +
                                                "Email = @Email, " +
                                                "Academic_degree = @AcademicDegree, " +
                                                "Position = @Position," +
                                                "Salary = @Salary " +
                                                "where id = @Id ", Connection);
                // записать общие поля
                SetFields(employee, cmd);
                // добавить идентфикатор
                cmd.Parameters.Add(new SqlParameter("@Id", employee.ID));

                // обновить данные в таблице
                cmd.ExecuteNonQuery();
                log.Info("Запись обновлена");

            }
            catch (Exception ex)
            {
                log.Error("Ошибка при обновлении данных", ex);
                result = false;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public bool AddRecord(Employee employee)
        {
            // добавление  новой записи в базу данных
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO university_employees (" +
                                                "Last_name,First_name,Middle_name," +
                                                "Birth_date,Phone_Number,Email," +
                                                "Academic_degree,Position,Salary) " +
                                                "VALUES (" +
                                                "@Lastname,@FirstName,@MiddleName," +
                                                "@BirthDate,@PhoneNumber,@Email," +
                                                "@AcademicDegree,@Position,@Salary)", Connection);
                // записать параметры новой записи                
                // идентификатор не передаем, так как он назначаетсяч автоматически
                SetFields(employee, cmd);

                // выполнить запрос
                cmd.ExecuteNonQuery();
                log.Info("Запись добавлена");
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Ошибка при вставке данных: ", ex.Message);
                //Console.ReadKey();
                log.Error("Ошибка при вствке данных", ex);
                result = false;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public bool DeleteRecord(int id, Employee records)
        {
            // удаление записи из базы данных
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("delete from university_employees where id = @Id", Connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.ExecuteNonQuery();
                log.Info("Запись удалена");
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Ошибка при удалении данных: ", ex.Message);
                //Console.ReadKey();
                log.Error("Ошибка при удалении данных", ex);
                result = false;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        private void GetFields(SqlDataReader reader, Employee employee)
        {
            // заполнение полей объекта из текущей записи
            // вынесено в отдельную процедуру, чтобы не дублировать код

            // извлекаем параметры сотрудника
            employee.ID = Convert.ToInt32(reader["ID"]);
            employee.Lastname = Convert.ToString(reader["Last_name"]);
            employee.FirstName = Convert.ToString(reader["First_Name"]);
            employee.MiddleName = Convert.ToString(reader["Middle_Name"]);
            employee.BirthDate = Convert.ToDateTime(reader["Birth_Date"]);
            employee.PhoneNumber = Convert.ToString(reader["Phone_Number"]);
            employee.Email = Convert.ToString(reader["Email"]);
            employee.AcademicDegree = Convert.ToString(reader["Academic_Degree"]);
            employee.Position = Convert.ToString(reader["Position"]);
            employee.Salary = Convert.ToDouble(reader["Salary"]);
        }

        private void SetFields(Employee employee, SqlCommand cmd)
        {
            // занесение полей в запись вынесено в отдельную процедуру, чтобы избежать дублирования
            cmd.Parameters.Add(new SqlParameter("@Lastname", employee.Lastname));
            cmd.Parameters.Add(new SqlParameter("@FirstName", employee.FirstName));
            cmd.Parameters.Add(new SqlParameter("@MiddleName", employee.MiddleName));
            cmd.Parameters.Add(new SqlParameter("@BirthDate", employee.BirthDate));
            cmd.Parameters.Add(new SqlParameter("@PhoneNumber", employee.PhoneNumber));
            cmd.Parameters.Add(new SqlParameter("@Email", employee.Email));
            cmd.Parameters.Add(new SqlParameter("@AcademicDegree", employee.AcademicDegree));
            cmd.Parameters.Add(new SqlParameter("@Position", employee.Position));
            cmd.Parameters.Add(new SqlParameter("@Salary", employee.Salary));

        }
    }
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}