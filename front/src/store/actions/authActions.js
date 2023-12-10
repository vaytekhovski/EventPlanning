import axiosInstance from "../../utils/axiosInstance";

export const login = (credentials) => async (dispatch) => {
  try {
    const response = await axiosInstance.post("/Auth/login", credentials);
    const data = response.data;
    dispatch({ type: "LOGIN_SUCCESS", payload: data });
    localStorage.setItem("token", data.token);
  } catch (error) {
    dispatch({ type: "LOGIN_FAILURE", payload: error.message });
  }
};

export const logout = () => {
  localStorage.removeItem("token");
  return { type: "LOGOUT" };
};

export const checkAuthState = () => {
  return (dispatch) => {
    const token = localStorage.getItem("token");
    if (token) {
      dispatch({ type: "AUTHENTICATE_USER" });
      dispatch(fetchUserData());
    } else {
      dispatch(logout());
    }
  };
};

export const fetchUserProfile = () => async (dispatch, getState) => {
  const token = localStorage.getItem("token");
  if (token) {
    axiosInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    try {
      const response = await axiosInstance.get("/users/profile");
      dispatch({ type: "SET_USER_DATA", payload: response.data });
    } catch (error) {
      console.error("Ошибка при получении данных пользователя", error);
    }
  }
};

export const fetchUserData = () => async (dispatch, getState) => {
  try {
    const userId = getState().auth.user.id;
    const response = await axiosInstance.get(`/users/${userId}`);
    dispatch({ type: "SET_USER_DATA", payload: response.data });
  } catch (error) {
    console.error("Ошибка при получении данных пользователя", error);
  }
};
