import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "http://localhost:5009/api", // ה-URL הבסיסי לשרת שלך
});

// הוספת Interceptor לבקשות
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("authToken"); // קבלת ה-JWT מ-localStorage
    if (token) {
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
