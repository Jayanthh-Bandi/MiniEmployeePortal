import { useState } from "react";
import { apiFetch } from "../services/api";

function Login({
    onLoginSuccess,
    onSwitchRegister
}) {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();

        setError("");
        setLoading(true);

        try {
            const data = await apiFetch(
                "/Auth/login",
                {
                    method: "POST",
                    body: JSON.stringify({
                        email,
                        password
                    })
                }
            );

            sessionStorage.setItem(
                "token",
                data.token
            );

            onLoginSuccess(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-card">
            <div className="brand">
                <h1>
                    Mini Employee Portal
                </h1>

                <p>
                    Sign in to your account
                </p>
            </div>

            <form onSubmit={handleSubmit}>
                <label>Email</label>

                <input
                    type="email"
                    value={email}
                    onChange={(e) =>
                        setEmail(e.target.value)
                    }
                    placeholder="Enter email"
                    required
                />

                <label>Password</label>

                <input
                    type="password"
                    value={password}
                    onChange={(e) =>
                        setPassword(e.target.value)
                    }
                    placeholder="Enter password"
                    required
                />

                <button
                    className="primary-btn"
                    disabled={loading}
                >
                    {loading
                        ? "Signing in..."
                        : "Login"}
                </button>
            </form>

            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}

            <p className="switch-text">
                Don't have an account?

                <button
                    className="link-btn"
                    onClick={onSwitchRegister}
                >
                    Register
                </button>
            </p>
        </div>
    );
}

export default Login;