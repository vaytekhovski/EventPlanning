import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axiosInstance from "../utils/axiosInstance";

const ConfirmParticipationPage = () => {
  const { eventId } = useParams();
  const [confirmationCode, setConfirmationCode] = useState("");
  const navigate = useNavigate();
  const [error, setError] = useState("");

  const handleSubmit = async (event) => {
    event.preventDefault();
    await axiosInstance
      .post(`/events/${eventId}/confirmParticipation/${confirmationCode}`)
      .then((data) => {
        if (data && data.includes("Участие успешно подтверждено.")) {
          navigate(`/events/${eventId}`);
        }
      })
      .catch((error) => {
        setError(error.response.data);
      });
  };

  return (
    <div>
      {error && <p className="error">{error}</p>}

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Код подтверждения"
          value={confirmationCode}
          onChange={(e) => setConfirmationCode(e.target.value)}
        />
        <button type="submit">Подтвердить участие</button>
      </form>
    </div>
  );
};

export default ConfirmParticipationPage;
