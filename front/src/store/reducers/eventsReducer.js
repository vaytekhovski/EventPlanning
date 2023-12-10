const initialState = {
  eventsList: [],
  currentEvent: null,
  isLoading: false,
  error: null,
};

const eventsReducer = (state = initialState, action) => {
  switch (action.type) {
    case "FETCH_EVENTS_REQUEST":
      return {
        ...state,
        isLoading: true,
      };
    case "FETCH_EVENTS_SUCCESS":
      return {
        ...state,
        eventsList: action.payload,
        isLoading: false,
      };
    case "FETCH_EVENTS_FAILURE":
      return {
        ...state,
        error: action.payload,
        isLoading: false,
      };
    case "FETCH_EVENT_REQUEST":
      return { ...state, isLoading: true };
    case "FETCH_EVENT_SUCCESS":
      return { ...state, currentEvent: action.payload, isLoading: false };
    case "FETCH_EVENT_FAILURE":
      return { ...state, error: action.payload, isLoading: false };
    case "DELETE_EVENT_REQUEST":
      return { ...state };
    case "DELETE_EVENT_SUCCESS":
      return {
        ...state,
        eventsList: state.eventsList.filter(
          (event) => event.id !== action.payload
        ),
      };
    case "DELETE_EVENT_FAILURE":
      return { ...state, error: action.payload };
    case "REQUEST_PARTICIPATION_REQUEST":
      return { ...state, isLoading: true };
    // case "REQUEST_PARTICIPATION_SUCCESS":
    //   return { ...state, isLoading: false, eventsList: action.payload };
    case "REQUEST_PARTICIPATION_FAILURE":
      return { ...state, isLoading: false, error: action.payload };
    default:
      return state;
  }
};
export default eventsReducer;
