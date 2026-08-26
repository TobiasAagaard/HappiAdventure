import { useState } from 'react'
import { ActivityPicker } from './components/ActivityPicker'
import { PlaceMap } from './components/PlaceMap'
import { useActivities } from './hooks/useActivities'
import { usePlaces } from './hooks/usePlaces'
import type { Place } from '../lib/types'

function App() {
  const [selectedActivity, setSelectedActivity] = useState<string | null>(null)
  const [selectedPlace, setSelectedPlace] = useState<Place | null>(null)

  const { activities, error: activitiesError } = useActivities()
  const { places, loading, error: placesError } = usePlaces(selectedActivity)

  const error = activitiesError ?? placesError

  return (
    <div className="app">
      <PlaceMap
        places={places}
        selectedPlace={selectedPlace}
        onSelectPlace={setSelectedPlace}
      />

      <aside className="panel">
        <h1 className="panel__brand">
          Happi<span>Adventure</span>
        </h1>
        <p className="panel__question">What do you want to do?</p>

        <ActivityPicker
          activities={activities}
          selected={selectedActivity}
          onSelect={(code) => {
            setSelectedActivity(code)
            setSelectedPlace(null)
          }}
        />

        <p className="panel__status">
          {error
            ? 'Could not reach the API — is it running?'
            : loading
              ? 'Loading places…'
              : `${places.length} place${places.length === 1 ? '' : 's'} in Aalborg`}
        </p>
      </aside>
    </div>
  )
}

export default App