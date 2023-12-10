import React, { useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import { fetchEventById, deleteEvent } from "../store/actions/eventActions";
import {
  requestParticipation,
  cancelParticipation,
} from "../store/actions/eventActions";

const EventDetailsPage = () => {
  const { eventId } = useParams();
  const dispatch = useDispatch();
  const events = useSelector((state) => state.events.eventsList);
  const currentEvent = useSelector((state) => state.events.currentEvent);
  const event = events.find((e) => e.id === eventId) || currentEvent;
  const navigate = useNavigate();
  const currentUser = useSelector((state) => state.auth.user);
  const isAdmin = useSelector((state) => state.auth.user.isAdmin);

  useEffect(() => {
    if (!event) {
      dispatch(fetchEventById(eventId));
    }
  }, [eventId, event, dispatch]);

  const handleEdit = () => {
    navigate(`/events/edit/${eventId}`);
  };

  const handleDelete = () => {
    if (window.confirm("Вы уверены, что хотите удалить это событие?")) {
      dispatch(deleteEvent(eventId));
      navigate("/events");
    }
  };

  console.log(event.participantIds);
  console.log(currentUser);
  let isParticipant = event.participantIds.includes(currentUser.id);
  const hasRequestedParticipation = null;

  const handleParticipation = () => {
    if (isParticipant) {
      handleCancelParticipation();
    } else {
      handleRequestParticipation();
    }
  };

  const handleCancelParticipation = async () => {
    try {
      dispatch(cancelParticipation(eventId));
    } catch (error) {
      console.error(error);
    }
  };

  const handleRequestParticipation = async () => {
    try {
      dispatch(requestParticipation(eventId));
      navigate(`/events/confirm-participation/${eventId}`);
    } catch (error) {
      console.error("Ошибка при запросе участия", error);
    }
  };

  if (!event) {
    return <div>Загрузка...</div>;
  }

  return (
    <div>
      <div>
        <h1>{event.title}</h1>
        <p>Описание: {event.description}</p>
        <p>Дата: {new Date(event.date).toLocaleDateString()}</p>
        <p>Местоположение: {event.location}</p>
        <p>Максимальное количество участников: {event.maxParticipants}</p>
        <p>
          Требуется подтверждение по телефону:{" "}
          {event.requiresPhoneConfirmation ? "Да" : "Нет"}
        </p>

        <div>
          <h3>Дополнительная информация:</h3>
          {Object.entries(event.dynamicFields).map(([key, value]) => (
            <p key={key}>
              {key}: {value}
            </p>
          ))}
        </div>
      </div>
      {isAdmin && (
        <>
          <button disabled={true} onClick={handleEdit}>
            Редактировать
          </button>
          <button onClick={handleDelete}>Удалить</button>
        </>
      )}

      <button onClick={handleParticipation}>
        {isParticipant
          ? "Отменить участие"
          : hasRequestedParticipation
          ? "Подтвердить участие"
          : "Запросить участие"}
      </button>
    </div>
  );
};

export default EventDetailsPage;
