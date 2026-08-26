export class ApiError extends Error {
    constructor(public status: number, public message: string) {
        super(message);
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