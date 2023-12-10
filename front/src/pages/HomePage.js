import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { Link } from "react-router-dom";
import { fetchUserProfile, logout } from "../store/actions/authActions";

const HomePage = () => {
  const dispatch = useDispatch();
  const user = useSelector((state) => state.auth.user);
  const isAuthenticated = useSelector((state) => state.auth.isAuthenticated);

  useEffect(() => {
    if (isAuthenticated && !user) {
      dispatch(fetchUserProfile());
    }
  }, [isAuthenticated, user, dispatch]);

  return (
    <div>
      <h1>Добро пожаловать в наше приложение</h1>
      {isAuthenticated ? (
        <div>
          <p>Привет, {user?.username}!</p>
          <Link to="/events">
            <button>События</button>
          </Link>
          <button onClick={() => dispatch(logout())}>Выход</button>
        </div>
      ) : (
        <div>
          <Link to="/login">
            <button>Вход в аккаунт</button>
          </Link>
          <Link to="/register">
            <button>Регистрация</button>
          </Link>
        </div>
      )}
    </div>
  );
};

export default HomePage;
