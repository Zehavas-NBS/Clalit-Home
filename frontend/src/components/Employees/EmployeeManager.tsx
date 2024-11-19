import React, { useEffect, useState } from "react";
import EmployeeList from "./EmployeeList";
import EmployeeForm from "./EmployeeForm";
import { Employee } from "../../types";
import { useAuth } from "../../AuthContext";
import "./EmployeeManager.css";
import axiosInstance from "../../Infra/axiosInstance";
import Autocomplete from "@mui/material/Autocomplete";
import TextField from "@mui/material/TextField";



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
          const response = await axiosInstance.post("/Employees/getEmployeesByManagerId");

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
      // Update existing employee
   
    if (employee.id) {
      try {
         await axiosInstance.put(`/Employees/update`,
            {
              id: employee.id,
              fullName: employee.fullName,
              email: employee.email,
              password: employee.password,
            }
        );
          setEmployees((prev) =>
            prev.map((e) => (e.id === employee.id ? employee : e))
          );
      }
       catch (err) {
        //setError("Invalid email or password.");
      }
     
      
    } else {
      // Add new employee

      try {

        const response = await axiosInstance.post(
            `http://localhost:5009/api/Employees/add`,{
              id: employee.id,
              fullName: employee.fullName,
              email: employee.email,
              password: employee.password,
            }
        );
          const { employeeData} = response.data.response;
          console.log(employeeData);
          setEmployees((prev) => [
            ...prev,
            { ...employee, id: employeeData.id },
          ]);
      } catch (err) {
        //setError("Invalid email or password.");
      }
    }
    setSelectedEmployee(null);
  };

  const handleDelete = async (id: string) => {
    
    try {
      await axiosInstance.delete(`/Employees/delete/${id}`);
      setEmployees((prev) => prev.filter((e) => e.id !== id));
      setSelectedEmployee(null);
    } catch (err) {
      //setError("Invalid email or password.");
    }
   
  };

  const handleCancel = () => {
    setSelectedEmployee(null);
  };

  const handleSelect = (event: any, value: Employee | null) => {
    if (value) {
      const filteredData = employees.filter((employee) => employee.id === value.id);
      setFilteredEmployees(filteredData);
      console.log('Selected employee:', value);
    }
    else
    {
      setFilteredEmployees(employees);
    }
  };

  return (
    <div className="employee-manager">
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
            
              <div className="header">
    
            <Autocomplete
                options={employees}
                getOptionLabel={(option) => option.fullName || ""} 
                onChange={handleSelect}
                style={{ width: "30%" }}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Search by name..."
                    variant="outlined"
                    // onChange={(e) => setSearchTerm(e.target.value)}
                    InputProps={{
                      ...params.InputProps,
                    
                    }}
                  />
                )}/>
                    <img
          src="/images/daily-briefing.jpg" // נתיב לתמונה
          alt="Logo"
          style={{ height: '100px', width: '150px' }}
        />
          </div>
          <EmployeeList
            employees={filteredEmployees}
            onSelect={setSelectedEmployee}
            onDelete={handleDelete}
          />
         {/* <div className="header">
        <img
          src="/images/daily-briefing.jpg" // נתיב לתמונה
          alt="Logo"
          style={{ height: '120px', width: '100px' }}
        /> */}
      
          <button  className="add-employee-button" onClick={() => setSelectedEmployee({ id: "", fullName: "", email: "", password:"" })}>
            Add Employee
          </button>
          {/* </div> */}
          </div>
        </>
      )}
    </div>
  );
};

export default EmployeeManager;
