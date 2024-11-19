import React, { useState } from "react";
import axios from "axios";
import { useAuth } from "../AuthContext";
import './Login.css';


const LogIn = (props: { onLogIn: (token: string) => void }) => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();


  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    try {
      const response = await axios.post("http://localhost:5009/api/Auth/login", formData);
      if (response.data && response.data.token) {
        const { token, managerData } = response.data;
        localStorage.setItem("authToken", token);
        login(managerData);
        props.onLogIn(token);
      } else {
        setError("Invalid response from server.");
      }
    } catch (err) {
      setError("Invalid email or password.");
    }
  };

  return (
    <div className="login-container">
      <h1>Log In</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleInputChange}
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleInputChange}
            required
          />
        </div>

        <div>
          {error &&
            <div className="signUp">
              <p style={{ color: "red" }}>{error}</p>
              <p>New manager? Sign up</p>
              <button type="button" onClick={() => window.location.href = "/signup"}>Sign Up</button>
            </div>
          }
          <button type="submit">Log In</button>
        </div>
        <div>

        </div>
      </form>
    </div>
  );
};

export default LogIn;
