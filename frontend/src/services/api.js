// const API_BASE_URL = import.meta.env.VITE_API_URL;

// function getErrorMessage(data, response) {
//     if (data?.message) {
//         return data.message;
//     }

//     if (data?.title) {
//         return data.title;
//     }

//     if (data?.errors) {
//         const messages = Object.values(data.errors)
//             .flat()
//             .filter(Boolean);

//         if (messages.length > 0) {
//             return messages.join(" ");
//         }
//     }

//     return `Request failed with status ${response.status}.`;
// }

// export async function apiFetch(endpoint, options = {}) {
//     const token = sessionStorage.getItem("token");

//     const headers = new Headers(options.headers || {});

//     if (options.body && !headers.has("Content-Type")) {
//         headers.set("Content-Type", "application/json");
//     }

//     if (token) {
//         headers.set("Authorization", `Bearer ${token}`);
//     }

//     let response;

//     try {
//         response = await fetch(
//             `${API_BASE_URL}${endpoint}`,
//             {
//                 ...options,
//                 headers
//             }
//         );
//     } catch (error) {
//         throw new Error(
//             "Cannot connect to the backend API."
//         );
//     }

//     if (response.status === 401) {
//         sessionStorage.removeItem("token");

//         throw new Error(
//             "Your session has expired. Please login again."
//         );
//     }

//     if (response.status === 403) {
//         throw new Error(
//             "You do not have permission to perform this operation."
//         );
//     }

//     if (response.status === 204) {
//         return null;
//     }

//     const text = await response.text();

//     let data = null;

//     if (text) {
//         try {
//             data = JSON.parse(text);
//         } catch {
//             data = text;
//         }
//     }

//     if (!response.ok) {
//         throw new Error(
//             getErrorMessage(data, response)
//         );
//     }

//     return data;
// }

const API_BASE_URL = import.meta.env.VITE_API_URL;

/*
 * Extract a useful error message from the ASP.NET Core API response.
 */
function getErrorMessage(data, response) {
    // Our custom ExceptionHandlingMiddleware
    if (data?.message) {
        return data.message;
    }

    // ASP.NET Core ProblemDetails
    if (data?.title) {
        return data.title;
    }

    // ASP.NET Core validation errors
    if (data?.errors) {
        const messages = Object.values(data.errors)
            .flat()
            .filter(Boolean);

        if (messages.length > 0) {
            return messages.join(" ");
        }
    }

    // API returned plain text
    if (typeof data === "string" && data.trim()) {
        return data;
    }

    return `Request failed with status ${response.status}.`;
}

/*
 * Common API function used by all React components.
 *
 * Automatically:
 * - Adds JSON Content-Type when a body exists
 * - Adds JWT Authorization header
 * - Handles 401 / 403
 * - Handles 204 No Content
 * - Parses JSON responses
 * - Displays backend validation/business errors
 * - Prevents stale GET responses with cache: no-store
 */
export async function apiFetch(endpoint, options = {}) {
    const token = sessionStorage.getItem("token");

    const headers = new Headers(options.headers || {});

    // Add JSON content type when request has a body
    if (
        options.body &&
        !headers.has("Content-Type")
    ) {
        headers.set(
            "Content-Type",
            "application/json"
        );
    }

    // Add JWT automatically
    if (token) {
        headers.set(
            "Authorization",
            `Bearer ${token}`
        );
    }

    let response;

    try {
        response = await fetch(
            `${API_BASE_URL}${endpoint}`,
            {
                ...options,

                // Always fetch fresh data
                cache: "no-store",

                headers
            }
        );
    } catch (error) {
        console.error(
            "API connection error:",
            error
        );

        throw new Error(
            "Cannot connect to the backend API. Make sure the ASP.NET Core server is running."
        );
    }

    /*
     * 401 = authentication problem
     *
     * Token may be missing, expired or invalid.
     */
    if (response.status === 401) {
        sessionStorage.removeItem("token");

        throw new Error(
            "Your session has expired or your token is invalid. Please login again."
        );
    }

    /*
     * 403 = authorization problem
     *
     * User is authenticated but doesn't have
     * permission for the operation.
     */
    if (response.status === 403) {
        throw new Error(
            "You do not have permission to perform this operation."
        );
    }

    /*
     * DELETE endpoints commonly return 204.
     * There is no response body to parse.
     */
    if (response.status === 204) {
        return null;
    }

    /*
     * Read the response as text first.
     *
     * This allows us to handle:
     * - JSON
     * - ProblemDetails
     * - Validation errors
     * - Plain text
     * - Empty responses
     */
    const text = await response.text();

    let data = null;

    if (text) {
        try {
            data = JSON.parse(text);
        } catch {
            data = text;
        }
    }

    /*
     * Any status outside 200-299
     * reaches this block.
     */
    if (!response.ok) {
        const message = getErrorMessage(
            data,
            response
        );

        console.error(
            "API Error:",
            {
                endpoint,
                method: options.method || "GET",
                status: response.status,
                response: data
            }
        );

        throw new Error(message);
    }

    return data;
}