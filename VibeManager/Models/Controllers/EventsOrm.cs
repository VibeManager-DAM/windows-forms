using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeManager.Data;

namespace VibeManager.Models.Controllers
{
    public class EventsOrm
    {
        public static int getTotalEvents()
        {
            int totalEvents = 0;

            try
            {
                totalEvents = Orm.db.EVENTS
                    .Count();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


            return totalEvents;
        }

        public static List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();

            try
            {
                events = (from e in Orm.db.EVENTS
                          join r in Orm.db.RESERVES on e.id equals r.id_event
                          join s in Orm.db.SPACES on r.id_space equals s.id
                          where e.id_organizer == App.CurrentUser.Id
                          select new Event
                          {
                              Id = e.id,
                              Title = e.title,
                              Description = e.description,
                              Date = e.date,
                              Time = e.time,
                              Capacity = e.capacity,
                              Seats = e.seats,
                              NumRows = e.num_rows ?? 0,
                              NumColumns = e.num_columns ?? 0,
                              SpaceName = s.name
                          }).ToList();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar eventos: " + ex.Message);
            }

            return events;
        }


        public static bool DeleteEventById(int eventId)
        {
            try
            {
                // 1. Eliminar MENSAJES de los CHATS asociados al evento
                var chats = Orm.db.CHAT.Where(c => c.id_event == eventId).ToList();
                var chatIds = chats.Select(c => c.id).ToList();

                var mensajes = Orm.db.MESSAGES.Where(m => chatIds.Contains(m.id_chat)).ToList();
                if (mensajes.Any())
                {
                    Orm.db.MESSAGES.RemoveRange(mensajes);
                }

                // 2. Eliminar los CHATS del evento
                if (chats.Any())
                {
                    Orm.db.CHAT.RemoveRange(chats);
                }

                // 3. Eliminar TICKETS relacionados
                var tickets = Orm.db.TICKETS.Where(t => t.id_event == eventId).ToList();
                if (tickets.Any())
                {
                    Orm.db.TICKETS.RemoveRange(tickets);
                }

                // 4. Eliminar RESERVES relacionados
                var reserves = Orm.db.RESERVES.Where(r => r.id_event == eventId).ToList();
                if (reserves.Any())
                {
                    Orm.db.RESERVES.RemoveRange(reserves);
                }

                // 5. Finalmente, eliminar el EVENTO
                var evento = Orm.db.EVENTS.FirstOrDefault(e => e.id == eventId);
                if (evento != null)
                {
                    Orm.db.EVENTS.Remove(evento);
                }

                Orm.db.SaveChanges();
                return true;
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar evento y sus relaciones: " + ex.Message);
                return false;
            }
        }


        public static bool CreateOrUpdateEvent(Event evt, string spaceName)
        {
            try
            {
                // Obtener el espacio relacionado
                var space = Orm.db.SPACES.FirstOrDefault(s => s.name == spaceName);
                if (space == null)
                {
                    Console.WriteLine("Espacio no encontrado.");
                    return false;
                }

                if (evt.Id == 0)
                {
                    // CREAR nuevo evento
                    var newEvent = new EVENTS
                    {
                        title = evt.Title,
                        description = evt.Description,
                        date = evt.Date,
                        time = evt.Time,
                        capacity = evt.Capacity,
                        seats = evt.Seats,
                        num_rows = evt.NumRows,
                        num_columns = evt.NumColumns,
                        id_organizer = App.CurrentUser.Id, 
                        price = 0
                    };

                    Orm.db.EVENTS.Add(newEvent);
                    Orm.db.SaveChanges();

                    var newReserve = new RESERVES
                    {
                        id_event = newEvent.id,
                        id_space = space.id
                    };
                    Orm.db.RESERVES.Add(newReserve);
                }
                else
                {
                    // EDITAR evento existente
                    var existingEvent = Orm.db.EVENTS.FirstOrDefault(e => e.id == evt.Id);
                    if (existingEvent != null)
                    {
                        existingEvent.title = evt.Title;
                        existingEvent.description = evt.Description;
                        existingEvent.date = evt.Date;
                        existingEvent.time = evt.Time;
                        existingEvent.capacity = evt.Capacity;
                        existingEvent.seats = evt.Seats;
                        existingEvent.num_rows = evt.NumRows;
                        existingEvent.num_columns = evt.NumColumns;

                        // Actualizar reserva
                        var reserve = Orm.db.RESERVES.FirstOrDefault(r => r.id_event == existingEvent.id);
                        if (reserve != null)
                        {
                            reserve.id_space = space.id;
                        }
                    }
                }

                Orm.db.SaveChanges();
                return true;
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear o editar evento: " + ex.Message);
                return false;
            }
        }


    }
}
