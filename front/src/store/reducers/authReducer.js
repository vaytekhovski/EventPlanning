const initialState = {
  isAuthenticated: false,
  user: null,
  error: null,
};

const authReducer = (state = initialState, action) => {
  switch (action.type) {
    case "AUTHENTICATE_USER":
      return {
        ...state,
        isAuthenticated: true,
      };
    case "SET_USER_DATA":
      return {
        ...state,
        user: { ...state.user, ...action.payload },
      };
    case "LOGIN_SUCCESS":
      return {
        ...state,
        isAuthenticated: true,
        user: action.payload.user,
        error: null,
      };
    case "LOGIN_FAILURE":
      return {
        ...state,
        error: action.payload,
      };
    case "LOGOUT":
      return {
        ...initialState,
      };
    default:
      return state;
  }
};

export default authReducer;
