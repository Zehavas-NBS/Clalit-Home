import React, { createContext, useContext, useState, ReactNode } from "react";
import { Employee } from "./types";

// סוג הנתונים שהקונטקסט מספק
interface AuthContextProps {
  currentManager: Employee | null;
  login: (employee: Employee) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextProps>({
  currentManager: null,
  login: () => { },
  logout: () => { },
});

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

export const useAuth = () => useContext(AuthContext);
