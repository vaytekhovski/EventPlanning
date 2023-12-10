import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { fetchEvents } from "../store/actions/eventActions";

const EventsPage = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { events, isAdmin, isLoading } = useSelector((state) => ({
    events: state.events.eventsList,
    isAdmin: state.auth.user.isAdmin,
    isLoading: state.events.isLoading,
  }));

  useEffect(() => {
    dispatch(fetchEvents());
  }, [dispatch]);

  const handleCreateEvent = () => {
    navigate("/create-event");
  };

  if (isLoading) {
    return <div>Загрузка...</div>;
  }

  return (
    <div>
      <h1>События</h1>
      {isAdmin && <button onClick={handleCreateEvent}>Создать событие</button>}
      {events.map((event) => (
        <div key={event.id}>
          <h3>{event.title}</h3>
          <h3>{event.date}</h3>
          <h3>{event.location}</h3>
          <h3>
            {event.participantIds.length} / {event.maxParticipants}
          </h3>
          <button onClick={() => navigate(`/events/${event.id}`)}>
            Просмотр
          </button>
        </div>
      ))}
    </div>
  );
};

export default EventsPage;
