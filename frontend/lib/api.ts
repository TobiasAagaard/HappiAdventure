import { apiFetch } from './client';
import type { Activity, Place } from './types';

export function getActivities(signal?: AbortSignal) {
    return apiFetch<Activity[]>('/api/activities', signal);
}

export function getPlaces(activityCode?: string, signal?: AbortSignal) {
  const query = activityCode ? `?activity=${encodeURIComponent(activityCode)}` : ''
  return apiFetch<Place[]>(`/api/places${query}`, signal);
}