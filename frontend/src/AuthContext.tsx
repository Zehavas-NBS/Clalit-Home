import React, { createContext, useContext, useState, ReactNode } from "react";
import { Employee } from "./types";

// סוג הנתונים שהקונטקסט מספק
interface AuthContextProps {
  currentManager: Employee | null;
  login: (employee: Employee) => void;
  logout: () => void;
}

// ברירת מחדל לקונטקסט
const AuthContext = createContext<AuthContextProps>({
  currentManager: null,
  login: () => {},
  logout: () => {},
});

// ספק (Provider) שמנהל את הקונטקסט
export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [currentManager, setCurrentManager] = useState<Employee | null>(null);

  const login = (employee: Employee) => {
    setCurrentManager(employee);
  };

  const logout = () => {
    setCurrentManager(null);
  };

  return (
    <AuthContext.Provider value={{ currentManager, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// הוק לשימוש בקונטקסט בצורה נוחה
export const useAuth = () => useContext(AuthContext);
