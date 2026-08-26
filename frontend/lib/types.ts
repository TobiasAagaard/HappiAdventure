
export interface Activity {
    id: number;
    code: string;
    name: string;
}

export interface Location {
    latitude: number;
    longitude: number;
}

export interface Place {
    id: number;
    name: string;
    location: Location;
    priceLevel: number;
}