import React, { useState } from "react";
import "./PasswordInput.css";

interface PasswordInputProps {
  value: string;
  onChange: (value: string, error: string) => void;
  label?: string;
}

const PasswordInput: React.FC<PasswordInputProps> = ({
  value,
  onChange,
  label = "Password",
}) => {
  const [password, setPassword] = useState<string>(value || "");
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [error, setError] = useState<string>("");

  // Validation rules
  const validatePassword = (pwd: string): string => {
    if (!pwd) {
      return "Password cannot be empty.";
    }
    if (pwd.length < 6) {
      return "Password must be at least 6 characters long.";
    }
    if (!/[A-Z]/.test(pwd)) {
      return "Password must contain at least one uppercase letter.";
    }
    if (!/[a-z]/.test(pwd)) {
      return "Password must contain at least one lowercase letter.";
    }
    if (!/[0-9]/.test(pwd)) {
      return "Password must contain at least one number.";
    }
    if (!/[!@#$%^&*]/.test(pwd)) {
      return "Password must contain at least one special character (!@#$%^&*).";
    }
    return ""; // No errors
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newPwd = e.target.value;
    setPassword(newPwd);

    const validationError = validatePassword(newPwd);
    setError(validationError);

    onChange(newPwd, validationError);
  };

  const toggleShowPassword = () => {
    setShowPassword((prev) => !prev);
  };

  return (
    <div className="password-input-container">
      <label className="password-input-label">{label}</label>
      <div className="password-input-wrapper">
        <input
          type={showPassword ? "text" : "password"}
          value={password}
          onChange={handleChange}
          className={`password-input-field ${error ? "password-input-error" : ""}`}
          placeholder="Enter your password"
        />
        <button
          type="button"
          onClick={toggleShowPassword}
          className="password-toggle-button"
        >
          {showPassword ? "Hide" : "Show"}
        </button>
      </div>
      {error && <p className="password-error-message">{error}</p>}
    </div>
  );
};

export default PasswordInput;
