import React, { useState } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import LogIn from "./components/LogIn";
import SignUp from "./components/SignUp";
import EmployeeManager from "./components/Employees/EmployeeManager"
import { AuthProvider } from "./AuthContext";


const App = () => {
  const [authToken, setAuthToken] = useState(localStorage.getItem("authToken"));

  const handleLogIn = (token ) => {
    setAuthToken(token);
  };

  return (
    <AuthProvider>
    <Router>
      <Routes>
        <Route path="/signup" element={<SignUp />} />
        <Route
          path="/login"
          element={!authToken ? <LogIn onLogIn={handleLogIn} /> : <Navigate to="/" />}
        />
        <Route
          path="/"
          element={
            authToken ? (
              <div>
                <EmployeeManager/>
                <button onClick={() => {
                  setAuthToken(null);
                  localStorage.removeItem("authToken");
                }}>
                  Log Out
                </button>
              </div>
            ) : (
              <Navigate to="/login" />
            )
          }
        />
      </Routes>
    </Router>
    </AuthProvider>
  );
};

export default App;
