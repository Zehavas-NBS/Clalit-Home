import React, { useEffect, useState } from "react";
import EmployeeList from "./EmployeeList";
import EmployeeForm from "./EmployeeForm";
import { Employee } from "../../types";
import axios from "axios"; 
import { useAuth } from "../../AuthContext";

const EmployeeManager =() => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const { currentManager, logout } = useAuth();

  useEffect(() => {
    const fetchEmployees = async () => {
      if (currentManager) {
        try {

          const response = await axios.post(
            "http://localhost:5009/api/Employees/getEmployeesByManagerId",
            {},
            {
              headers: {
                Authorization: `Bearer ${localStorage.getItem("authToken")}`,
                "Content-Type": "application/json", // שים לב לשימוש הנכון
              },
            }
          );
          if (response.data) {
            setEmployees(response.data);
          } 
        } catch (err) {
          //setError("Invalid email or password.");
        }
      }
    };
    fetchEmployees();
  }, [currentManager]);

  const handleSave = async (employee: Employee, isNew : boolean) => {
    if (employee.id) {
      try {

        const response = await axios.put(
            `http://localhost:5009/api/Employees/update/${employee.id}`,
            {
             employee
            },
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("authToken")}`,
              "Content-Type": "application/json", // שים לב לשימוש הנכון
            },
          }
        );
        if (response.status.toString() === "200") {
          setEmployees((prev) =>
            prev.map((e) => (e.id === employee.id ? employee : e))
          );
        } 
      } catch (err) {
        //setError("Invalid email or password.");
      }
     
      // Update existing employee
      
    } else {
      // Add new employee

      try {

        const response = await axios.post(
            `http://localhost:5009/api/Employees/add`,employee,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("authToken")}`,
              "Content-Type": "application/json", // שים לב לשימוש הנכון
            },
          }
        );
        if (response.status.toString() === "200") {
          const { EmployeeData} = response.data;
          setEmployees((prev) => [
            ...prev,
            { ...EmployeeData },
          ]);
        } 
      } catch (err) {
        //setError("Invalid email or password.");
      }
      
     
    }
    setSelectedEmployee(null);
  };

  const handleDelete = async (id: string) => {
    
    try {

      const response = await axios.delete(
          `http://localhost:5009/api/Employees/delete/${id}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("authToken")}`,
            "Content-Type": "application/json", // שים לב לשימוש הנכון
          },
        }
      );
      if (response.status.toString() === "200") {
        setEmployees((prev) => prev.filter((e) => e.id !== id));
      } 
    } catch (err) {
      //setError("Invalid email or password.");
    }
   
  };

  const handleCancel = () => {
    setSelectedEmployee(null);
  };

  return (
    <div>
      {selectedEmployee ? (
        <EmployeeForm
          employee={selectedEmployee}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      ) : (
        <>
          <EmployeeList
            employees={employees}
            onSelect={setSelectedEmployee}
            onDelete={handleDelete}
          />
          <button onClick={() => setSelectedEmployee({ id: "", name: "", email: "" })}>
            Add Employee
          </button>
        </>
      )}
    </div>
  );
};

export default EmployeeManager;
