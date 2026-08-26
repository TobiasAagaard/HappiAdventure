import { useEffect, useState } from 'react'
import { getActivities } from '../lib/api'
import type { Activity } from '../lib/types'


export function useActivities() {
  const [activities, setActivities] = useState<Activity[]>([])
  const [error, setError] = useState<string | null>(null)


  useEffect(() => {
    const controller = new AbortController()
    getActivities(controller.signal)
      .then(setActivities)
      .catch((e: unknown) => {
        if (!controller.signal.aborted) setError(e instanceof Error ? e.message : String(e))
      })
    return () => controller.abort()
  }, [])

  return { activities, error }
}