import React, { useState, useEffect } from "react";
import { Employee } from "../../types";

interface EmployeeFormProps {
  employee?: Employee;
  onSave: (employee: Employee, isNew : boolean) => void;
  onCancel: () => void;
}

const EmployeeForm=  ({ employee, onSave, onCancel }: EmployeeFormProps) => {
  const [name, setName] = useState<string>(employee?.name || "");
  const [email, setEmail] = useState<string>(employee?.email || "");
  const [isNew] = useState<boolean>(employee?.id === undefined);

  useEffect(() => {
    if (employee) {
      setName(employee.name ?? "");
      setEmail(employee.email ?? "");
    }
  }, [employee]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave({ ...employee, name, email, id: employee?.id || "" }, isNew);
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>{employee ? "Edit Employee" : "Add New Employee"}</h2>
      <label>
        Name:
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </label>
      <label>
        Email:
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </label>
      <button type="submit">Save</button>
      <button type="button" onClick={onCancel}>
        Cancel
      </button>
    </form>
  );
};

export default EmployeeForm;
