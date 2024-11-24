import axios from "axios";
import { jwtDecode } from 'jwt-decode';



interface MyTokenType {
  sub: string;
  name: string;
  iat: number;
  exp: number;
}
const axiosInstance = axios.create({
  baseURL: "http://localhost:5009/api", 
});


const isTokenExpired = (token : string) => {
  try {

      const decodedToken = jwtDecode<MyTokenType>(token);
      const currentTime = Math.floor(Date.now() / 1000);
      return decodedToken.exp < currentTime;
  } catch (error) {
      console.error("Invalid token:", error);
      return true;
  }
};


axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("authToken"); 
    if (token) {
      if(isTokenExpired(token)){
        alert('Session expired. Please log in again.');
        localStorage.removeItem("authToken");
        window.location.href = "/login";
        return Promise.reject(new Error('Token expired'));
      }
      config.headers.Authorization = `Bearer ${token}`;
      config.headers["Content-Type"] = "application/json";
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;
