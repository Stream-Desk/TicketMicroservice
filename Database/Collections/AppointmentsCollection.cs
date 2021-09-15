using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Appointments;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class AppointmentsCollection : IAppointmentCollection
    {
        private IMongoCollection<Appointment> _appointmentCollection;
        public AppointmentsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var appointmentsCollectionName = configuration.GetValue<string>("MongoDb:AppointmentsCollection");

            _appointmentCollection = database.GetCollection<Appointment>(appointmentsCollectionName);

        }
        public async Task<List<Appointment>> GetAppointments(CancellationToken cancellationToken = default)
        {
            var cursor = await _appointmentCollection.FindAsync(a => true);
            var appointment = await cursor.ToListAsync(cancellationToken);
            return appointment;
        }

        public async Task<Appointment> GetAppointmentById(string appointmentId, CancellationToken cancellationtoken = default)
        {
            var cursor = await _appointmentCollection.FindAsync(a => a.AppointmentId == appointmentId);
            var appointment = await cursor.FirstOrDefaultAsync(cancellationtoken);
            return appointment;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment, CancellationToken cancellationtoken = default)
        {
            await _appointmentCollection.InsertOneAsync(appointment);
            return appointment;
        }

        public void CancelAppointment(string appointmentId)
        {
            _appointmentCollection.DeleteOne(a => a.AppointmentId == appointmentId);
        }
    }
}