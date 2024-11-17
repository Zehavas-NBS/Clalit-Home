import React, { useState } from "react";
import { Employee } from "../../types";

interface EmployeeListProps {
  employees: Employee[];
  onSelect: (employee: Employee) => void;
  onDelete: (id: string) => void;
}

const EmployeeList = ({ employees, onSelect, onDelete } : EmployeeListProps) => {
  const [selectedEmployeeId, setSelectedEmployeeId] = useState("");

  const select = (employee: Employee) =>
  {
    setSelectedEmployeeId(employee.id);
    onSelect(employee);
  }
  const onDeleted = (employeeId : string) =>
    {
      setSelectedEmployeeId(employeeId);
      onDelete(employeeId);
    }

  return (
    <div>
      <h2>Employee List</h2>
      <ul >
        {employees.map((employee) => (
          <li key={employee.id}
          style={{
            padding: "10px",
            backgroundColor:
              employee.id === selectedEmployeeId ? "#f0f8ff" : "transparent",
            border: employee.id === selectedEmployeeId ? "1px solid #000" : "",
          }}>
            <span>
              {employee.name} ({employee.email})
            </span>
            <button onClick={() => select(employee)}>Edit</button>
            <button onClick={() => onDeleted(employee.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default EmployeeList;
