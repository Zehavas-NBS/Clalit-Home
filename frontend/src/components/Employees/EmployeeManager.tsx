import React, { useEffect, useState } from "react";
import EmployeeList from "./EmployeeList";
import EmployeeForm from "./EmployeeForm";
import { Employee } from "../../types";
import axios from "axios"; 
import { useAuth } from "../../AuthContext";
import "./EmployeeManager.css";


const EmployeeManager =() => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [filteredEmployees, setFilteredEmployees] = useState<Employee[]>([]);

  const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>(null);
  const [selectedEmployee1, setSelectedEmployee1] = useState<Employee | null>(null);

  const { currentManager, logout } = useAuth();
  const [searchTerm, setSearchTerm] = useState("");


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

  useEffect(() => {
    // Filter employees based on the search term
  const filteredData = employees.filter((employee) =>
    employee.email?.toLowerCase().includes(searchTerm.toLowerCase())
  );
  setFilteredEmployees(filteredData);
  }, [employees]);

  useEffect(() => {
    if(searchTerm === "") 
      { 
        setFilteredEmployees(employees);
        return;
       }
    // Filter employees based on the search term
  const filteredData = employees.filter((employee) =>
    employee.email?.toLowerCase().includes(searchTerm.toLowerCase())
  );
  setFilteredEmployees(filteredData);
  }, [searchTerm]);

  const handleSave = async (employee: Employee, isNew : boolean) => {
    if (employee.id) {
      try {
console.log(employee);
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
      setSelectedEmployee(null);
    } catch (err) {
      //setError("Invalid email or password.");
    }
   
  };

  const handleCancel = () => {
    setSelectedEmployee(null);
  };


  

  return (
    <div className="employee-manager">
      <h1 className="header">Employee Manager</h1>
      {selectedEmployee ? (
          <div className="employee-form-container">
        <EmployeeForm
          employee={selectedEmployee}
          onSave={handleSave}
          onCancel={handleCancel}
          readOnly={false}
        />
        </div>
      ) : (
        <>
          <div className="employee-list-container">
              {/* Search Input */}
          <div className="search-container">
            <input
              type="text"
              placeholder="Search by name..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
              style={{ width: "30%" }}
            />
          </div>
          <EmployeeList
            employees={filteredEmployees}
            onSelect={setSelectedEmployee}
            onDelete={handleDelete}
          />
         
          <button  className="add-employee-button" onClick={() => setSelectedEmployee({ id: "", fullName: "", email: "", password:"" })}>
            Add Employee
          </button>
          </div>
        </>
      )}
    </div>
  );
};

export default EmployeeManager;
