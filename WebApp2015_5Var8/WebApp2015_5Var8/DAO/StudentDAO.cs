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
    public class StudentDAO : DAO
    {
        //Включаем логирование
	    public static readonly ILog log = LogManager.GetLogger(typeof(StudentDAO));
        public StudentDAO()
        {
            // подключение конфигурации логгера (для статической переменной вызываем однократно
            if (!LogManager.GetRepository().Configured)
            {
                XmlConfigurator.Configure();
                log.Info("Start");
            }
        }
        public Student GetRecord(int id)
        {
            Connect();
            Student student = new Student();
            try
            {
                SqlCommand command = new SqlCommand("select * from students where id = @Id", Connection);
                // задаем идентификатор студента в качестве параметра запроса
                command.Parameters.Add(new SqlParameter("@Id", id));
                SqlDataReader reader = command.ExecuteReader();

                // читаем одну строку, так как идентификатор уникален
                reader.Read();

                // заполнить поля объекта
                GetFields(reader, student);

                reader.Close();
                log.Info("Получена одна запись");
            }
            catch (Exception ex)
            {
                // Не обрабатывать исключительные ситуации - дурной тон
                // добавить вывод в лог
                log.Error("Ошибка при чтении записи", ex);
                //Console.WriteLine("Ошибка при чтении записи: ", ex.Message);
                //Console.ReadKey();

            }
            finally
            {
                Disconnect();
            }
            return student;
        }

        public List<Student> GetAllRecords()
        {
            Connect();
            List<Student> studentList = new List<Student>();
            try
            {
                SqlCommand command = new SqlCommand("select * from students "+
                                                    "order by last_name, first_name, middle_name", 
                                                    Connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Student student = new Student();

                    // заполнить поля объекта
                    GetFields(reader, student);

                    // добавить в список
                    studentList.Add(student);
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
            return studentList;
        }

        public bool UpdateRecord(Student student) // обновление записи в базе данных
        {
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("update students set  " +
                                                "Last_name = @Lastname, " +
                                                "First_name = @FirstName, " +
                                                "Middle_name = @MiddleName, " +
                                                "Birth_date = @BirthDate, " +
                                                "Phone_Number = @PhoneNumber, " +
                                                "Credit_Card_Number = @CreditCardNumber, " +
                                                "Educational_Form = @EducationalForm, " +
                                                "Basis_Of_Training = @BasisOfTraining " +
                                                "where id = @Id ", Connection);
                // записать общие поля
                SetFields(student, cmd);
                // добавить идентфикатор
                cmd.Parameters.Add(new SqlParameter("@Id", student.ID));

                // обновить данные в таблице
                cmd.ExecuteNonQuery();
                log.Info("Запись обновлена");

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Ошибка при обновлении данных: ", ex.Message);
                //Console.ReadKey();
                log.Error("Ошибка при обновлении данных", ex);

                result = false;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public bool AddRecord(Student student)
        {
            // добавление  новой записи в базу данных
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO students ("+
                                                "Last_name,First_name,Middle_name,"+
                                                "Birth_date,Phone_Number,Credit_Card_Number,"+
                                                "Educational_Form,Basis_Of_Training) " +
                                                "VALUES ("+
                                                "@Lastname,@FirstName,@MiddleName,"+
                                                "@BirthDate,@PhoneNumber,@CreditCardNumber,"+
                                                "@EducationalForm,@BasisOfTraining)", Connection);
                // записать параметры новой записи                
                // идентификатор не передаем, так как он назначаетсяч автоматически
                SetFields(student, cmd);

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

        public bool DeleteRecord(int id , Student records)
        {
            // удаление записи из базы данных
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("delete from students where id = @Id", Connection);
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

        private void GetFields(SqlDataReader reader, Student student)
        {
            // заполнение полей объекта из текущей записи
            // вынесено в отдельную процедуру, чтобы не дублировать код

            // извлекаем параметры студента
            student.ID = Convert.ToInt32(reader["ID"]);
            student.Lastname = Convert.ToString(reader["Last_name"]);
            student.FirstName = Convert.ToString(reader["First_Name"]);
            student.MiddleName = Convert.ToString(reader["Middle_Name"]);
            student.BirthDate = Convert.ToDateTime(reader["Birth_Date"]); 
            student.PhoneNumber = Convert.ToString(reader["Phone_Number"]);
            student.CreditCardNumber = Convert.ToString(reader["Credit_Card_Number"]);          
            student.EducationalForm = Convert.ToInt32(reader["Educational_Form"]);
            student.BasisOfTraining = Convert.ToInt32(reader["Basis_Of_Training"]);
        }

        private void SetFields(Student student, SqlCommand cmd)
        {
            // занесение полей в запись вынесено в отдельную процедуру, чтобы избежать дублирования
            cmd.Parameters.Add(new SqlParameter("@Lastname", student.Lastname));
            cmd.Parameters.Add(new SqlParameter("@FirstName", student.FirstName));
            cmd.Parameters.Add(new SqlParameter("@MiddleName", student.MiddleName));
            cmd.Parameters.Add(new SqlParameter("@BirthDate", student.BirthDate));
            cmd.Parameters.Add(new SqlParameter("@PhoneNumber", student.PhoneNumber));
            cmd.Parameters.Add(new SqlParameter("@CreditCardNumber", student.CreditCardNumber));
            cmd.Parameters.Add(new SqlParameter("@EducationalForm", student.EducationalForm));
            cmd.Parameters.Add(new SqlParameter("@BasisOfTraining", student.BasisOfTraining));
            
         }
    }
    /*public static class Logger
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
    */
}