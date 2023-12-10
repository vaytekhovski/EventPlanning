import { combineReducers } from "redux";
import authReducer from "./authReducer";
import eventsReducer from "./eventsReducer";

const rootReducer = combineReducers({
  auth: authReducer,
  events: eventsReducer,
});

export default rootReducer;
