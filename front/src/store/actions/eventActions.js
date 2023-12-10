import axiosInstance from "../../utils/axiosInstance";

import { toast } from "react-toastify";

export const fetchEvents = () => async (dispatch) => {
  dispatch({ type: "FETCH_EVENTS_REQUEST" });

  try {
    const response = await axiosInstance.get("/events");
    dispatch({
      type: "FETCH_EVENTS_SUCCESS",
      payload: response.data,
    });
  } catch (error) {
    dispatch({
      type: "FETCH_EVENTS_FAILURE",
      payload: error.message,
    });
  }
};

export const fetchEventById = (eventId) => async (dispatch) => {
  dispatch({ type: "FETCH_EVENT_REQUEST" });

  try {
    const response = await axiosInstance.get(`/events/${eventId}`);
    dispatch({
      type: "FETCH_EVENT_SUCCESS",
      payload: response.data,
    });
  } catch (error) {
    dispatch({
      type: "FETCH_EVENT_FAILURE",
      payload: error.message,
    });
  }
};

export const deleteEvent = (eventId, navigate) => async (dispatch) => {
  dispatch({ type: "DELETE_EVENT_REQUEST" });

  try {
    await axiosInstance.delete(`/events/${eventId}`);
    dispatch({ type: "DELETE_EVENT_SUCCESS", payload: eventId });

    toast.success("Событие успешно удалено");
  } catch (error) {
    dispatch({ type: "DELETE_EVENT_FAILURE", payload: error.message });
    toast.error("Ошибка при удалении события");
  }
};

export const requestParticipation = (eventId) => async (dispatch) => {
  dispatch({ type: "REQUEST_PARTICIPATION_REQUEST" });

  try {
    const response = await axiosInstance.post(
      `/events/${eventId}/requestParticipation`
    );
    dispatch({ type: "REQUEST_PARTICIPATION_SUCCESS", payload: response.data });
  } catch (error) {
    dispatch({ type: "REQUEST_PARTICIPATION_FAILURE", payload: error.message });
  }
};

export const cancelParticipation = (eventId) => async (dispatch) => {
  dispatch({ type: "CANCEL_PARTICIPATION_REQUEST" });

  try {
    const response = await axiosInstance.post(
      `/events/${eventId}/cancelParticipation`
    );
    dispatch({ type: "CANCEL_PARTICIPATION_SUCCESS", payload: response.data });
  } catch (error) {
    dispatch({ type: "CANCELPARTICIPATION_FAILURE", payload: error.message });
  }
};
