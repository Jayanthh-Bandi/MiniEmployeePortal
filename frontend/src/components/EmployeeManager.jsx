import { useEffect, useState } from "react";
import { apiFetch } from "../services/api";

function EmployeeManager({ isAdmin }) {
    const [employees, setEmployees] = useState([]);
    const [departments, setDepartments] = useState([]);
    const [selectedId, setSelectedId] = useState("");
    const [editingId, setEditingId] = useState(null);

    const [form, setForm] = useState({
        name: "",
        email: "",
        salary: "",
        departmentId: ""
    });

    const [message, setMessage] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(true);

    // Load employees and departments
    const loadData = async () => {
        setLoading(true);
        setError("");

        try {
            const employeeData = await apiFetch("/Employees");
            const departmentData = await apiFetch("/Departments");

            setEmployees(employeeData || []);
            setDepartments(departmentData || []);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadData();
    }, []);

    // Handle form input changes
    const handleChange = (event) => {
        const { name, value } = event.target;

        setForm((previous) => ({
            ...previous,
            [name]: value
        }));
    };

    // Clear employee form
    const clearForm = () => {
        setForm({
            name: "",
            email: "",
            salary: "",
            departmentId: ""
        });

        setEditingId(null);
        setSelectedId("");
    };

    // Create / Update employee
    const saveEmployee = async (event) => {
        event.preventDefault();

        setMessage("");
        setError("");

        // Basic frontend validation
        if (!form.name.trim()) {
            setError("Employee name is required.");
            return;
        }

        if (!form.email.trim()) {
            setError("Employee email is required.");
            return;
        }

        if (!form.salary || Number(form.salary) <= 0) {
            setError("Salary must be greater than 0.");
            return;
        }

        if (!form.departmentId || Number(form.departmentId) <= 0) {
            setError("Please select a department.");
            return;
        }

        try {
            const payload = {
                name: form.name.trim(),
                email: form.email.trim(),
                salary: Number(form.salary),
                departmentId: Number(form.departmentId)
            };

            console.log("Employee payload:", payload);

            if (editingId !== null) {
                await apiFetch(`/Employees/${editingId}`, {
                    method: "PUT",
                    body: JSON.stringify(payload)
                });

                setMessage(
                    "Employee updated successfully."
                );
            } else {
                await apiFetch("/Employees", {
                    method: "POST",
                    body: JSON.stringify(payload)
                });

                setMessage(
                    "Employee created successfully."
                );
            }

            clearForm();
            await loadData();
        } catch (error) {
            setError(error.message);
        }
    };

    // Load employee into form for editing
    const editEmployee = (employee) => {
        setEditingId(employee.employeeId);

        setForm({
            name: employee.name || "",
            email: employee.email || "",
            salary: employee.salary ?? "",
            departmentId: employee.departmentId ?? ""
        });

        setMessage("");
        setError("");

        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    };

    // Delete employee
    const deleteEmployee = async (id) => {
        if (
            !window.confirm(
                "Are you sure you want to delete this employee?"
            )
        ) {
            return;
        }

        setMessage("");
        setError("");

        try {
            await apiFetch(`/Employees/${id}`, {
                method: "DELETE"
            });

            setMessage(
                "Employee deleted successfully."
            );

            await loadData();
        } catch (error) {
            setError(error.message);
        }
    };

    // Find employee by ID
    const getEmployeeById = async () => {
        setMessage("");
        setError("");

        if (!selectedId) {
            setError("Enter an employee ID.");
            return;
        }

        try {
            const employee = await apiFetch(
                `/Employees/${selectedId}`
            );

            setEditingId(employee.employeeId);

            setForm({
                name: employee.name || "",
                email: employee.email || "",
                salary: employee.salary ?? "",
                departmentId: employee.departmentId ?? ""
            });

            setMessage("Employee loaded successfully.");

            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        } catch (error) {
            setError(error.message);
        }
    };

    // Refresh employee data
    const refreshData = async () => {
        setMessage("");
        setError("");

        await loadData();
    };

    if (loading) {
        return (
            <div className="content-card">
                <p>Loading employees...</p>
            </div>
        );
    }

    return (
        <div className="content-card">

            {/* Header */}
            <div className="section-header">
                <div>
                    <h2>Employees</h2>
                    <p>
                        Manage employee records and profiles.
                    </p>
                </div>

                <span className="role-badge employee">
                    {employees.length} Employees
                </span>
            </div>

            {/* Messages */}
            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}

            {message && (
                <div className="success-message">
                    {message}
                </div>
            )}

            {/* Search / Refresh */}
            <div className="toolbar">
                <input
                    type="number"
                    placeholder="Employee ID"
                    value={selectedId}
                    onChange={(event) =>
                        setSelectedId(event.target.value)
                    }
                />

                <button
                    type="button"
                    className="secondary-btn"
                    onClick={getEmployeeById}
                >
                    Find
                </button>

                <button
                    type="button"
                    className="secondary-btn"
                    onClick={refreshData}
                >
                    Refresh
                </button>
            </div>

            {/* Admin Create / Update Form */}
            {isAdmin && (
                <form
                    className="data-form"
                    onSubmit={saveEmployee}
                >
                    <h3>
                        {editingId !== null
                            ? "Update Employee"
                            : "Add Employee"}
                    </h3>

                    <div className="form-grid">

                        {/* Name */}
                        <div>
                            <label>Name</label>

                            <input
                                name="name"
                                type="text"
                                placeholder="Enter employee name"
                                value={form.name}
                                onChange={handleChange}
                                required
                            />
                        </div>

                        {/* Email */}
                        <div>
                            <label>Email</label>

                            <input
                                name="email"
                                type="email"
                                placeholder="Enter employee email"
                                value={form.email}
                                onChange={handleChange}
                                required
                            />
                        </div>

                        {/* Salary */}
                        <div>
                            <label>Salary</label>

                            <input
                                name="salary"
                                type="number"
                                min="1"
                                placeholder="Enter salary"
                                value={form.salary}
                                onChange={handleChange}
                                required
                            />
                        </div>

                        {/* Department */}
                        <div>
                            <label>Department</label>

                            <select
                                name="departmentId"
                                value={form.departmentId}
                                onChange={handleChange}
                                required
                            >
                                <option value="">
                                    Select Department
                                </option>

                                {departments.map(
                                    (department) => (
                                        <option
                                            key={department.id}
                                            value={department.id}
                                        >
                                            {
                                                department.departmentName
                                            }
                                        </option>
                                    )
                                )}
                            </select>
                        </div>
                    </div>

                    {/* Form buttons */}
                    <div className="button-row">
                        <button
                            type="submit"
                            className="primary-btn"
                        >
                            {editingId !== null
                                ? "Update Employee"
                                : "Create Employee"}
                        </button>

                        {editingId !== null && (
                            <button
                                type="button"
                                className="secondary-btn"
                                onClick={clearForm}
                            >
                                Cancel
                            </button>
                        )}
                    </div>
                </form>
            )}

            {/* Employee Table */}
            <div className="table-wrapper">
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Salary</th>
                            <th>Department</th>

                            {isAdmin && (
                                <th>Actions</th>
                            )}
                        </tr>
                    </thead>

                    <tbody>
                        {employees.length === 0 ? (
                            <tr>
                                <td
                                    colSpan={
                                        isAdmin ? 6 : 5
                                    }
                                >
                                    No employees found.
                                </td>
                            </tr>
                        ) : (
                            employees.map(
                                (employee) => (
                                    <tr
                                        key={
                                            employee.employeeId
                                        }
                                    >
                                        <td>
                                            {
                                                employee.employeeId
                                            }
                                        </td>

                                        <td>
                                            {employee.name}
                                        </td>

                                        <td>
                                            {employee.email}
                                        </td>

                                        <td>
                                            ₹{" "}
                                            {employee.salary}
                                        </td>

                                        <td>
                                            {
                                                employee.departmentName
                                            }
                                        </td>

                                        {isAdmin && (
                                            <td>
                                                <button
                                                    type="button"
                                                    className="small-btn"
                                                    onClick={() =>
                                                        editEmployee(
                                                            employee
                                                        )
                                                    }
                                                >
                                                    Edit
                                                </button>

                                                <button
                                                    type="button"
                                                    className="danger-btn"
                                                    onClick={() =>
                                                        deleteEmployee(
                                                            employee.employeeId
                                                        )
                                                    }
                                                >
                                                    Delete
                                                </button>
                                            </td>
                                        )}
                                    </tr>
                                )
                            )
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default EmployeeManager;