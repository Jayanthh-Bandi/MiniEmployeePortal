import { useEffect, useState } from "react";
import { apiFetch } from "../services/api";

function UserManager() {
    const [users, setUsers] = useState([]);
    const [message, setMessage] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(true);

    const loadUsers = async () => {
        setLoading(true);
        setError("");

        try {
            const data =
                await apiFetch("/Auth/users");

            setUsers(data || []);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    const updateRole = async (userId, role) => {
        try {
            await apiFetch(
                `/Auth/users/${userId}/role`,
                {
                    method: "PUT",
                    body: JSON.stringify({ role })
                }
            );

            setMessage(
                "Role changed successfully. The user must login again to receive a new JWT."
            );

            await loadUsers();
        } catch (error) {
            setError(error.message);
        }
    };

    const deleteUser = async (userId) => {
        if (!window.confirm(
            "Delete this user and linked employee record?"
        )) {
            return;
        }

        try {
            await apiFetch(
                `/Auth/users/${userId}`,
                {
                    method: "DELETE"
                }
            );

            setMessage(
                "User and linked employee deleted."
            );

            await loadUsers();
        } catch (error) {
            setError(error.message);
        }
    };

    if (loading) {
        return (
            <div className="content-card">
                Loading users...
            </div>
        );
    }

    return (
        <div className="content-card">
            <div className="section-header">
                <div>
                    <h2>User Management</h2>
                    <p>
                        Admin-only account and role management.
                    </p>
                </div>

                <button
                    className="secondary-btn"
                    onClick={loadUsers}
                >
                    Refresh
                </button>
            </div>

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

            <div className="table-wrapper">
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>User</th>
                            <th>Email</th>
                            <th>Role</th>
                            <th>Actions</th>
                        </tr>
                    </thead>

                    <tbody>
                        {users.map((user) => (
                            <tr key={user.userId}>
                                <td>
                                    {user.userId}
                                </td>

                                <td>
                                    {user.userName}
                                </td>

                                <td>
                                    {user.email}
                                </td>

                                <td>
                                    <span
                                        className={
                                            user.role
                                                ?.toLowerCase() ===
                                            "admin"
                                                ? "role-badge admin"
                                                : "role-badge employee"
                                        }
                                    >
                                        {user.role ||
                                            "No Role"}
                                    </span>
                                </td>

                                <td>
                                    <select
                                        value={
                                            user.role ||
                                            "Employee"
                                        }
                                        onChange={(e) =>
                                            updateRole(
                                                user.userId,
                                                e.target.value
                                            )
                                        }
                                    >
                                        <option value="Employee">
                                            Employee
                                        </option>

                                        <option value="Admin">
                                            Admin
                                        </option>
                                    </select>

                                    <button
                                        className="danger-btn"
                                        onClick={() =>
                                            deleteUser(
                                                user.userId
                                            )
                                        }
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default UserManager;