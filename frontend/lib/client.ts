export class ApiError extends Error {
    status: number;
    message: string;

    constructor(status: number, message: string) {
        super(message);
        this.status = status;
        this.message = message;
        this.name = 'ApiError';
    }
}


export async function apiFetch<T>(url: string, signal?: AbortSignal) : Promise<T> {
    const response = await fetch(url, { signal });
    if (!response.ok) {
        const errorText = await response.text();
        throw new ApiError(response.status, `${url} responded ${response.status}: ${errorText}`);
    }
    return response.json() as Promise<T>;
}