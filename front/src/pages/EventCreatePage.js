import React, { useState } from "react";
import axiosInstance from "../utils/axiosInstance";
import { useNavigate } from "react-router-dom";

const EventCreatePage = () => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [date, setDate] = useState("");
  const [location, setLocation] = useState("");
  const [maxParticipants, setMaxParticipants] = useState(0);
  const [requiresPhoneConfirmation, setRequiresPhoneConfirmation] =
    useState(false);
  const [dynamicFields, setDynamicFields] = useState([{ key: "", value: "" }]);
  const navigate = useNavigate();
  const [error, setError] = useState("");

  const handleAddField = () => {
    setDynamicFields([...dynamicFields, { key: "", value: "" }]);
  };

  const handleRemoveField = (index) => {
    const newFields = dynamicFields.filter((_, idx) => idx !== index);
    setDynamicFields(newFields);
  };

  const updateField = (index, key, value) => {
    const newFields = [...dynamicFields];
    newFields[index] = { ...newFields[index], [key]: value };
    setDynamicFields(newFields);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");
    const eventData = {
      title,
      description,
      date,
      location,
      maxParticipants,
      requiresPhoneConfirmation,
      dynamicFields: dynamicFields.reduce((obj, field) => {
        if (field.key && field.value) {
          obj[field.key] = field.value;
        }
        return obj;
      }, {}),
    };
    try {
      const response = await axiosInstance.post("/events", eventData);
      navigate(`/events/${response.data.id}`);
    } catch (error) {
      console.log("ERROR", error.response.data);

      if (error.response.data.errors) {
        setError(error.response.data.title);
      } else if (
        error.response &&
        error.response.data?.includes("Дата события должна быть в будущем.")
      ) {
        setError("Дата события должна быть в будущем.");
      } else {
      }
    }
  };

  const isAddButtonDisabled = dynamicFields.some(
    (field) => !field.key || !field.value
  );

  return (
    <>
      {error && <p className="error">{error}</p>}
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <textarea
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <input
          type="datetime-local"
          value={date}
          onChange={(e) => setDate(e.target.value)}
        />
        <input
          type="text"
          placeholder="Location"
          value={location}
          onChange={(e) => setLocation(e.target.value)}
        />
        <input
          type="number"
          placeholder="Max Participants"
          value={maxParticipants}
          onChange={(e) => setMaxParticipants(e.target.value)}
        />
        <label>
          Requires Phone Confirmation:
          <input
            type="checkbox"
            checked={requiresPhoneConfirmation}
            onChange={() =>
              setRequiresPhoneConfirmation(!requiresPhoneConfirmation)
            }
          />
        </label>{" "}
        {dynamicFields.map((field, index) => (
          <div key={index}>
            <input
              type="text"
              placeholder="Key"
              value={field.key}
              onChange={(e) => updateField(index, "key", e.target.value)}
            />
            <input
              type="text"
              placeholder="Value"
              value={field.value}
              onChange={(e) => updateField(index, "value", e.target.value)}
            />
            <button type="button" onClick={() => handleRemoveField(index)}>
              Удалить
            </button>
          </div>
        ))}
        <button
          type="button"
          onClick={handleAddField}
          disabled={isAddButtonDisabled}
        >
          Добавить поле
        </button>
        <button type="submit">Создать событие</button>
      </form>
    </>
  );
};

export default EventCreatePage;
