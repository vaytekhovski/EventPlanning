import React, { useEffect } from "react";
import { useDispatch } from "react-redux";
import { Routes, Route } from "react-router-dom";
import {
  HomePage,
  LoginPage,
  RegisterPage,
  EventsPage,
  EventCreatePage,
  EventDetailsPage,
  ConfirmParticipationPage,
} from "./pages";
import { checkAuthState } from "./store/actions/authActions";
import ProtectedRoute from "./utils/ProtectedRoute";

function App() {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(checkAuthState());
  }, [dispatch]);

  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route
        path="/events"
        element={
          <ProtectedRoute>
            <EventsPage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/create-event"
        element={
          <ProtectedRoute>
            <EventCreatePage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/events/:eventId"
        element={
          <ProtectedRoute>
            <EventDetailsPage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/events/confirm-participation/:eventId"
        element={
          <ProtectedRoute>
            <ConfirmParticipationPage />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
}

export default App;
