import React, { useState } from "react";
import { Employee } from "../../types";
import "./EmployeeList.css";

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
    <div className="employee-list">
      <h2>Employee List</h2>
      <ul >
        {employees.map((employee) => (
          <li  key={employee.id}   className={`employee-item ${
            employee.id === selectedEmployeeId ? "selected" : ""
          }`}
        >
            <span>
              {employee.name} ({employee.email})
            </span>
            <div className="actions">
            <button onClick={() => select(employee)}>Edit</button>
            <button onClick={() => onDeleted(employee.id)}>Delete</button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default EmployeeList;
