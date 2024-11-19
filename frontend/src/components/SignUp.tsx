import React, { useState } from 'react';
import axios from 'axios';
import PasswordInput from './Common/PasswordInput';
import './SignUp.css';  // הוספת קובץ ה-CSS

//import { SignupRequest } from '../types/api';

const SignUp = () => {
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
  });
  const [message, setMessage] = useState<string>('');

  const [password, setPassword] = useState<string>("");
  const [passwordError, setPasswordError] = useState<string>("");

  const handlePasswordChange = (newPassword: string, validationError: string) => {
    setFormData({ ...formData, password: newPassword });
    setPasswordError(validationError);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      let result;
       await axios.post('http://localhost:5009/api/Auth/signup', formData).
      then((res) => result = res.data);
      setMessage('Signup successful! You can now log in.');
      window.location.href = '/login';
    } 
    catch (error: unknown) {
      console.error(error);
      setMessage('Signup failed. Please try again.');
    }
  };

  return (
    <form className='sign-up-form' onSubmit={handleSubmit}>
      <h2>Sign Up</h2>
      <label>
        Full Name:
        <input
          type="text"
          name="fullName"
          value={formData.fullName}
          onChange={handleChange}
          required
        />
      </label>
      <label>
        Email:
        <input
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          required
        />
      </label>
      <PasswordInput
        value={formData.password}
        onChange={handlePasswordChange}
        label="Password"
      />
      <button type="submit">Sign Up</button>
      {message && <p>{message}</p>}
    </form>
  );
};

export default SignUp;
